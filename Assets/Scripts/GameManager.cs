using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private bool _gameLoopActive;

    [Header("Experience")] public int currentPlayerLevel = 1;
    public float currentExperience;
    private readonly float[] _maxExperience = { 5, 15, 25, 50 };

    [Header("GameObjects")] 
    [SerializeField] private PlayerManager player;
    [SerializeField] private List<ObstacleParentMovement> obstacleParents;

    [Header("Finish Distance")] 
    public LevelData[] levelDatas;
    private float _currentLevelLength;
    private bool _stageCompleted;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
        if (!_gameLoopActive) return;

        player.Tick();
        
        foreach (var obstacleParent in obstacleParents)
            obstacleParent.Tick();
        
        UIManager.Instance.SetProgressSliderValue(player.transform.position.y / _currentLevelLength);

        if (player.transform.position.y >= _currentLevelLength)
        {
            StageCompleted();
        }
    }

    public void BlockKilled(int xp)
    {
        currentExperience += xp;

        if (currentExperience >= _maxExperience[currentPlayerLevel - 1])
        {
            currentExperience -= _maxExperience[currentPlayerLevel - 1];
            currentPlayerLevel++;
            Time.timeScale = 0;
            UIManager.Instance.upgradePanel.SetActive(true);
        }

        UIManager.Instance.SetXpSliderValue(currentExperience / _maxExperience[currentPlayerLevel - 1]);
    }

    private void StageCompleted()
    {
        _gameLoopActive = false;
        _stageCompleted = true;
        UIManager.Instance.StageCompleted();
    }

    public void StageFailed()
    {
        _gameLoopActive = false;
        UIManager.Instance.StageFailed();
    }

    private void HandleLevelStart(int levelToStart)
    {
        if (levelToStart >= levelDatas.Length)
        {
            levelToStart = 0;
            PlayerPrefs.SetInt("LevelToStart", 0);
        }

        foreach (LevelData levelData in levelDatas)
        {
            levelData.levelPrefab.SetActive(false);
        }

        levelDatas[levelToStart].levelPrefab.SetActive(true);
        _currentLevelLength = levelDatas[levelToStart].levelLength;
        
        if(levelDatas[levelToStart].levelPrefab.GetComponentInChildren<ObstacleParentMovement>()!=null) 
            obstacleParents.AddRange(levelDatas[levelToStart].levelPrefab.GetComponentsInChildren<ObstacleParentMovement>());
    }

    public void PauseGame()
    {
        _gameLoopActive = false;
        UIManager.Instance.PauseGame();
    }

    public void ResumeGame()
    {
        StartCoroutine(UIManager.Instance.ResumeEnumerator());
    }

    public void ActivateGameLoop()
    {
        _gameLoopActive = true;
    }

    public void StartGameLoop()
    {
        _gameLoopActive = true;
    }

    public void StartNextLevel()
    {
        if (_stageCompleted)
        {
            PlayerPrefs.SetInt("LevelToStart", PlayerPrefs.GetInt("LevelToStart") + 1);
        }
        
        SceneManager.LoadScene(0);
    }
}

[Serializable]
public class LevelData{
    public int levelLength;
    public GameObject levelPrefab;
}