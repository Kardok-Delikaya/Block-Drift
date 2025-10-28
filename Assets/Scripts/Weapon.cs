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
    private float _shootTimer;

    [Header("Object Pooling")]
    [SerializeField] private List<Projectile> projectilePool;
    [SerializeField] private List<Projectile> inactiveProjectilePool;
    
    public void AttackTick()
    {
        _shootTimer-= Time.deltaTime;
        
        if (_shootTimer <= 0f)
        {
            Shoot();
            _shootTimer = 1/shootRate;
        }
    }

    public void ProjectileTick()
    {
        foreach (Projectile projectile in projectilePool)
        {
            projectile.Tick();
        }
    }
    
    private void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        if (inactiveProjectilePool.Count > 0)
        {
            inactiveProjectilePool[0].gameObject.SetActive(true);
            inactiveProjectilePool[0].transform.position = shootPoint.position;
            inactiveProjectilePool[0].GetComponent<Projectile>().GetStats(damage,durability,speed,this);
            inactiveProjectilePool.RemoveAt(0);
            
        }
        else
        {
            GameObject projectile= Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().GetStats(damage,durability,speed,this);
            projectilePool.Add(projectile.GetComponent<Projectile>());
        }
    }

    public void AddProjectileToDeactivatedPool(Projectile projectile)
    {
        inactiveProjectilePool.Add(projectile);
    }
}