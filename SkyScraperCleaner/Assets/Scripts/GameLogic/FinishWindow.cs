using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishWindow : MonoBehaviour
{
   


    void OnTriggerEnter(Collider i_OtherCollider)
    {
        if(i_OtherCollider.tag == "Player")
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        Debug.Log("Start Scene");
    }

}
