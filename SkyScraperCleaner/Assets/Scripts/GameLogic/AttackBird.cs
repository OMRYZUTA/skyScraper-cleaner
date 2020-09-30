using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie = 2;
        public Animator dieAnimation;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnCollisionEnter(Collision collision)
        {
            m_ShotsCounter++;

            if (m_ShotsCounter == m_NumOfShotsToDie)
            {
                Debug.Log("killed");
                Destroy(this);
            }
        }
    }
}
