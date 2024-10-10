using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] public int health = 5;

    public Transform target;
    public float speed = 3f;
    public float rotateSpeed = 0.0025f;

    [SerializeField] private int weaponDamage = 10;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed;
    }

    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg -
            90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed);
    }

    private void GetTarget()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        //if (damagable != null)
        //{
        //    damagable.Damage(weaponDamage);
        //}

        if (collision.gameObject.GetComponent<IDamagable>() is IDamagable damagable)
        {
            damagable.Damage(weaponDamage);
        }
    }
    public void Hit(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(int damageAmount)
    {
        Hit(damageAmount);
    }
}
