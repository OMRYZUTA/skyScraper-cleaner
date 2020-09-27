using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject m_AreYouSurePopUp;
    [SerializeField] GameObject m_HowToPlayPopUp;
    public void PlayerWantsToExit()
    {
        m_AreYouSurePopUp.SetActive(true);
    }
    public void ShowInstructions()
    {
        m_HowToPlayPopUp.SetActive(true);
    }
    public void PlayerSureAboutExit()
    {
        Application.Quit();
    }
    public void PlayerWantsToStay()
    {
        m_AreYouSurePopUp.SetActive(false);
    }
    public void CloseHowToPlayPopUp()
    {
        m_HowToPlayPopUp.SetActive(false);
    }
}
