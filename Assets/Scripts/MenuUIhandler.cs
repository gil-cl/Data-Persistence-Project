using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR // Moet erin omdat anders fout zal geven dat deze bib niet vindt bij het runnen op iets anders dan de unity editor. 
using UnityEditor;
#endif
using TMPro;
using System.IO; 

public class MenuUIhandler : MonoBehaviour
{
    // Gebruikt om code op te slaan naar volgende scene! 
    public static MenuUIhandler Instance;
    [SerializeField] private TMP_InputField typePlayerName;
    [SerializeField] private TMP_Text bestScoreText;

    public string currentPlayerName = "";  // Leeg starten. 
    private string m_highScorePlayerName = "";
    public int m_highScore = 0;

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
    // Start is called before the first frame update
    void Start()
    {
        typePlayerName.onEndEdit.AddListener(delegate { OnEndEdit(typePlayerName); });

        LoadHighscores(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        if (currentPlayerName != "")
        {
            SceneManager.LoadScene(1);
        }

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void OnEndEdit(TMP_InputField name)
    {
        currentPlayerName = name.text;
    }


    // Properties: 

    public string HighScorePlayerName
    {
        get
        {
            return (m_highScorePlayerName == "" ? currentPlayerName : m_highScorePlayerName);
        }
        set
        {
            m_highScorePlayerName = value;
        }

    }

    public int HighScore
    {
        get
        {
            return m_highScore;
        }
        set
        {
            m_highScore = value;
            HighScorePlayerName = currentPlayerName;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highscore; 


    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.playerName = HighScorePlayerName;
        data.highscore = HighScore;


        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }

    public void LoadHighscores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_highScorePlayerName = data.playerName;
            m_highScore = data.highscore;

            Debug.Log(data.playerName);
            bestScoreText.text = "Best Score : " + HighScorePlayerName + " : " + HighScore.ToString();
        }
    }










}
