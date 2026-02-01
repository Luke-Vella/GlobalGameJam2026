
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    public Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    public void Fire(Vector2 direction)
    {
        AudioManager.Instance.PlaySFX(AudioDatabase.Instance.FiringProjectilesClip);
        rb.velocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        Destroy(gameObject);
    }
}