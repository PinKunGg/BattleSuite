using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.UI;

public class GM_DemoScene : MonoBehaviour
{
    #region ตัวแปร
    public GameObject GroundSpawn;
    public  GameObject FlyingSpawn;
    public GameObject GroundEnemy;
    public GameObject FlyingEnemy;
    public GameObject BossFlying;
    public GameObject BossGround;
    public GameObject GameOver;

    float GMax;
    float FMax;
    public float BGMax = 1f;
    public float BFMax = 1f;
    public float Score;
    public float LastHightScoreShow;
    public float WaveCount;

    public static List<GroundEnemy> GroundEnemyList = new List<GroundEnemy>();
    public static List<FlyingEnemyScript> FlyingEnemyList = new List<FlyingEnemyScript>();
    public static List<RoboBoss> BossG = new List<RoboBoss>();
    public static List<RoboBossFlight> BossF = new List<RoboBossFlight>();

    public Text ScoreInRound;
    public Text SaveLastHightScore;
    public Text PlayerGameOver;

    bool esc = false;
    public GameObject pauseCanvas;
    #endregion

    private void Start()
    {
        GMax = 3f;
        FMax = 3f;
        WaveCount = 0f;
        GroundEnemyList.Clear();
        FlyingEnemyList.Clear();
        GameOver.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        pauseCanvas.SetActive(false);

        EventManager.AddBossSpawnListener(EventSpawnBoss);
    }
    private void Update()
    {
        Esc();

        if (GroundEnemyList.Count == 0 && FlyingEnemyList.Count == 0)
        {
            CheckGroundEnemyAmount();
            CheckFlyingEnemyAmount();
            GMax++;
            FMax++;
            WaveCount++;
        }
    }

    public void EventSpawnBoss()
    {
        if (BossG.Count == 0 && BossF.Count == 0)
        {
            SpawnBoss();
            BGMax++;
            BFMax++;
        }
    }

    void SpawnBoss()
    {
        for (int i = 0; i < BGMax; i++)
        {
            Instantiate(BossGround, GroundSpawn.transform.position, Quaternion.identity);
        }
        for (int i = 0; i < BFMax; i++)
        {
            Instantiate(BossFlying, FlyingSpawn.transform.position, Quaternion.identity);
        }
    }
    void CheckGroundEnemyAmount()
    {
        for (int i = 0; i < GMax; i++)
        {
            Instantiate(GroundEnemy, GroundSpawn.transform.position, Quaternion.identity);
        }
    }
    void CheckFlyingEnemyAmount()
    {
        for (int i = 0; i < FMax; i++)
        {
            Instantiate(FlyingEnemy, FlyingSpawn.transform.position, Quaternion.identity);
        }
    }
    public void GameOverMethod()
    {
        GameOver.SetActive(true);
        SaveHightestScore();
    }
    void Esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            esc = !esc;

            if (esc == false)
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                pauseCanvas.SetActive(false);
            }
            else if (esc == true)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                pauseCanvas.SetActive(true);
            }
        }
    }

    #region IO Section

    public void SaveHightestScore()
    {
        //SaveShow_Score
        string playerName = PlayerPrefs.GetString("PlayerName");
        playerName = PlayerPrefs.GetString("PlayerName", playerName);

        try
        {
            PlayerGameOver.text = "Player: " + playerName;
            ScoreInRound.text = "Player: " + playerName + " | " + Score.ToString();

            string HightScorePath = Application.streamingAssetsPath + "/Data/HightScoreSave.txt";
            string HightScorePlayerNamePath = Application.streamingAssetsPath + "/Data/HightScorePlayerName.txt";

            string LastSaveHightScore = File.ReadAllText(HightScorePath);
            string savePlayerHightScore = File.ReadAllText(HightScorePlayerNamePath);

            LastHightScoreShow = float.Parse(LastSaveHightScore);

            SaveLastHightScore.text = "Player: " + savePlayerHightScore + " | " + LastSaveHightScore.ToString();

            if (Score > LastHightScoreShow)
            {
                File.WriteAllText(HightScorePath, Score.ToString());
                File.WriteAllText(HightScorePlayerNamePath, playerName);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion

    #region Button
    public void MenuButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    #endregion
}
