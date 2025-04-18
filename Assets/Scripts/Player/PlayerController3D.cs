using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = 20f;

    [Header("Look")]
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;
    public float minY = -80f;
    public float maxY = 80f;

    [Header("Cursor Control")]
    public bool lockCursorOnStart = true;

    private CharacterController controller;
    private Vector3 velocity;
    private float cameraPitch = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursorOnStart)
        {
            LockCursor(true);
        }
    }

    void Update()
    {
        if (IntelUI.IsUIOpen)
        {
            LockCursor(false);
            return;
        }

        HandleMovement();
        HandleLook();
        LockCursor(true); // Always lock when not in UI
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpForce;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minY, maxY);

        cameraHolder.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void LockCursor(bool shouldLock)
    {
        Cursor.lockState = shouldLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldLock;
    }
}
