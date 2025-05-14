using UnityEngine;

public class GunFlashlight : MonoBehaviour
{
    public Light flashlight; // Assign in Inspector
    public KeyCode toggleKey = KeyCode.T;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (flashlight != null)
                flashlight.enabled = !flashlight.enabled;
        }
    }
}
