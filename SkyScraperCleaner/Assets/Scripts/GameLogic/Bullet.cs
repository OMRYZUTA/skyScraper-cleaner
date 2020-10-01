using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_Speed = 60f;
    
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_Speed * Time.deltaTime;
    }
}
