using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Prefabs to pick from (drop ALL variants here)")]
    public GameObject[] npcPrefabs;

    [Header("Spawn Points (empty GameObjects in scene)")]
    public Transform[] spawnPoints;

    public int npcCount = 10;       // total NPCs you want this round

    void Start() => SpawnNPCs();

    void SpawnNPCs()
    {
        if (npcPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("NPCSpawner: prefabs or spawn points are missing.");
            return;
        }

        int aliensSpawned = 0;

        for (int i = 0; i < npcCount; i++)
        {
            /* pick a random spawn point (reuse allowed) */
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

            /* small offset so they don't overlap perfectly */
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f),
                0f,
                Random.Range(-1f, 1f));

            /* pick a random prefab variant (human or alien) */
            GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

            GameObject npc = Instantiate(prefab,
                                         spawn.position + offset,
                                         spawn.rotation);

            /* tally aliens for the score system */
            DesignerNPC d = npc.GetComponent<DesignerNPC>();
            if (d && d.isAlien) aliensSpawned++;
        }

        /* inform ScoreManager of the real alien count */
        ScoreManager.Instance.SetTotalAliens(aliensSpawned);
    }
}
