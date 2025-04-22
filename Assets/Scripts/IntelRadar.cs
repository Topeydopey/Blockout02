using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarSystem : MonoBehaviour
{
    [Header("References")]
    public Transform player;                   // Your player’s transform
    public RectTransform radarContainer;       // The UI container (e.g. 200×200) for blips
    public RectTransform sweepLine;            // The UI Image you rotate to sweep
    public GameObject blipPrefab;              // A small UI Image prefab for a blip
    public LayerMask intelLayer;               // LayerMask for all Paper intel

    [Header("Short‑Range Mode")]
    public float shortRangeRadius = 10f;
    public float shortRangeSpeed = 180f;       // degrees per second

    [Header("Long‑Range Mode")]
    public float longRangeRadius = 25f;
    public float longRangeSpeed = 60f;

    // Internal state
    private float scanRadius, scanSpeed;
    private float currentAngle;
    private bool longRangeMode;
    private bool skipNextScan;
    private HashSet<Transform> scannedThisSweep = new HashSet<Transform>();

    void Start()
    {
        SetMode(false); // start in short‑range
    }

    void Update()
    {
        // 1) Toggle modes
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(false);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(true);

        // 2) Advance sweep
        currentAngle += scanSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
        {
            currentAngle -= 360f;
            scannedThisSweep.Clear();
            skipNextScan = true;  // skip one frame so we don’t immediately catch a center‑blip
        }

        // 3) Rotate the UI sweep line
        sweepLine.localRotation = Quaternion.Euler(0, 0, -currentAngle);

        // 4) Perform scan (unless skipping this frame)
        if (skipNextScan) skipNextScan = false;
        else ScanForIntel();
    }

    void SetMode(bool longRange)
    {
        longRangeMode = longRange;
        if (longRangeMode)
        {
            scanRadius = longRangeRadius;
            scanSpeed = longRangeSpeed;
        }
        else
        {
            scanRadius = shortRangeRadius;
            scanSpeed = shortRangeSpeed;
        }
        scannedThisSweep.Clear();
    }

    void ScanForIntel()
    {
        // find all intel props in range
        Collider[] hits = Physics.OverlapSphere(player.position, scanRadius, intelLayer);
        foreach (var hit in hits)
        {
            Transform intel = hit.transform;
            if (scannedThisSweep.Contains(intel)) continue;

            // direction in XZ plane
            Vector3 worldDir = intel.position - player.position;
            Vector2 planar = new Vector2(worldDir.x, worldDir.z);
            if (planar.sqrMagnitude < 0.01f) continue; // skip super‑close

            // compute angle of intel relative to world X+
            float intelAngle = (Mathf.Atan2(planar.y, planar.x) * Mathf.Rad2Deg + 360f) % 360f;

            // convert to angle relative to player's forward
            float playerYaw = player.eulerAngles.y;
            float relative = intelAngle - playerYaw;
            if (relative < 0) relative += 360f;

            // check if sweep line is over it (±threshold)
            float delta = Mathf.DeltaAngle(relative, currentAngle);
            if (Mathf.Abs(delta) <= 2f)  // sweep tolerance in degrees
            {
                scannedThisSweep.Add(intel);
                SpawnBlip(planar, worldDir.magnitude);
            }
        }
    }

    void SpawnBlip(Vector2 planarDir, float worldDist)
    {
        // normalized 0–1
        float nd = Mathf.Clamp01(worldDist / scanRadius);

        // rotate planarDir by -playerYaw so forward = up
        float rad = -player.eulerAngles.y * Mathf.Deg2Rad;
        float c = Mathf.Cos(rad), s = Mathf.Sin(rad);
        Vector2 rotated = new Vector2(
            planarDir.x * c - planarDir.y * s,
            planarDir.x * s + planarDir.y * c
        ).normalized;

        // place inside UI circle
        float uiR = radarContainer.rect.width * 0.5f;
        Vector2 pos = rotated * (nd * uiR);

        // instantiate and position
        var blip = Instantiate(blipPrefab, radarContainer);
        blip.GetComponent<RectTransform>().anchoredPosition = pos;
        Destroy(blip, 0.5f);  // auto‑fade
    }
}
