using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public int damage=1;
    public int durability=1;
    public float shootRate=2.5f;
    [SerializeField] private float speed=10;
    private float _shootTimer=0.4f;

    [Header("Object Pooling")]
    [SerializeField] private List<GameObject> deactivatedProjectilePool;
    
    public void Tick()
    {
        _shootTimer-= Time.deltaTime;
        
        if (_shootTimer <= 0f)
        {
            Shoot();
            _shootTimer = 1/shootRate;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        if (deactivatedProjectilePool.Count > 0)
        {
            deactivatedProjectilePool[0].SetActive(true);
            deactivatedProjectilePool[0].transform.position = shootPoint.position;
            deactivatedProjectilePool[0].GetComponent<Projectile>().GetStats(damage,durability,speed,this);
            deactivatedProjectilePool.RemoveAt(0);
            
        }
        else
        {
            GameObject projectile= Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().GetStats(damage,durability,speed,this);
        }
    }

    public void AddProjectileToDeactivatedPool(GameObject projectile)
    {
        deactivatedProjectilePool.Add(projectile);
    }
}