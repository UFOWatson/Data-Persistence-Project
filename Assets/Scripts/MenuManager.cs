using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public Text playerNameText;
    public string playerName;
    public static MenuManager Instance;

    private Transform entryContainer;
    private Transform entryTemplate;
    private List<Transform> highscorentryTransformList;


    private void Awake()
    {
       
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
       Application.Quit();
#endif  

    }
   


    public void AddHighscoreEntry(int score)
    {          
        Highscores highscores = LoadSave();

        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = playerName };
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            if (highscores.highscoreEntryList[i].name == playerName)
            {
                highscores.highscoreEntryList[i].score = score;
                break;
            }
            else if (true)
            {
                highscores.highscoreEntryList.Add(highscoreEntry);
                break;
            }
        }       
        string writeJson = JsonUtility.ToJson(highscores);
        File.WriteAllText(Application.persistentDataPath + "/playerNamefile.json", writeJson);
        
    }

    private Highscores LoadSave()
    {
        string path = Application.persistentDataPath + "/playerNamefile.json";
        if (File.Exists(path))
        {
            string readJson = File.ReadAllText(path);
            Highscores highscores = JsonUtility.FromJson<Highscores>(readJson);
            return highscores;
        }
        else
        {
            Highscores highscores = new Highscores();
            return highscores;
        }
    }
  

    public void ScoreBoardScene()
    {
        SceneManager.LoadScene(2);
    }

    public void StartNew()
    {
        playerName = playerNameText.text;
        SceneManager.LoadScene(1);
    }

    public void LoadScoreBoard()
    {
        Highscores highscores = LoadSave();

        if (highscores.highscoreEntryList.Count > 0)
            {
                if (entryContainer == null)
                {
                    entryContainer = GameObject.Find("highscoreEntryContainer").transform;
                }
                if (entryTemplate == null)
                {
                    entryTemplate = GameObject.Find("highscoreEntryTemplate").transform;
                }


                entryTemplate.gameObject.SetActive(false);


                highscorentryTransformList = new List<Transform>();
                foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
                {
                    CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscorentryTransformList);
                }
            }
        
        
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);


        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3ND"; break;
        }

        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;


        transformList.Add(entryTransform);
    }

    public void Sort()
    {
        Highscores highscores = LoadSave();

        if (highscores.highscoreEntryList.Count > 0)
        {
            //Sort entry list by Score
            for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                    {
                        //Swap

                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }
        }       
    }

    public int LoadBestScore()
    {
        Highscores highscores = LoadSave();
        if (highscores.highscoreEntryList.Count > 0)
        {
            int bestScore = highscores.highscoreEntryList[0].score;
            return bestScore;
        }
        else
        {
            return 0;
        }
    }


    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }
}
