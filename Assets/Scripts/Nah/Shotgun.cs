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

    public AudioSource shotgunAudio;

    private CameraShake cameraShake;
    private float lastFireTime;

    void Start()
    {
        // Get the camera shake component from main camera
        cameraShake = GetComponent<CameraShake>();
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
        // Play audio
        if (shotgunAudio != null)
            shotgunAudio.Play();

        // Trigger camera shake
        if (cameraShake != null)
            cameraShake.Shake(0.15f, 0.1f); // duration, magnitude

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
}
