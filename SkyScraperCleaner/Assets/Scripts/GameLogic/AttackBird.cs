using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie = 2;
        public Animator dieAnimation;
        public AudioSource m_AttackSound;

        void Start()
        {
            m_AttackSound = GetComponent<AudioSource>();
        }

        public void PlayeEagleSound()
        {
            m_AttackSound.Play();
        }

        void OnCollisionEnter(Collision collision)
        {
            m_ShotsCounter++;
            Debug.Log("Enter Collision");
            if (m_ShotsCounter == m_NumOfShotsToDie)
            {
                GetComponent<Animator>().SetBool("m_IsDead", true);
                Destroy(m_AttackSound);
                Destroy(this.gameObject, 3);
            }
        }
    }
}
