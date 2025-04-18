using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntelUI : MonoBehaviour
{
    public static IntelUI Instance;

    public GameObject intelPanel;
    public TextMeshProUGUI intelTitle;
    public TextMeshProUGUI intelBody;
    public static bool IsUIOpen = false;

    void Awake()
    {
        Instance = this;
        intelPanel.SetActive(false);
    }

    public void DisplayIntel(IntelProp.IntelType type, string content)
    {
        intelPanel.SetActive(true);
        intelTitle.text = type.ToString();
        intelBody.text = content;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        IsUIOpen = true;
    }

    public void CloseIntel()
    {
        intelPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        IsUIOpen = false;
    }
}