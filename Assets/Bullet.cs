using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float speed = 10f;

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    [SerializeField] private int bulletDamage = 1;
    private int bounceCount = 0;

    private GameObject creator;

    public void Initialize(GameObject creator)
    {
        this.creator ??= creator;
        Collider2D bulletCollider = GetComponent<Collider2D>();
        Collider2D creatorCollider = creator.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(bulletCollider, creatorCollider, true);
        StartCoroutine(EnableCollisionWithCreator(bulletCollider, creatorCollider));
    }

    private IEnumerator EnableCollisionWithCreator(Collider2D bulletCollider, Collider2D creatorCollider)
    {
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(bulletCollider, creatorCollider, false);
        Physics2D.GetIgnoreCollision(bulletCollider, creatorCollider);
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate() => rb.velocity = transform.up * speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "indistructableWall" when bounceCount < 1:


                // vietoj bounceCount++ panaudoti bitines operacijas
                int increment = 1; //kiek pridesim prie bounceCount
                while (increment != 0)
                {
                    int carry = bounceCount & increment;  // Paziuri ar bus bitas kuri reikes paslenkti kairen
                    bounceCount = bounceCount ^ increment;      // Sudeda bitus (0+1 = 1, 1+1 = 0) nepasklenkant reiksmes
                    increment = carry << 1;         // Paslenka bita i kaire 0101->0110
                }


                Vector2 reflectDir = Vector2.Reflect(rb.velocity, collision.contacts[0].normal);
                rb.velocity = reflectDir.normalized * speed;
                transform.up = reflectDir;

                if (Mathf.Abs(Vector2.Dot(reflectDir, collision.contacts[0].normal)) < 0.1f)
                {
                    reflectDir = Quaternion.Euler(0, 0, 10) * reflectDir;
                    rb.velocity = reflectDir.normalized * speed;
                    transform.up = reflectDir;
                }
                break;

            case "indistructableWall":
                Destroy(gameObject);
                break;

            case "Bullet":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;

            default:
                IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
                if (damagable != null)
                {
                    damagable.Damage(bulletDamage);
                    Destroy(gameObject);
                }
                break;
        }
    }

}
