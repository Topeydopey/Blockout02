using UnityEngine;

public class DesignerNPC : MonoBehaviour
{
    public bool isAlien;

    [TextArea(2, 4)]
    public string[] humanResponses;
    [TextArea(2, 4)]
    public string[] alienResponses;

    private string chosenLine;

    void Start()
    {
        string[] source = isAlien ? alienResponses : humanResponses;

        if (source.Length > 0)
            chosenLine = source[Random.Range(0, source.Length)];
        else
            chosenLine = "…";
    }

    public string GetResponse()
    {
        return chosenLine;
    }
}
