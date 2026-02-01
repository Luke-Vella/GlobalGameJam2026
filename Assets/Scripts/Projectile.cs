
using Assets.Scripts;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
            
        // Check if the object can take damage
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(); // You can add a damage parameter
            Destroy(gameObject);
            return;
        }

            Destroy(gameObject);
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            return;
        }

        // Check if the object can take damage
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(); // You can add a damage parameter
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);

    }
}