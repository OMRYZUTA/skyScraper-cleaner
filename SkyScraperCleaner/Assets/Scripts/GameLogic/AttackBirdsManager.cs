using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBirdsManager : MonoBehaviour
{
    public GameObject m_AttackBird;
    public float m_SpawnTime = 20f;
    [SerializeField] private int m_BirdsNum;
    private int m_MaxBirdsNum= 3;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAttackBird", m_SpawnTime, m_SpawnTime);
    }
    void SpawnAttackBird()
    {
        if (m_BirdsNum < m_MaxBirdsNum)
        {
            var newBird = GameObject.Instantiate(m_AttackBird);
            m_BirdsNum++;
        }
    }
}
