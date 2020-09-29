using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShot : MonoBehaviour
{
    public float m_Speed = 20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }
}
