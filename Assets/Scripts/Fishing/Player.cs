using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) gun.Play();
        // else if (Input.GetKeyUp(KeyCode.Space)) gun.Stop();
    }

    private void FixedUpdate()
    {
        var move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * 10, rb.velocity.y);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * 500);
        }
    }
}
