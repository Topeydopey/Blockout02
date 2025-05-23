using UnityEngine;

public class BossManager : MonoBehaviour
{
    public static BossManager Instance;

    [Header("Boss Setup")]
    public GameObject bossPrefab;        // Boss_RobertYang prefab
    public Transform spawnPoint;         // one empty on upper deck

    [Header("Credits")]
    public CreditController credits;     // drag component (next section)

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    /* called once from StaticInteraction (engine wall) */
    public void SpawnFinalBoss()
    {
        Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    /* called by the boss on death */
    public void OnBossKilled()
    {
        credits.ShowCredits();
    }
}
