using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouching")]
    public float crouchHeight = 1f;     // Adjust based on how low the crouch is
    private float originalHeight;
    public float crouchSpeedMultiplier = 0.5f; // Movement speed when crouching
    private Vector3 originalCenter;

    [Header("References")]
    public CharacterController characterController;
    public Transform playerCamera;

    private float originalCameraHeight;
    private bool isCrouched = false;

    void Start()
    {
        // Store original values
        originalHeight = characterController.height;
        originalCenter = characterController.center;
        originalCameraHeight = playerCamera.localPosition.y;
    }

    void Update()
    {
        HandleCrouch();
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            StandUp();
        }
    }

    void Crouch()
    {
        if (isCrouched) return;

        characterController.height = crouchHeight;
        characterController.center = new Vector3(0, crouchHeight / 2f, 0);

        // Lower camera smoothly
        playerCamera.localPosition = new Vector3(
            playerCamera.localPosition.x,
            originalCameraHeight - (originalHeight - crouchHeight) / 2f,
            playerCamera.localPosition.z
        );

        isCrouched = true;

        // Adjust your movement speed here if necessary
        // e.g., movementSpeed *= crouchSpeedMultiplier;
    }

    void StandUp()
    {
        if (!isCrouched) return;

        characterController.height = originalHeight;
        characterController.center = originalCenter;

        // Reset camera position
        playerCamera.localPosition = new Vector3(
            playerCamera.localPosition.x,
            originalCameraHeight,
            playerCamera.localPosition.z
        );

        isCrouched = false;

        // Reset movement speed if modified
        // e.g., movementSpeed /= crouchSpeedMultiplier;
    }
}