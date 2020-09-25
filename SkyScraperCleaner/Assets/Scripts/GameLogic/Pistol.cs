using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{

    [SerializeField]
    private Player m_Player;
    private bool m_IsBirdHit;

    // Start is called before the first frame update
    void Start()
    {
        m_IsBirdHit = false;
        m_Player.ReportBirdHit += Player_ReportBirdHit;
    }

    private void Player_ReportBirdHit(GameObject obj)
    {
        m_IsBirdHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsBirdHit)
        {

            if (Input.anyKeyDown)
            {
                Debug.Log("Shooting");
            }
        }
    }
}
