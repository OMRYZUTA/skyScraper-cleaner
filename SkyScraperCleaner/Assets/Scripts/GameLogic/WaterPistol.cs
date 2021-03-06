﻿using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class WaterPistol : MonoBehaviour
    {

        [SerializeField] private Player m_Player;
        [SerializeField] private GameObject m_Camera;
        [SerializeField] private GameObject m_Shot;
        private bool m_IsWindowHit;

        // Start is called before the first frame update
        void Start()
        {
            m_IsWindowHit = false;
            m_Player.ReportBirdHit += M_Player_ReportBirdHit;
            m_Player.ReportBuildingHit += Player_ReportBuildingHit;
            m_Player.ReportWindowHit += Player_ReportWindowHit;
        }

        private void Player_ReportWindowHit(GameObject i_Obj)
        {
            m_IsWindowHit = true;
        }

        private void Player_ReportBuildingHit(GameObject i_Obj)
        {
            m_IsWindowHit = false;
        }

        private void M_Player_ReportBirdHit(GameObject i_Obj)
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
                    GameObject WaterShot = Instantiate(m_Shot);
                    WaterShot.transform.position = m_Camera.transform.position + 3 * m_Camera.transform.forward;
                    WaterShot.transform.forward = m_Camera.transform.forward;
                    Destroy(WaterShot, 1.5f);
                }
            }
        }
    }
}
