using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public float range = 10f;
    public float spreadAngle = 10f;
    public int pelletCount = 8;
    public float fireCooldown = 1f;
    public KeyCode fireKey = KeyCode.Mouse0;

    public GameObject redBloodEffect;
    public GameObject greenBloodEffect;

    //public GameObject redBloodPoolPrefab;
    //public GameObject greenBloodPoolPrefab;

    public AudioSource shotgunAudio;

    private float lastFireTime;

    void Update()
    {
        if (Input.GetKeyDown(fireKey) && Time.time > lastFireTime + fireCooldown)
        {
            FireShotgun();
            lastFireTime = Time.time;
        }
    }

    void FireShotgun()
    {
        // Play audio
        if (shotgunAudio != null)
            shotgunAudio.Play();

        // Fire multiple raycasts
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 direction = GetSpreadDirection();
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, range))
            {
                DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();
                if (npc != null)
                {
                    Vector3 hitPoint = hit.point;
                    SpawnBlood(npc, hitPoint);
                    //                   SpawnBloodPool(npc, hit.collider.transform.position);

                    // Hide dialogue if that NPC was talking
                    FindObjectOfType<TypewriterDialogue>()?.HideNow();

                    ScoreManager.Instance.RegisterKill(npc.isAlien);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    Vector3 GetSpreadDirection()
    {
        Vector3 forward = transform.forward;
        forward += Random.insideUnitSphere * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
        return forward.normalized;
    }

    void SpawnBlood(DesignerNPC npc, Vector3 position)
    {
        GameObject blood = Instantiate(
            npc.isAlien ? greenBloodEffect : redBloodEffect,
            position,
            Quaternion.identity
        );

        Destroy(blood, 2f);
    }
    /*
        void SpawnBloodPool(DesignerNPC npc, Vector3 basePosition)
        {
            Debug.Log($"Attempting to spawn blood pool for {(npc.isAlien ? "alien" : "human")}");

            Vector3 rayStart = basePosition + Vector3.up * 0.5f;
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit groundHit, 2f))
            {
                Debug.Log("Raycast hit at " + groundHit.point);

                Vector3 spawnPos = groundHit.point + Vector3.up * 0.01f;
                Quaternion rotation = Quaternion.Euler(90f, Random.Range(0f, 360f), 0f);

                GameObject poolPrefab = npc.isAlien ? greenBloodPoolPrefab : redBloodPoolPrefab;

                GameObject spawned = Instantiate(poolPrefab, spawnPos, rotation);
                Debug.Log("Spawned blood pool at " + spawnPos);
            }
            else
            {
                Debug.LogWarning("Blood pool raycast did not hit anything.");
            }
        }
        */
}
