using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterPistol : MonoBehaviour
{
    
    [SerializeField]
    private Player m_Player;
    private bool m_IsWindowHit;

    [SerializeField]
    private GameObject m_Shot;
    // Start is called before the first frame update
    void Start()
    {
        m_IsWindowHit = false;
        m_Player.ReportBirdHit += M_Player_ReportBirdHit;
        m_Player.ReportBuildingHit += Player_ReportBuildingHit;
        m_Player.ReportWindowHit += Player_ReportWindowHit;
    }

    private void Player_ReportWindowHit(GameObject obj)
    {
        m_IsWindowHit = true;
    }

    private void Player_ReportBuildingHit(GameObject obj)
    {
        m_IsWindowHit = false;
    }

    private void M_Player_ReportBirdHit(GameObject obj)
    {
        m_IsWindowHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsWindowHit)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("Splahing water!");
                Vector3 createInPosition = transform.position -transform.forward;
                createInPosition.y += 1;
                GameObject clone =Instantiate(m_Shot, createInPosition , Quaternion.identity);
                clone.GetComponent<Rigidbody>().AddForce(-transform.forward * 800);
                 Destroy (clone, 1.0f);
            }
        }
    }
}
