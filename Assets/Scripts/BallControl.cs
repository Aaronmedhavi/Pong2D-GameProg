using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Take Rigibody2D from ball
        Invoke("GoBall", 2); // Call GoBall function in 2 seconds
    }
    
    void GoBall()
    {
        float rand = Random.Range(0, 2); // Take random values between 0 to 1
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -15)); // AddForce give power
            // See how to use AddForce here : https://docs.unity3d.com/ScriptReference/Rigidbody.AddForce.html
        }
        else 
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    void ResetBall() // Make transform value = 0
    {
        rb2d.velocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) // If hit player
        {
            RotateBall();
            Vector2 vel;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3); // Takes player velocity value
            rb2d.velocity = vel;
        }
    }

    void RotateBall()
    {
        // Rotate the ball 180 degrees on the Z-axis
        transform.Rotate(0, 0, 180);
    }
}
