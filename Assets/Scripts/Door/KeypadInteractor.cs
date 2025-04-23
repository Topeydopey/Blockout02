using UnityEngine;

public class KeypadInteractor : MonoBehaviour
{
    [Tooltip("How far you can reach to open a door/keypad")]
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            {
                // look for a KeypadLock on whatever we hit
                KeypadLock kl = hit.collider.GetComponent<KeypadLock>();
                if (kl != null)
                {
                    kl.ShowKeypad();
                }
            }
        }
    }
}
