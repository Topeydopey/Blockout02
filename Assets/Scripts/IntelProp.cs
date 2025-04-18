using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelProp : MonoBehaviour
{
    public enum IntelType { Paper, Folder }
    public IntelType intelType;

    [TextArea(3,10)]
    public string intelContent;

    public void Interact()
    {
        IntelUI.Instance.DisplayIntel(intelType, intelContent);
        gameObject.SetActive(false);
    }
}
