﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtyWindow : MonoBehaviour
{
    private int m_HitCounter = 0;
    private int m_NumOfHitsToClean;

    // Start is called before the first frame update
    void Start()
    {
        System.Random rand = new System.Random();
        m_NumOfHitsToClean = rand.Next(1, 3);
    }

    void OnCollisionEnter(Collision i_Collision)
    {
        m_HitCounter++;

        if (m_HitCounter == m_NumOfHitsToClean)
        {
            MeshRenderer myMesh = GetComponent<MeshRenderer>();
            myMesh.enabled = false;
            Destroy(this);
        }
    }
}
