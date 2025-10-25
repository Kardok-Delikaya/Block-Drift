using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private GameObject stageCompleted;
    
    [Header("Upgrade Panel")]
    public GameObject upgradePanel;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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

    public void SetProgressSliderValue(float value)
    {
        progressSlider.value = value;
    }

    public void SetXpSliderValue(float value)
    {
        xpSlider.value = value;
    }

    public void CloseGameStartText()
    {
        gameStartText.SetActive(false);
    }

    public void StageCompleted()
    {
        stageCompleted.SetActive(true);
    }

    public void StageFailed()
    {
        
    }
}
