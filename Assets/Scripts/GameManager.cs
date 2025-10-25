using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
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
    public float finishY = 30;

    private void Awake()
    {
        if(Instance==null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (isLevelEnded) return;
        
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
        
        UIManager.Instance.SetProgressSliderValue(player.transform.position.y / finishY);
        
        if (player.transform.position.y >= finishY)
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
        UIManager.Instance.StageCompleted();
    }

    public void StageFailed()
    {
        isLevelEnded= true;
        UIManager.Instance.StageFailed();
    }
}