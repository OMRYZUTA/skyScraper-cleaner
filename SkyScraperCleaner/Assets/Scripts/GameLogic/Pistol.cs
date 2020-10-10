using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    [SerializeField] private Player m_Player;
    [SerializeField] private GameObject m_Camera;
    [SerializeField] private GameObject m_Shot;
    private bool m_IsBirdHit;

    // Start is called before the first frame update
    void Start()
    {
        m_IsBirdHit = false;
        m_Player.ReportBirdHit += Player_ReportBirdHit;
        m_Player.ReportBuildingHit += Player_ReportBuildingHit;
        m_Player.ReportWindowHit += Player_ReportWindowHit;
    }

    private void Player_ReportBirdHit(GameObject i_Obj)
    {
        m_IsBirdHit = true;
    }

    private void Player_ReportWindowHit(GameObject i_Obj)
    {
        m_IsBirdHit = false;
    }

    private void Player_ReportBuildingHit(GameObject i_Obj)
    {
        m_IsBirdHit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsBirdHit)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("shooting bullet!!");
                GameObject Bullet = Instantiate(m_Shot);
                Bullet.transform.position = m_Camera.transform.position + 2 * m_Camera.transform.forward;
                Bullet.transform.forward = m_Camera.transform.forward;
                Destroy(Bullet, 2.0f);
            }
        }       
    }
}
