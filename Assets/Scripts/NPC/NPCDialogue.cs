using UnityEngine;

[System.Serializable] public struct QA { public string question; public string answer; }

public class NPCDialogue : MonoBehaviour
{
    [Header("Place up to 3 Q&A pairs")]
    public QA[] dialogue = new QA[3];
    
    public void Talk(int index, System.Action<string> callback)
    {
        if (index >= 0 && index < dialogue.Length)
            callback?.Invoke(dialogue[index].answer);
    }
}
