using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private ParticleSystem gun;
    [SerializeField] private GameObject explosionPrefab;
    private List<ParticleCollisionEvent> collisionEvents = new();
    private Rigidbody2D rb;
    
    private void OnParticleCollision(GameObject other)
    {
        gun.GetCollisionEvents(other, collisionEvents);

        var intersection = collisionEvents[0].intersection;
        var explosion = Instantiate(explosionPrefab, intersection, Quaternion.identity);

        var p = explosion.GetComponent<ParticleSystem>();
        p.Play();

        if (other.GetComponent<Rigidbody2D>() != null)
        {
            Debug.Log("Adding force");
            other.GetComponent<Rigidbody2D>()
                .AddForceAtPosition((intersection * 100) - transform.position, intersection + Vector3.up);
        }
        
    }
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun.Play();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) gun.Play();
        // else if (Input.GetKeyUp(KeyCode.Space)) gun.Stop();
    }

    private void FixedUpdate()
    {
        var move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * 13, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * 500);
        }
    }
}
