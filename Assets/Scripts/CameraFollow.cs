using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;        
    public float followSpeed = 5f;   
    public float yOffset = 0f;       

    private float _startYDistance; 

    private void Start()
    {
        if (target != null)
            _startYDistance = transform.position.y - target.position.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        float targetY = target.position.y + _startYDistance + yOffset;

        Vector3 newPos = transform.position;
        newPos.y = Mathf.Lerp(transform.position.y, targetY, followSpeed * Time.deltaTime);
        transform.position = newPos;
    }
}