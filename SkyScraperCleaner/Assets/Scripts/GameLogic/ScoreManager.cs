using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private Player m_Player;

    private int m_Score;

    // Start is called before the first frame update
    void Start()
    {
        m_Player.ReportWindowHit += Player_ReportWindowHit;
        m_Player.ReportBirdHit += Player_ReportBirdHit;
    }

    private void Player_ReportBirdHit(GameObject i_Obj)
    {
        m_Score += 500;
    }

    private void Player_ReportWindowHit(GameObject i_Obj)
    {
        m_Score += 100;
    }

    public void TrySaveResult()
    {
        m_Score += (int)-m_Player.PlayTime * 50;
        HighScoreTable hT = new HighScoreTable();
        hT.AddHighscoreEntry(m_Score);
    }

}
