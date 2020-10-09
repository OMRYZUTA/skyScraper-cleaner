using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie = 2;
        private int m_SideFactor = 2;
        private bool m_IsLeft;
        public AudioSource m_AttackSound;
        [SerializeField] public Transform m_LookAtMe;

        void Start()
        {
            m_LookAtMe = GameObject.Find("Player").transform;
            System.Random rand= new System.Random();
            int isLeft = rand.Next(1, 3);

            if (isLeft == 2)
            {
                m_IsLeft = true;
                m_SideFactor = -2;
            }
            m_AttackSound = GetComponent<AudioSource>();
            PlayeEagleSound();
        }

        public void PlayeEagleSound()
        {
            m_AttackSound.Play();
        }

        void Update()
        {
            Vector3 lookAt = new Vector3();
            lookAt = m_LookAtMe.transform.position;
            lookAt.x -= 0.3f;

            transform.position = new Vector3(
                m_LookAtMe.transform.position.x + m_SideFactor,
                m_LookAtMe.transform.position.y,
                m_LookAtMe.transform.position.z - 2);
            transform.LookAt(lookAt);
        }

        void OnCollisionEnter(Collision i_Collision)
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
