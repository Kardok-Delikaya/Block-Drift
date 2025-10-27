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
    [SerializeField] private List<Projectile> activeProjectilePool;
    [SerializeField] private List<Projectile> deactiveProjectilePool;
    
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
        if(activeProjectilePool.Count>0)
            foreach (var projectile in activeProjectilePool)
                projectile.Tick();
    }

    private void Shoot()
    {
        if (projectilePrefab == null || shootPoint == null) return;

        if (deactiveProjectilePool.Count > 0)
        {
            deactiveProjectilePool[0].gameObject.SetActive(true);
            deactiveProjectilePool[0].transform.position = shootPoint.position;
            deactiveProjectilePool[0].GetComponent<Projectile>().GetStats(damage,durability,speed,this);
            activeProjectilePool.Add(deactiveProjectilePool[0]);
            deactiveProjectilePool.RemoveAt(0);
            
        }
        else
        {
            GameObject projectile= Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().GetStats(damage,durability,speed,this);
            activeProjectilePool.Add(projectile.GetComponent<Projectile>());
        }
    }

    public void AddProjectileToDeactivatedPool(Projectile projectile)
    {
        activeProjectilePool.Remove(projectile);
        deactiveProjectilePool.Add(projectile);
    }
}