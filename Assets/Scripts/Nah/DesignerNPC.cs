using UnityEngine;

public class DesignerNPC : MonoBehaviour
{
    [TextArea(2, 4)]
    public string[] humanResponses;
    [TextArea(2, 4)]
    public string[] alienResponses;

    public bool isAlien = false; // Set this manually or randomize later

    public string GetRandomResponse()
    {
        if (isAlien && alienResponses.Length > 0)
            return alienResponses[Random.Range(0, alienResponses.Length)];

        if (!isAlien && humanResponses.Length > 0)
            return humanResponses[Random.Range(0, humanResponses.Length)];

        return "Uh... I forgot what I was saying.";
    }
}
