using UnityEngine;

public class SpinAndFloatChild : MonoBehaviour
{
    public float rotationSpeed = 100f;  // Rotation speed
    public float floatSpeed = 5f;      // Speed of up/down movement
    public float floatHeight = 0.0001f;   // Max height variation

    private Vector3 startLocalPosition; // Store initial local position

    void Start()
    {
        startLocalPosition = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        float newY = startLocalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = new Vector3(startLocalPosition.x, newY, startLocalPosition.z);
    }
}