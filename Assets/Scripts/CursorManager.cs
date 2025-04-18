using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Update()
    {
        if (IntelUI.IsUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
