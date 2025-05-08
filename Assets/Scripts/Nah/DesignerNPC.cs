using System.Collections.Generic;
using UnityEngine;

public class DesignerNPC : MonoBehaviour
{
    public bool isAlien;

    [TextArea(2, 4)]
    public string[] humanResponses;
    [TextArea(2, 4)]
    public string[] alienResponses;

    private Queue<string> responseQueue = new Queue<string>();
    private string lastLine = "";

    private void Start()
    {
        ResetResponseQueue(); // initialize at spawn
    }

    public string GetNextResponse()
    {
        if (responseQueue.Count == 0)
        {
            ResetResponseQueue(); // refill if we've used all
        }

        lastLine = responseQueue.Dequeue();
        return lastLine;
    }

    private void ResetResponseQueue()
    {
        string[] source = isAlien ? alienResponses : humanResponses;

        // Shuffle the array before enqueuing
        List<string> shuffled = new List<string>(source);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int rand = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[rand]) = (shuffled[rand], shuffled[i]);
        }

        foreach (var line in shuffled)
            responseQueue.Enqueue(line);
    }
}
