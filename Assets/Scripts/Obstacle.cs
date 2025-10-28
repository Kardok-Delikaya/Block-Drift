using UnityEngine;
using TMPro;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int health = 2;
    [SerializeField] private TMP_Text healthText;
    private int _xpReward=2;
    private void Start()
    {
        healthText.text = health.ToString();
        _xpReward = health;
    }

    public void TakeHit(int damage = 1)
    {
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.BlockKilled(_xpReward);
        }

        healthText.text = health.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerManager>().TakeDamage();
            Destroy(gameObject);
        }
    }
}