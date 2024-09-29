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
        // Schedule the GoBall method to be called after a 2-second delay.
        Invoke("GoBall", 2);
    }

    // The GoBall method applies an initial force to the ball to start its movement.
    void GoBall()
    {
        // Generate a random floating-point number between 0 and 2.
        float rand = Random.Range(0, 2);

        // If the random number is less than 1, apply force in one direction.
        if (rand < 1)
        {
            // Apply a force to the ball to move it to the right and slightly downward.
            rb2d.AddForce(new Vector2(20, -15));
        }
        else
        {
            // Apply a force to the ball to move it to the left and slightly downward.
            rb2d.AddForce(new Vector2(-20, -15));
        }
    }

    // The ResetBall method stops the ball's movement and returns it to the center of the screen.
    void ResetBall()
    {
        // Set the ball's velocity to zero, stopping any movement.
        rb2d.velocity = Vector2.zero;

        // Set the ball's angular velocity to zero, stopping any rotation.
        rb2d.angularVelocity = 0f;

        // Move the ball's position to the center of the screen (coordinates 0,0).
        transform.position = Vector2.zero;

        // Reset the ball's rotation to its original state.
        transform.rotation = Quaternion.identity;

        // If a TrailRenderer is attached, stop emitting the trail and clear any existing trail.
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
            trailRenderer.Clear();
        }
    }

    // The RestartGame method resets the ball and prepares it to start moving again.
    void RestartGame()
    {
        // Call the ResetBall method to stop and center the ball.
        ResetBall();

        // Start a coroutine that will re-enable the trail after a short delay.
        // Coroutines allow for waiting periods without freezing the entire game.
        StartCoroutine(EnableTrailAfterDelay(1.0f));
    }

    // This coroutine waits for a specified delay before re-enabling the trail and moving the ball.
    private IEnumerator EnableTrailAfterDelay(float delay)
    {
        // Wait for the specified number of seconds before continuing.
        yield return new WaitForSeconds(delay);

        // If a TrailRenderer is attached, start emitting the trail again.
        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        // Call the GoBall method to apply force and start the ball moving.
        GoBall();
    }

    // This method is called automatically by Unity whenever the ball collides with another object.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If an AudioSource is attached, play the collision sound.
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Check if the object the ball collided with has the tag "Player".
        if (collision.collider.CompareTag("Player"))
        {
            // Create a new Vector2 to hold the new velocity of the ball.
            Vector2 vel;

            // Keep the current horizontal (x) velocity of the ball.
            vel.x = rb2d.velocity.x;

            // Adjust the vertical (y) velocity based on the ball's current velocity and the player's movement.
            vel.y = (rb2d.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);

            // Apply the new velocity to the ball.
            rb2d.velocity = vel;
        }
    }
}
