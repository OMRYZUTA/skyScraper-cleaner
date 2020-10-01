using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBirdsManager : MonoBehaviour
{
    public GameObject attackBird;
    public float spawnTime = 20f;
    [SerializeField] public Transform lookAtMe;
    [SerializeField]
    private int m_BirdsNum ;
    private int m_MaxBirdsNum= 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAttackBird", spawnTime, spawnTime);
    }

   


    void SpawnAttackBird()
    {
        if(m_BirdsNum < m_MaxBirdsNum)
        {
            var newBird = GameObject.Instantiate(attackBird);
            m_BirdsNum++;
        }
    }
}
