using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie = 2;
        public Animator dieAnimation;


        void OnCollisionEnter(Collision collision)
        {
            m_ShotsCounter++;
            Debug.Log("Enter Collision");
            if (m_ShotsCounter == m_NumOfShotsToDie)
            {
                GetComponent<Animator>().SetBool("m_IsDead",true);
                Debug.Log("killed");
                Destroy(this.gameObject,3);
            }
        }
    }
}
