using System;
using UnityEngine;

public class ObstacleParentMovement : MonoBehaviour
{
    private Vector2 _startPos;
    [SerializeField] private MoveStyle moveStyle;
    [SerializeField] private int direction = 1;
    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        switch (moveStyle)
        {
            case MoveStyle.LeftAndRight:
                LeftAndRightMovement();
                break;
            case MoveStyle.SpinAround:
                break;
        }
    }

    private void LeftAndRightMovement()
    {
        if (Mathf.Approximately(transform.position.x, _startPos.x+2f)||Mathf.Approximately(transform.position.x, _startPos.x-2f))
        {
            direction *= -1;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_startPos.x + 2f*direction, transform.position.y), Time.deltaTime);
    }
}

public enum MoveStyle
{
    LeftAndRight,
    SpinAround
}