using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8.0f;
    [SerializeField] private float gravity = 16f;

    [Header("Inputs Customization")]
    [SerializeField] private string horizontalMoveInput = "Horizontal";
    [SerializeField] private string verticalMoveInput = "Vertical";
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Footstep Sounds")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.3f;
    [SerializeField] private float velocityThreshold = 2.0f;

    private bool isMoving;
    private float nextStepTime;
    private CharacterController characterController;
    private Vector3 currentMovement;
    private float verticalSpeed;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileForce = 30.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleFootsteps();
    }

    void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.forward * projectileForce, ForceMode.Impulse);
    }

    void HandleMovement()
    {
        float verticalInput = Input.GetAxis(verticalMoveInput);
        float horizontalInput = Input.GetAxis(horizontalMoveInput);
        float speedMultiplier = Input.GetKey(sprintKey) ? sprintMultiplier : 1f;

        verticalSpeed = verticalInput * walkSpeed * speedMultiplier;
        float horizontalSpeed = horizontalInput * walkSpeed * speedMultiplier;

        Vector3 horizontalMovement = new Vector3(horizontalSpeed, 0, verticalSpeed);
        horizontalMovement = transform.rotation * horizontalMovement;

        HandleJump();

        currentMovement.x = horizontalMovement.x;
        currentMovement.z = horizontalMovement.z;

        characterController.Move(currentMovement * Time.deltaTime);

        isMoving = verticalInput != 0 || horizontalInput != 0;
    }

    void HandleJump()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    void HandleFootsteps()
    {
        float currentStepInterval = Input.GetKey(sprintKey) ? sprintStepInterval : walkStepInterval;

        if (characterController.isGrounded && isMoving && Time.time > nextStepTime &&
            characterController.velocity.magnitude > velocityThreshold)
        {
            PlayFootstepSounds();
            nextStepTime = Time.time + currentStepInterval;
        }
    }

    void PlayFootstepSounds()
    {
        int randomIndex = Random.Range(0, footstepSounds.Length);
        footstepSource.clip = footstepSounds[randomIndex];
        footstepSource.Play();
    }
}