using System.Collections.Generic;
using UnityEngine;

public class FolderUnlockManager : MonoBehaviour
{
    public static FolderUnlockManager Instance;

    private HashSet<string> unlockedFolders = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MarkAsUnlocked(string folderID)
    {
        if (!unlockedFolders.Contains(folderID))
        {
            unlockedFolders.Add(folderID);
            Debug.Log($"Folder unlocked: {folderID}");
        }
    }

    public bool IsUnlocked(string folderID)
    {
        return unlockedFolders.Contains(folderID);
    }
}
