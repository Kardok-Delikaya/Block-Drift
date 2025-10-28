using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Weapon _weapon;
    private readonly float _lifeTime = 3f;
    private int _damage = 1;
    private int _durability = 1;
    private float _speed = 10f;

    public void Tick()
    {
        if (!gameObject.activeSelf) return;
        
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (gameObject.transform.position.y-_weapon.transform.position.y>8)
        {
            _weapon.AddProjectileToDeactivatedPool(this);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (_durability > 0)
            {
                _durability--;
                other.GetComponent<Obstacle>().TakeHit(_damage);
            }

            if (_durability <= 0)
            {
                _weapon.AddProjectileToDeactivatedPool(this);
                gameObject.SetActive(false);
            }
        }
    }

    public void GetStats(int damage, int durability, float speed, Weapon weapon)
    {
        _damage = damage;
        _durability=durability;
        _speed = speed;
        _weapon = weapon;
    }
}