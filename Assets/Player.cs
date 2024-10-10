using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Player : MonoBehaviour, IDamagable, IFormattable
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private int health = 10;
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [Range(0.1f, 2f)]
    [SerializeField] private float fireRate = 0.5f;

    [SerializeField] private float teleportDistance = 5f;
    [SerializeField] private float teleportCooldown = 2f;

    private Rigidbody2D rb;
    private float mx;
    private float my;

    private float fireTimer;
    private float teleportTimer;

    private Vector2 mousePos;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        my = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x -
            transform.position.x) * Mathf.Rad2Deg - 90f;

        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            Shoot();
            fireTimer = fireRate;
        } else
        {
            fireTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && teleportTimer <= 0f)
        {
            Teleport();
            teleportTimer = teleportCooldown;
        }
        else
        {
            teleportTimer -= Time.deltaTime;
        }

        // Debug.Log(this.ToString("G", null));

    }

    private void FixedUpdate() => rb.velocity = new Vector2(mx, my).normalized * speed;

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bullet.GetComponent<Bullet>().Initialize(gameObject);
    }

    private void Teleport()
    {
        Vector2 direction = new Vector2(mx, my).normalized;
        rb.position += direction * teleportDistance;
    }

    public void Hit(int damageAmount)
    {
        health -= damageAmount;

        if(health <= 0)
        {
            Destroy(gameObject);
            LevelManager.manager.GameOver();
        }
        
    }
    public void Damage(int damageAmount)
    {
        Hit(damageAmount);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        if (string.IsNullOrEmpty(format)) format = "G";
        if (formatProvider == null) formatProvider = System.Globalization.CultureInfo.CurrentCulture;

        switch (format.ToUpperInvariant())
        {
            case "G":
                return $"Player: CurrentVelocity={rb.velocity}, Health={health}, FireRateTimer={fireTimer}, TeleportTimer={teleportTimer}";
            case "H":
                return $"Health={health}";
            default:
                throw new FormatException($"The {format} format string is not supported.");
        }
    }

    public override string ToString()
    {
        return ToString("G", null);
    }

}
