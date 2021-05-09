﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public int currentLevel = 1;
    int MaxLevelNumber = 10;
    public bool isGameStarted, isGameOver;

    public ShipController m_ShipController;
    public ClawController m_ClawController;
    public CollisionDetection m_CollisionDetection;

    #region UI Elements
    public GameObject WinPanel, LosePanel, InGamePanel;
    public Button VibrationButton, TapToStartButton;
    public Sprite on, off;
    public Text LevelText;
    public GameObject PlayText, ContinueText;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        if (!PlayerPrefs.HasKey("VIBRATION"))
        {
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButton.GetComponent<Image>().sprite = on;
        }
        else
        {
            if (PlayerPrefs.GetInt("VIBRATION") == 1)
            {
                VibrationButton.GetComponent<Image>().sprite = on;
            }
            else
            {
                VibrationButton.GetComponent<Image>().sprite = off;
            }
        }

        if (PlayerPrefs.GetInt("FromMenu") == 1)
        {
            ContinueText.SetActive(false);
            PlayerPrefs.SetInt("FromMenu", 0);
        }
        else
        {
            PlayText.SetActive(false);
        }
        currentLevel = PlayerPrefs.GetInt("LevelId");
        LevelText.text = currentLevel.ToString();
    }



    public IEnumerator WaitAndGameWin()
    {
        isGameOver = true;
        //SoundManager.Instance.StopAllSounds();
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Win);

        yield return new WaitForSeconds(1f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);

        currentLevel++;
        PlayerPrefs.SetInt("LevelId", currentLevel);
        WinPanel.SetActive(true);
    }

    public IEnumerator WaitAndGameLose()
    {
        isGameOver = true;
        //SoundManager.Instance.playSound(SoundManager.GameSounds.Lose);

        yield return new WaitForSeconds(1f);

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Medium);

        LosePanel.SetActive(true);
    }

    public void TapToNextButtonClick()
    {
        if (currentLevel > MaxLevelNumber)
        {
            int rand = Random.Range(1, MaxLevelNumber);
            if (rand == PlayerPrefs.GetInt("LastRandomLevel"))
            {
                rand = Random.Range(1, MaxLevelNumber);
            }
            else
            {
                PlayerPrefs.SetInt("LastRandomLevel", rand);
            }
            SceneManager.LoadScene("Level" + rand);
        }
        else
        {
            SceneManager.LoadScene("Level" + currentLevel);
        }
    }

    public void TapToTryAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapToStartButtonClick()
    {

    }

    public void VibrateButtonClick()
    {
        if (PlayerPrefs.GetInt("VIBRATION").Equals(1))
        {//Vibration is on
            PlayerPrefs.SetInt("VIBRATION", 0);
            VibrationButton.GetComponent<Image>().sprite = off;
        }
        else
        {//Vibration is off
            PlayerPrefs.SetInt("VIBRATION", 1);
            VibrationButton.GetComponent<Image>().sprite = on;
        }

        //if (PlayerPrefs.GetInt("VIBRATION") == 1)
        //    TapticManager.Impact(ImpactFeedback.Light);
    }
}