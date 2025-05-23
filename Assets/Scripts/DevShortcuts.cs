using UnityEngine;

public class DevShortcuts : MonoBehaviour
{
    [Tooltip("Press this to flag every alien as dead (no object deletion).")]
    public KeyCode instantWinKey = KeyCode.F9;

    void Update()
    {
        if (Input.GetKeyDown(instantWinKey))
        {
            int aliensCounted = 0;

            foreach (DesignerNPC npc in FindObjectsOfType<DesignerNPC>())
            {
                if (npc.isAlien && ScoreManager.Instance != null)
                {
                    ScoreManager.Instance.RegisterKill(true);  // counts as alien kill
                    aliensCounted++;
                }
            }

            Debug.Log($"[DevShortcuts] Registered {aliensCounted} alien kills via {instantWinKey}");
        }
    }
}
