using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgenteInteligente : MonoBehaviour
{
    private enum Estados { PARED, E_IZQ, ES_IZQ, E_DER, ES_DER, PS_IZQ, PS_DER, S_PASILLO, LIBRE };
    private Dictionary<Estados, List<int>> background = new Dictionary<Estados, List<int>>();

    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Estados estadoActual;
    [SerializeField]
    private Estados estadoAnterior;

    [SerializeField]
    private Colliders[] colliders;

    [SerializeField]
    private int cantidadPasos;
    [SerializeField]
    private bool automatic;

    [SerializeField]
    private float velocidad = 0.3f;

    private void Awake()
    {
        background.Add(Estados.PARED, new List<int>() { 0, 3, 5 });
        background.Add(Estados.E_IZQ, new List<int>() { 5 });
        background.Add(Estados.PS_IZQ, new List<int>() { 0, 5 });
        background.Add(Estados.PS_DER, new List<int>() { 2, 7 });
        background.Add(Estados.E_DER, new List<int>() { 7 });
        background.Add(Estados.ES_DER, new List<int>() { 2 });
        background.Add(Estados.ES_IZQ, new List<int>() { 0 });
        background.Add(Estados.S_PASILLO, new List<int>() { 5, 7 });
    }

    Estados GetKeyFromValue(List<int> valueVar)
    {
        foreach (Estados keyVar in background.Keys)
        {
            if (background[keyVar].SequenceEqual(valueVar))
            {
                return keyVar;
            }
        }
        return Estados.LIBRE;
    }

    void Start()
    {
        position = transform.position;
        colliders = transform.GetChild(2).transform.GetComponentsInChildren<Colliders>();

        if (automatic)
            StartCoroutine(AutomaticMove());
    }

    IEnumerator AutomaticMove()
    {
        for (int i = 0; i <= cantidadPasos; i++)
        {
            Rotar();

            if (i > 0)
                Move();

            yield return new WaitForSeconds(velocidad);
        }
    }

    void Move()
    {
        Debug.Log("Me muevo");

        position = Vector3.zero;
        position = transform.forward;
        position.y = 0;
        position.Normalize();

        transform.position += position;
    }

    void Rotar()
    {

        //! Prioridades
        //! 1. Salir del pasillo -> Da preferencia a girar al lado contrario de donde viene
        //! 2. Girar derecha
        //! 3. Girar izquierda

        if (estadoActual != Estados.LIBRE)
            estadoAnterior = estadoActual;

        estadoActual = ValidarCollisions();
        Debug.Log("Verifico estado: " + estadoActual);

        if (estadoActual == Estados.S_PASILLO)
        {
            /*
            
                ! Si estadoAnterior == PS_IZQ O estadoAnterior == IZQ ENTONCES:
                !           muevo (izq);
                ! Si estadoAnterior == PS_DER O estadoAnterior == DER ENTONCES:
                !           muevo (derecha);
            
            */
            if (estadoAnterior == Estados.PS_DER || estadoAnterior == Estados.E_DER || estadoAnterior == Estados.PARED)
                transform.rotation *= Quaternion.Euler(0, 90, 0);
            else if (estadoAnterior == Estados.PS_IZQ || estadoAnterior == Estados.E_IZQ || estadoAnterior == Estados.PARED)
                transform.rotation *= Quaternion.Euler(0, -90, 0);
        }

        if (colliders[1].ocupado)
        {
            if (!colliders[4].ocupado)
                transform.rotation *= Quaternion.Euler(0, 90, 0);
            else if (!colliders[3].ocupado)
                transform.rotation *= Quaternion.Euler(0, -90, 0);
            else
                transform.rotation *= Quaternion.Euler(0, 180, 0);
        }

        switch (estadoActual)
        {
            case Estados.PS_IZQ:
            case Estados.ES_IZQ:
            case Estados.E_IZQ:
                transform.rotation *= Quaternion.Euler(0, -90, 0);
                break;
            case Estados.PS_DER:
            case Estados.ES_DER:
            case Estados.E_DER:
                transform.rotation *= Quaternion.Euler(0, 90, 0);
                break;
        }
    }

    Estados ValidarCollisions()
    {
        List<int> inds = new List<int>();

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].ocupado)
                inds.Add(i);
        }

        return GetKeyFromValue(inds);
    }
}