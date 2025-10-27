using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [Header("Sliders")]
    public Slider progressSlider;
    public Slider xpSlider;
    
    [Header("Health")]
    [SerializeField] private Transform healthIconsParent;
    [SerializeField] private GameObject healthIconGameObject;
    private List<Image> _healthIcons=new List<Image>();

    [Header("Texts")]
    [SerializeField] private GameObject gameStartText;
    [SerializeField] private GameObject stageCompletedTextGameObject;
    [SerializeField] private GameObject stageFailedTextGameObject;
    [SerializeField] private TMP_Text levelText;
    
    [Header("Upgrade Panel")]
    public GameObject upgradePanel;
    
    [Header("Pause Panel")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text countDownText;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        levelText.text="Level: "+(PlayerPrefs.GetInt("LevelToStart")+1);
    }

    public void HandleHealthIconStart(int maxHealth)
    {
        if (maxHealth < 1) maxHealth = 1;
        
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject newHealthIcon = Instantiate(healthIconGameObject, healthIconsParent);
            _healthIcons.Add(newHealthIcon.GetComponent<Image>());
        }
        
        UpdateHealthIconsColor(maxHealth);
    }

    public void UpdateHealthIconsColor(int health)
    {
        for (int i = 0; i < _healthIcons.Count; i++)
        {
            if (health <= i)
            {
                _healthIcons[i].color = Color.red;
            }
            else
            {
                _healthIcons[i].color = Color.green;
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
    }
    
    public IEnumerator ResumeEnumerator()
    {
        pausePanel.SetActive(false);
        countDownText.gameObject.SetActive(true);
        countDownText.text = "Game will resume in 3 seconds...";
        yield return new WaitForSeconds(1);
        countDownText.text = "Game will resume in 2 seconds...";
        yield return new WaitForSeconds(1);
        countDownText.text = "Game will resume in 1 seconds...";
        yield return new WaitForSeconds(1);
        countDownText.gameObject.SetActive(false);
        GameManager.instance.ActivateGameLoop();
    }
    
    public void SetProgressSliderValue(float value)
    {
        progressSlider.value = value;
    }

    public void SetXpSliderValue(float value)
    {
        xpSlider.value = value;
    }

    public void StageCompleted()
    {
        stageCompletedTextGameObject.SetActive(true);
    }

    public void StageFailed()
    {
        stageFailedTextGameObject.SetActive(true);
    }
}
