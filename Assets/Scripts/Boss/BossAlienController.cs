using UnityEngine;

public class BossAlienController : MonoBehaviour
{
    public int hp = 1;
    AudioSource src;

    void Awake() => src = gameObject.AddComponent<AudioSource>();

    public void TakeHit()
    {
        hp--;
        if (hp <= 0) Die();
    }

    void Die()
    {
        BossManager.Instance?.OnBossKilled();
        Destroy(gameObject);
    }
}
