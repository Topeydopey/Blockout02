using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class KeypadLock : MonoBehaviour
{
    public static KeypadLock Instance;

    [Header("UI References")]
    public GameObject panel;                 // The overlay panel (set inactive in Awake)
    public TextMeshProUGUI displayText;      // TextMeshPro field that shows ●●●●
    [Header("Lock Settings")]
    public string correctCode = "1234";      // Change this in inspector
    public SlidingDoorController door;   // NEW                                         // Drag your DoorController here (optional)

    // internal
    private string entered = "";

    void Awake()
    {
        // Singleton setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Start hidden
        panel.SetActive(false);
    }

    /// <summary>
    /// Call this (e.g. from your interactor) to open the keypad UI.
    /// </summary>
    public void ShowKeypad()
    {
        entered = "";
        UpdateDisplay();

        panel.SetActive(true);
        IntelUI.IsUIOpen = true;
        // CursorManager will pick up the flag and unlock the cursor
    }

    /// <summary>
    /// Hook this to your Close button’s OnClick.
    /// </summary>
    public void HideKeypad()
    {
        panel.SetActive(false);
        IntelUI.IsUIOpen = false;
    }

    /// <summary>
    /// Wire each digit button’s OnClick → OnDigitPress("0"/"1"/…)
    /// </summary>
    public void OnDigitPress(string digit)
    {
        if (entered.Length >= correctCode.Length) return;
        entered += digit;
        UpdateDisplay();
    }

    /// <summary>
    /// (Optional) you can wire a “Clear” button to call this.
    /// </summary>
    public void OnClear()
    {
        entered = "";
        UpdateDisplay();
    }

    /// <summary>
    /// Wire your “Enter” button to call this.
    /// </summary>
    public void OnEnter()
    {
        if (entered == correctCode)
        {
            // Unlock the door if you assigned one
            if (door != null)
                door.UnlockAndOpen();

            HideKeypad();
        }
        else
        {
            StartCoroutine(ShakeDisplay());
            entered = "";
            UpdateDisplay();
        }
    }

    // update the display to show filled vs empty slots
    private void UpdateDisplay()
    {
        displayText.text = new string('●', entered.Length)
                         + new string('_', correctCode.Length - entered.Length);
    }

    // quick red-flash on the display when code is wrong
    private IEnumerator ShakeDisplay()
    {
        Color orig = displayText.color;
        displayText.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        displayText.color = orig;
    }
}
