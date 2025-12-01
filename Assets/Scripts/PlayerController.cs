using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MinecraftPlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 1.5f;

    [Header("Look")]
    public float mouseSensitivity = 200f;
    public Transform cameraHolder;  // your Camera

    [Header("Gravity")]
    public float gravity = -9.81f;

    [Header("Shooting")]
    public Transform shootPoint;       // where bullets spawn
    public GameObject bulletPrefab;    // bullet prefab
    public float bulletSpeed = 30f;

    [Header("Gun Sound")]
    public AudioSource audioSource;
    public AudioClip gunShotClip;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        HandleShoot();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void HandleMovement()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * h + transform.forward * v).normalized;
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleShoot()
    {
        // Left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || cameraHolder == null)
        {
            Debug.LogWarning("Shoot called but bulletPrefab or cameraHolder is not assigned.");
            return;
        }

        // Play gun sound
        if (audioSource != null && gunShotClip != null)
        {
            audioSource.PlayOneShot(gunShotClip);
        }

        // Spawn bullet from camera
        Vector3 origin = cameraHolder.position + cameraHolder.forward * 0.2f;
        Vector3 dir = cameraHolder.forward;

        GameObject bullet = Instantiate(
            bulletPrefab,
            origin,
            Quaternion.LookRotation(dir, Vector3.up)
        );

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * bulletSpeed;
        }
    }



}
