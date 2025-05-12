using UnityEngine;
using System.Linq;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public Transform[] spawnPoints;
    public int npcCount = 5;
    [Range(0f, 1f)] public float alienChance = 0.25f; // 0.25 = 25% of NPCs will be aliens

    void Start()
    {
        SpawnNPCs();
    }

    void SpawnNPCs()
    {
        int aliensSpawned = 0;

        for (int i = 0; i < npcCount; i++)
        {
            // Pick a random spawn point
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Slightly randomize local offset so they don't overlap
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f),
                0,
                Random.Range(-1f, 1f)
            );

            GameObject npc = Instantiate(npcPrefab, spawn.position + offset, Quaternion.identity);

            DesignerNPC npcScript = npc.GetComponent<DesignerNPC>();
            if (npcScript != null)
            {
                npcScript.isAlien = Random.value < alienChance;
                if (npcScript.isAlien)
                    aliensSpawned++;
            }
        }

        // Send real alien count to score system
        ScoreManager.Instance.SetTotalAliens(aliensSpawned);
    }
}
