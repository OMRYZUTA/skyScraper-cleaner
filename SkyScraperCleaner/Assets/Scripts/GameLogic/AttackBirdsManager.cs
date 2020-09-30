using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBirdsManager : MonoBehaviour
{
    public GameObject attackBird;
    public float spawnTime = 12f;
    [SerializeField] public Transform lookAtMe;
    [SerializeField]
    private int m_BirdsNum ;
    private int m_MaxBirdsNum= 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAttackBird", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnAttackBird()
    {
        var newBird = GameObject.Instantiate(attackBird);
        if(m_BirdsNum < m_MaxBirdsNum)
        {
            Vector3 lookAt = new Vector3();
            lookAt = lookAtMe.transform.position;
            lookAt.x -= 0.3f;

            newBird.transform.position = new Vector3(
                lookAtMe.transform.position.x + 2,
                lookAtMe.transform.position.y,
                lookAtMe.transform.position.z - 2);
            newBird.transform.LookAt(lookAt);
            m_BirdsNum++;
        }
    }
}
