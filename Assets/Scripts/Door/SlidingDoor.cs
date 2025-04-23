using UnityEngine;

public class SlidingDoorController : MonoBehaviour
{
    [Header("Lock & Slide Settings")]
    public bool isLocked = true;        // Starts locked
    public float slideDistance = 2f;          // How far (in local units) to slide open
    public float slideSpeed = 2f;          // How fast to slide

    [Tooltip("Direction in local space to slide when opening")]
    public Vector3 openDirection = Vector3.left;

    // internal state
    private bool isOpen;
    private Vector3 closedPos;
    private Vector3 openPos;

    void Start()
    {
        // Record where “closed” is, then compute the target “open” position
        closedPos = transform.localPosition;
        openPos = closedPos + openDirection.normalized * slideDistance;
    }

    void Update()
    {
        // Smoothly move toward either closedPos or openPos
        Vector3 target = isOpen ? openPos : closedPos;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target,
            slideSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Call this (e.g. from your KeypadLock.OnEnter) to unlock and slide the door open.
    /// </summary>
    public void UnlockAndOpen()
    {
        isLocked = false;
        isOpen = true;
    }

    /// <summary>
    /// Optional: click the door to toggle it after unlocked.
    /// </summary>
    void OnMouseDown()
    {
        if (!isLocked)
            isOpen = !isOpen;
    }
}
