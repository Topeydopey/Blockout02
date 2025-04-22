using UnityEngine;

public class IntelProp : MonoBehaviour
{
    public enum IntelType { Paper, Folder }
    public IntelType intelType;

    [TextArea(3, 10)]
    public string intelContent;

    [Header("For Paper Clues")]
    public string unlocksFolderID;

    [Header("For Folder")]
    public string folderID;

    public void Interact()
    {
        // Folder locked check
        if (intelType == IntelType.Folder && !FolderUnlockManager.Instance.IsUnlocked(folderID))
        {
            Debug.Log("Folder is locked. Need correct clue first.");
            return;
        }

        IntelUI.Instance.DisplayIntel(intelType, intelContent);

        if (intelType == IntelType.Paper && !string.IsNullOrEmpty(unlocksFolderID))
        {
            FolderUnlockManager.Instance.MarkAsUnlocked(unlocksFolderID);
        }

        gameObject.SetActive(false);
    }
}
