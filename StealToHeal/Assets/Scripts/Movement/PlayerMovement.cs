using System;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public int money = 0;
    public TMP_Text moneyText;
    
    //Assingables
    public Transform playerCam;
    public Transform orientation;
    
    //Other
    private Rigidbody rb;
    public bool cutscene = false;

    //Rotation and look
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    
    //Movement
    public float moveSpeed = 4500f;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
    
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 150f;
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching;
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;
    
    private GameObject targetedObject;
    
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void FixedUpdate() {
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
        RaycastFromCamera();
            
        moneyText.text = money.ToString() + "$";
        
        if (Input.GetKeyDown(KeyCode.E) && targetedObject != null) {
            Destroy(targetedObject);
            if (targetedObject.name == "Gold Ingot")
            {
                money += 3;
            } else if (targetedObject.name == "dollar")
            {
                money += 1;
            } else if (targetedObject.name == "Gem")
            {
                money += 5;
            }
        }
    }
    
    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);
        
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }
    
    private void RaycastFromCamera() {
        Ray ray = new Ray(playerCam.position, playerCam.forward);
        RaycastHit hit;
        
        float raycastDistance = 100f;
        
        if (Physics.Raycast(ray, out hit, raycastDistance)) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Targetable")) {
                targetedObject = hit.collider.gameObject;
            } else {
                targetedObject = null;
            }
        } else {
            targetedObject = null;
        }
    }
    
    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.linearVelocity.magnitude > 0.5f) {
            if (grounded) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }
    
    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }
    
    private void Movement() {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;
        
        CounterMovement(x, y, mag);
        
        if (readyToJump && jumping) Jump();
        
        float maxSpeed = this.maxSpeed;
        
        if (crouching && grounded && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;
        
        float multiplier = 1f, multiplierV = 1f;
        
        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        if (grounded && crouching) multiplierV = 0f;
        
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;
            
            rb.AddForce(Vector2.up * jumpForce * 1.2f);
            rb.AddForce(normalVector * jumpForce * 0.3f);
            
            Vector3 vel = rb.linearVelocity;
            if (rb.linearVelocity.y < 0.5f)
                rb.linearVelocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.linearVelocity.y > 0) 
                rb.linearVelocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
    
    private float desiredX;
    private void Look() {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
            
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;
            
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);   
    }
    
    public void DisableCameraMovement()
    {
        sensitivity = 0f;
    }

    public void EnableCameraMovement()
    {
        sensitivity = 50f;
    }
    
    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;
        
        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.linearVelocity.normalized * slideCounterMovement);
            return;
        }
        
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        if (Mathf.Sqrt((Mathf.Pow(rb.linearVelocity.x, 2) + Mathf.Pow(rb.linearVelocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.linearVelocity.y;
            Vector3 n = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(n.x, fallspeed, n.z);
        }
    }
    
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.linearVelocity.x, rb.linearVelocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.linearVelocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    private void OnCollisionStay(Collision other) {
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;
        
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }
        
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }
}