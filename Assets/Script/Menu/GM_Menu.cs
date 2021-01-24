using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GM_Menu : MonoBehaviour
{
    [SerializeField]
    Text PlayerNameInput;

    private void Awake()
    {
        string playerName = PlayerPrefs.GetString("PlayerName");

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Enter PlayerName...";
        }

        string HightScorePath = Application.streamingAssetsPath + "/Data/HightScoreSave.txt";
        if (!File.Exists(HightScorePath))
        {
            string content = "0";
            File.WriteAllText(HightScorePath, content);
        }
        string HightScorePlayerNamePath = Application.streamingAssetsPath + "/Data/HightScorePlayerName.txt";
        if (!File.Exists(HightScorePlayerNamePath))
        {
            string content = "-";
            File.WriteAllText(HightScorePlayerNamePath, content);
        }

        InputField playerNameField = PlayerNameInput.GetComponent<InputField>();
        playerNameField.text = playerName;

        playerNameField.onEndEdit.AddListener(SavePlayerName);
    }

    void SavePlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
