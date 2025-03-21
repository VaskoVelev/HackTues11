using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private bool movingToEnd = true;

    void Start()
    {
        transform.position = startPoint;
    }

    void Update()
    {
        if (movingToEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.fixedDeltaTime);
            if (transform.position == endPoint)
            {
                movingToEnd = false;
                transform.rotation = Quaternion.Euler(0f, 270f, 0f);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint, speed * Time.fixedDeltaTime);
            if (transform.position == startPoint)
            {
                movingToEnd = true;
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
        }
    }
}