using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private AudioSource audioSource;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); 
        trailRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
        Invoke("GoBall", 2); 
    }
    
    void GoBall()
    {
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -15)); 
        }
        else 
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    void ResetBall() // Make transform value = 0
    {
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
            trailRenderer.Clear();
        }
    }

    void RestartGame()
    {
        ResetBall();
        StartCoroutine(EnableTrailAfterDelay(1.0f));
    }

    private IEnumerator EnableTrailAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }
        GoBall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        if (collision.collider.CompareTag("Player"))
        {
            Vector2 vel;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            rb2d.velocity = vel;
        }
    }
}
