using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isLocked = true;     // Starts locked
    public float openAngle = 90f;    // How far it swings
    public float openSpeed = 2f;     // Swing speed

    private bool isOpen = false;
    private Quaternion closedRot, openRot;

    void Start()
    {
        closedRot = transform.localRotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        Quaternion target = isOpen ? openRot : closedRot;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * openSpeed);
    }

    public void UnlockAndOpen()
    {
        isLocked = false;
        isOpen = true;
    }

    // Optionally, detect player “use”:
    void OnMouseDown()
    {
        if (!isLocked) isOpen = !isOpen;
        // else you’d trigger the keypad UI instead
    }
}
