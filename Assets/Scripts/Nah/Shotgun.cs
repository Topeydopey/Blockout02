using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Ballistics")]
    public float range = 10f;
    public float spreadAngle = 10f;
    public int pelletCount = 8;
    public float fireCooldown = 1f;
    public KeyCode fireKey = KeyCode.Mouse0;

    [Header("Effects")]
    public GameObject redBloodEffect;
    public GameObject greenBloodEffect;
    public AudioSource shotgunAudio;
    private CameraShake cameraShake;

    float lastFireTime;

    void Start()
    {
        cameraShake = GetComponent<CameraShake>();   // assumes CameraShake is on the same object
    }

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
        if (shotgunAudio) shotgunAudio.Play();
        if (cameraShake) cameraShake.Shake();      // uses the defaults set in CameraShake

        // fire multiple pellets
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 dir = GetSpreadDirection();
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range))
            {
                DesignerNPC npc = hit.collider.GetComponent<DesignerNPC>();
                if (npc)
                {
                    SpawnBlood(npc, hit.point);
                    ScoreManager.Instance.RegisterKill(npc.isAlien);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    Vector3 GetSpreadDirection()
    {
        Vector3 fwd = transform.forward;
        fwd += Random.insideUnitSphere * Mathf.Tan(spreadAngle * Mathf.Deg2Rad);
        return fwd.normalized;
    }

    void SpawnBlood(DesignerNPC npc, Vector3 pos)
    {
        GameObject fx = Instantiate(
            npc.isAlien ? greenBloodEffect : redBloodEffect,
            pos,
            Quaternion.identity);

        Destroy(fx, 2f);
    }
}
