using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishWindow : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    [SerializeField]
    private ScoreManager m_ScoreManager;

    void OnTriggerEnter(Collider i_OtherCollider)
    {
        if(i_OtherCollider.tag == "Player")
        {
            GetComponent<MeshRenderer>().enabled = true;
            m_Player.IsGameOver = true;
            m_ScoreManager.TrySaveResult();
         //   SceneManager.LoadScene("WelcomeScene");
        }

  
    }

}
