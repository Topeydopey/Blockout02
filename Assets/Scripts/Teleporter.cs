using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("Destination")]
    public Transform target;

    [Header("Behaviour")]
    public bool keepPlayerRotation = true;
    public float cooldownSeconds = 0.3f;

    [Header("Phase-2 gating")]
    [Tooltip("If checked, this teleporter is OFF until ScoreManager fires onAllAliensKilled.")]
    public bool enableOnAct2 = false;          // ← NEW toggle

    /* ---- private ---- */
    static bool globalCoolDown;
    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Start()
    {
        if (enableOnAct2)
        {
            col.enabled = false;                           // keep OFF
            if (ScoreManager.Instance)
                ScoreManager.Instance.onAllAliensKilled
                    .AddListener(EnableTeleporter);
        }
    }

    void EnableTeleporter() => col.enabled = true;         // called by event

    void OnDestroy()
    {
        if (enableOnAct2 && ScoreManager.Instance)
            ScoreManager.Instance.onAllAliensKilled
                .RemoveListener(EnableTeleporter);
    }

    void OnTriggerEnter(Collider other)
    {
        if (globalCoolDown || !col.enabled) return;
        if (!other.CompareTag("Player") || target == null) return;

        Transform player = other.transform;

        var cc = player.GetComponent<CharacterController>();
        if (cc && cc.enabled) cc.enabled = false;

        player.position = target.position;
        player.rotation = keepPlayerRotation
            ? Quaternion.Euler(0, player.eulerAngles.y, 0)
            : target.rotation;

        if (cc) cc.enabled = true;

        StartCoroutine(CoolDownRoutine());
    }

    System.Collections.IEnumerator CoolDownRoutine()
    {
        globalCoolDown = true;
        yield return new WaitForSeconds(cooldownSeconds);
        globalCoolDown = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (target)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.2f,
                            target.position + Vector3.up * 0.2f);
        }
    }
#endif
}
