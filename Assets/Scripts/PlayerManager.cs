using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float upwardSpeed = 2f;
    [SerializeField] private float horizontalSpeed = 4f;
    [SerializeField] private float xLimit = 3f;

    [Header("Weapons")]
    [SerializeField] private List<Weapon> weapons=new List<Weapon>(); 

    [Header("Health")]
    public int maxHealth = 3;
    private int _currentHealth;
    private bool _isTouching = false;

    private void Start()
    {
        _currentHealth = maxHealth;
        UIManager.Instance.HandleHealthIconStart(maxHealth);
    }

    public void Tick()
    {
        transform.Translate(Vector3.up * upwardSpeed * Time.deltaTime);
        HandleMovementInput();
        HandleWeaponsAttack();
    }

    private void HandleWeaponsAttack()
    {
        foreach (var w in weapons)
        {
            if (_isTouching)
            {
                w.Tick();
            }
        }
    }
    
    private void HandleMovementInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                _isTouching = true;
            
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                _isTouching = false;

            if (touch.phase == TouchPhase.Moved)
            {
                float moveX = (touch.deltaPosition.x / Screen.width) * horizontalSpeed * 50f;
                Vector3 newPos = transform.position + Vector3.right * moveX * Time.deltaTime;
                newPos.x = Mathf.Clamp(newPos.x, -xLimit, xLimit);
                transform.position = newPos;
            }
        }

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0)) _isTouching = true;
            if (Input.GetMouseButtonUp(0)) _isTouching = false;

            if (Input.GetMouseButton(0))
            {
                float moveX = Input.GetAxis("Mouse X") * horizontalSpeed * Time.deltaTime * 50f;
                Vector3 newPos = transform.position + Vector3.right * moveX;
                newPos.x = Mathf.Clamp(newPos.x, -xLimit, xLimit);
                transform.position = newPos;
            }
        }
    }
    
    public void TakeDamage()
    {
        _currentHealth--;
        
        if (_currentHealth <= 0)
        {
            GameManager.Instance.StageFailed();
        }
        
        UIManager.Instance.UpdateHealthIconsColor(_currentHealth);
    }

    public void UpgradeWeapons(int upgradeID)
    {
        switch (upgradeID)
        {
            case 0:
                weapons[0].damage += 1;
                break;
            case 1:
                weapons[0].durability += 1;
                break;
            case 2:
                weapons[0].shootRate *= 1.15f;
                break;
        }

        UIManager.Instance.upgradePanel.SetActive(false);
        Time.timeScale = 1;
    }
}