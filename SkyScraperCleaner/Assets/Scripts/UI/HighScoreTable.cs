using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform m_EntryContainer;
    private Transform m_EntryTemplate;
    private List<Transform> m_HighscoreEntryTransformList;
    private void Awake()
    {
        m_EntryContainer = transform.Find("EntryContainer");
        m_EntryTemplate = m_EntryContainer.Find("HighScoreEntryTemplate");
        m_EntryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) 
        {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            AddHighscoreEntry(0);
            AddHighscoreEntry(0);
            AddHighscoreEntry(0);
            AddHighscoreEntry(0);

            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }
        // bubble sort the Table
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].m_Score > highscores.highscoreEntryList[i].m_Score)
                {
                    HighScoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        resetHighScoreTable(highscores);
        m_HighscoreEntryTransformList = new List<Transform>();

        foreach (var highscore in highscores.highscoreEntryList)
        {
            if (highscore != null)
            {
                CreateHighScoreEntryTransform(highscore, m_EntryContainer, m_HighscoreEntryTransformList);
            }
        }
    }

    private void resetHighScoreTable(Highscores i_Highscores)
    {
        for (int i = 5; i < i_Highscores.highscoreEntryList.Count; i++)
        {
            i_Highscores.highscoreEntryList.Remove(i_Highscores.highscoreEntryList[i]);
        }

        string json = JsonUtility.ToJson(i_Highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighScoreEntryTransform(
        HighScoreEntry i_HighscoreEntry,
        Transform i_Container,
        List<Transform> i_TransformList)
    {
        float templateHeight = 0.03f;
        Transform entryTransform = Instantiate(m_EntryTemplate, m_EntryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i_TransformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = i_TransformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default:
                rankString = rank + "TH";
                break;
            case 1:
                rankString = "1ST";
                break;
            case 2:
                rankString = "2ND";
                break;
            case 3:
                rankString = "3RD";
                break;
        }

        entryTransform.Find("PosTemplate").GetComponent<Text>().text = rankString;
        entryTransform.Find("ScoreTemplate").GetComponent<Text>().text = i_HighscoreEntry.m_Score.ToString();
        entryTransform.Find("NameTemplate").GetComponent<Text>().text = i_HighscoreEntry.m_Name;
        i_TransformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int i_Score)
    {
        //create highscore entry
        HighScoreEntry highscoreEntry = new HighScoreEntry(i_Score);
        // load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        // add new highscore
        if (highscores == null) 
        {
            // There's no stored table, initialize
            highscores = new Highscores() {highscoreEntryList = new List<HighScoreEntry>()};
        }

        highscores.highscoreEntryList.Add(highscoreEntry);
        //save updated score
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public class Highscores
    {
        public List<HighScoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighScoreEntry
    {
        public int m_Score;
        public string m_Name;

        public HighScoreEntry(int i_Score)
        {
            m_Score = i_Score;
            m_Name = DateTime.Now.ToString();
        }
    }
}


