using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class PlayerName : MonoBehaviour
{
    public Text text_Game;
    public Text Score_Game;

    GM_DemoScene GM_Demo;
    private void Start()
    {
        GM_Demo = GameObject.Find("DEMO_GM").GetComponent<GM_DemoScene>();

        string playerName = PlayerPrefs.GetString("PlayerName");
        playerName = PlayerPrefs.GetString("PlayerName",playerName);

        text_Game.text = "Player: " + playerName;
    }

    private void Update()
    {
        Score_Game.text = "Score: " + GM_Demo.Score.ToString();
    }
}
