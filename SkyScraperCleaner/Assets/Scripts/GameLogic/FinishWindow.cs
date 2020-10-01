using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishWindow : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;


    void OnTriggerEnter(Collider i_OtherCollider)
    {
        if(i_OtherCollider.tag == "Player")
        {
            GetComponent<MeshRenderer>().enabled = true;
        }

        m_Player.IsGameOver = true;
    }

}
