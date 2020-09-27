using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject m_AreYouSurePopUp;
    [SerializeField] GameObject m_HowToPlayPopUp;
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void PopUpHowToPlay()
    {
        m_HowToPlayPopUp.SetActive(true);
    }
    public void Exit()
    {
        m_AreYouSurePopUp.SetActive(true);
    }
}
