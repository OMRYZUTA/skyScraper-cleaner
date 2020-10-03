using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighScoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    private void Awake()
    {
        entryContainer = transform.Find("EntryContainer");
        entryTemplate = entryContainer.Find("HighScoreEntryTemplate");
        entryTemplate.gameObject.SetActive(false);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
       
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
        highscoreEntryTransformList = new List<Transform>();
        foreach (var highscore in highscores.highscoreEntryList)
        {
            if(highscore != null)
            {
                CreateHighScoreEntryTransform(highscore, entryContainer, highscoreEntryTransformList);
            }
        }
    }

    private void resetHighScoreTable(Highscores highscores)
    {
        
        for(int i = 5; i < highscores.highscoreEntryList.Count; i++)
        {
            highscores.highscoreEntryList.Remove(highscores.highscoreEntryList[i]);
        }

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void CreateHighScoreEntryTransform(
        HighScoreEntry highscoreEntry,
        Transform container,
        List<Transform> transformList)
    {

        float templateHeight = 0.04f;
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
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

        entryTransform.Find("ScoreTemplate").GetComponent<Text>().text = highscoreEntry.m_Score.ToString();

        entryTransform.Find("NameTemplate").GetComponent<Text>().text = highscoreEntry.m_Name;
        transformList.Add(entryTransform);
    }

    public void AddHighscoreEntry(int i_Score )
    {
        //create highscore entry
        HighScoreEntry highscoreEntry = new HighScoreEntry(i_Score);
        // load saved highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        // add new highscore
        highscores.highscoreEntryList.Add(highscoreEntry);
        //save updated score
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores
    {
        public List<HighScoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighScoreEntry
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


