using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public static bool collision;
    public bool ocupado;

    private void OnTriggerEnter(Collider other) {
        ocupado = other.CompareTag("Wall");
        collision = true;
    }

    private void OnTriggerExit(Collider other) {
        ocupado = false;
    }
}
