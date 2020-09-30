using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie;
        public Animator dieAnimation;

        // Start is called before the first frame update
        void Start()
        {
            System.Random rand = new System.Random();
            m_NumOfShotsToDie = rand.Next(1, 5);
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
                dieAnimation.SetTrigger("die");
            }
        }
    }
}
