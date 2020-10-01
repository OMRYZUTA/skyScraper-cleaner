using System.Security.Cryptography;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class AttackBird : MonoBehaviour
    {
        private int m_ShotsCounter = 0;
        private int m_NumOfShotsToDie = 2;
        private bool m_IsLeft;
        public Animator dieAnimation;
        public AudioSource m_AttackSound;
        [SerializeField] public Transform lookAtMe;
        private  int m_SideFactor = 2;

        void Start()
        {
            lookAtMe = GameObject.Find("Player").transform;
            System.Random rand= new System.Random();
            int isLeft = rand.Next(1, 3);
            if(isLeft == 2)
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
            lookAt = lookAtMe.transform.position;
            lookAt.x -= 0.3f;

            transform.position = new Vector3(
                lookAtMe.transform.position.x + m_SideFactor,
                lookAtMe.transform.position.y,
                lookAtMe.transform.position.z - 2);
            transform.LookAt(lookAt);

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
