using System.Collections.Generic;
using UnityEngine;

public class ObstacleParentMovement : MonoBehaviour
{
    private Vector2 _startPos;
    [SerializeField] private MoveStyle moveStyle;
    [SerializeField] private bool directionIsLeft;
    [SerializeField] private int speed = 1;
    private List<Projectile> projectiles=new List<Projectile>();

    private void Awake()
    {
        if(moveStyle==MoveStyle.LeftAndRight)
            _startPos = transform.position;
        else if(moveStyle == MoveStyle.SpinAround) 
            projectiles.AddRange(GetComponentsInChildren<Projectile>());
    }
    
    public void Tick()
    {
        switch (moveStyle)
        {
            case MoveStyle.LeftAndRight:
                LeftAndRightMovement();
                break;
            case MoveStyle.SpinAround:
                SpinAroundMovement();
                break;
        }
    }

    private void SpinAroundMovement()
    {
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (directionIsLeft==false?1:-1) * speed * 15*Time.deltaTime);
    }
    
    private void LeftAndRightMovement()
    {
        if (Mathf.Approximately(transform.position.x, _startPos.x+2f)||Mathf.Approximately(transform.position.x, _startPos.x-2f))
        {
            directionIsLeft=!directionIsLeft;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_startPos.x + 2f*(directionIsLeft==false?1:-1), transform.position.y), speed*Time.deltaTime);
    }
}

public enum MoveStyle
{
    LeftAndRight,
    SpinAround
}