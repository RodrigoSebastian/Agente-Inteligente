using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject floor;
    [SerializeField]
    private GameObject wall;

    void Start(){
        for (int z = -1; z <= 11; z++)
        {
            for (int x = -1; x <= 12; x++)
            {
                if((z == -1 || z == 11) || (x == -1 || (x == 12 && (z <= 3 || z >= 7)))){
                    Instantiate(wall,new Vector3(x,0.5f,z),Quaternion.identity,gameObject.transform);
                }
                else{
                    if((x >= 5 && x <= 8 && z <= 2) || 
                    (z == 7 && x >= 3 && x <= 8) || 
                    (z >= 5 && z <= 6 && (x >= 3 && x <= 4 || x >= 7 && x <= 8))
                    )
                        Instantiate(wall,new Vector3(x,0.5f,z),Quaternion.identity,gameObject.transform);
                    else{
                        if(z >= 4 && z <= 6)
                            for (int te = x+1; te < x+3; te++)
                                Instantiate(floor,new Vector3(te,0,z),Quaternion.identity,gameObject.transform);
                        Instantiate(floor,new Vector3(x,0,z),Quaternion.identity,gameObject.transform);
                    }
                }
            }
        }
    }

    void Update(){
        
    }
}
