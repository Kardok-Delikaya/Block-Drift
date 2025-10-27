using Unity.VisualScripting;
using UnityEngine;

public class TourchToStartNextLevel : MonoBehaviour
{
    [SerializeField] private StartType startType;
    
    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            switch(startType)
            {
                case StartType.NextLevel:
                    GameManager.instance.StartNextLevel();
                    break;
                case StartType.CurrentLevel:
                    GameManager.instance.StartGameLoop();
                    break;
            }
            
            gameObject.SetActive(false);
        }
    }
}

enum StartType
{
    NextLevel,
    CurrentLevel
}