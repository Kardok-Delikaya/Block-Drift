using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool _isLevelStarted = false;
    private bool isLevelEnded = false;
    
    [Header("Experience")]
    public int currentPlayerLevel = 1;
    public float currentExperience;
    private readonly float[] _maxExperience = { 5, 15, 25, 50 };
    
    [Header("GameObjects")]
    public PlayerManager player;

    [Header("Finish Distance")] 
    public LevelData[] levelDatas;
    private float _currentLevelLength = 30;
    private bool _stageCompleted;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        HandleLevelStart(PlayerPrefs.GetInt("LevelToStart"));
    }

    private void Update()
    {
        if (isLevelEnded)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                if (_stageCompleted)
                {
                    PlayerPrefs.SetInt("LevelToStart", PlayerPrefs.GetInt("LevelToStart") + 1);
                    SceneManager.LoadScene(0);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
            }
            return;
        }
        
        if (!_isLevelStarted)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                player.Tick();
                UIManager.Instance.CloseGameStartText();
                _isLevelStarted = true;
            }
            return;
        }

        player.Tick();
        
        UIManager.Instance.SetProgressSliderValue(player.transform.position.y / _currentLevelLength);
        
        if (player.transform.position.y >= _currentLevelLength)
        {
            StageCompleted();
        }
    }

    public void BlockKilled(int xp)
    {
        currentExperience += xp;

        if (currentExperience>=_maxExperience[currentPlayerLevel-1])
        {
            currentExperience-=_maxExperience[currentPlayerLevel-1];
            currentPlayerLevel++;
            Time.timeScale = 0;
            UIManager.Instance.upgradePanel.SetActive(true);
        }
        
        UIManager.Instance.SetXpSliderValue(currentExperience / _maxExperience[currentPlayerLevel-1]);
    }
    
    public void StageCompleted()
    {
        isLevelEnded= true;
        _stageCompleted = true;
        UIManager.Instance.StageCompleted();
    }

    public void StageFailed()
    {
        isLevelEnded= true;
        UIManager.Instance.StageFailed();
    }

    private void HandleLevelStart(int levelToStart)
    {
        if (levelToStart >= levelDatas.Length)
        {
            levelToStart = 0;
            PlayerPrefs.SetInt("LevelStart", 0);
        }
        
        foreach (LevelData levelData in levelDatas)
        {
            levelData.levelPrefab.SetActive(false);
        }
        
        levelDatas[levelToStart].levelPrefab.SetActive(true);
        _currentLevelLength = levelDatas[levelToStart].levelLength;
    }
}

[System.Serializable]
public class LevelData{
    public int levelLength;
    public GameObject levelPrefab;
}