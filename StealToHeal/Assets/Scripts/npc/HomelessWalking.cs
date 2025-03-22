using UnityEngine;
using System.Collections;

public class HomelessWalking : MonoBehaviour
{
    public float speed = 5f;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private bool movingToEnd = true;
    public bool interacting = false;
    
    // Other
    public Animator animator;
    private Rigidbody rbPlayer;
    public PlayerMovement player;
    
    // GameObjects
    public GameObject text;
    public GameObject box;
    public GameObject person;
    public GameObject portal;
    public GameObject indicator;

    void Start()
    {
        transform.position = startPoint;
    }

    void Update()
    {
        if (!interacting)
        {
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
                if (transform.position == endPoint)
                {
                    movingToEnd = false;
                    transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPoint, speed * Time.deltaTime);
                if (transform.position == startPoint)
                {
                    movingToEnd = true;
                    transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        interacting = true;
        animator.enabled = false;
        indicator.SetActive(false);
        rbPlayer = collision.gameObject.GetComponent<Rigidbody>();
        rbPlayer.constraints = RigidbodyConstraints.FreezeAll;
        player.DisableCameraMovement();
        text.SetActive(true);
        box.SetActive(true);
        StartCoroutine(WaitPeriod());
        rbPlayer.constraints = RigidbodyConstraints.None;
        rbPlayer.constraints = RigidbodyConstraints.FreezeRotation;
        player.EnableCameraMovement();
    }
    
    IEnumerator WaitPeriod()
    {
        yield return new WaitForSeconds(5f);
        text.SetActive(false);
        box.SetActive(false);
        person.SetActive(false);
        portal.SetActive(true);
    }
}