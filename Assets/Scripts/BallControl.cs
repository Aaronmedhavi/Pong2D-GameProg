using System.Collections;
using UnityEngine;
using Photon.Pun;  // Import Photon PUN

[RequireComponent(typeof(PhotonView))]
public class BallControl : MonoBehaviourPunCallbacks
{
    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private AudioSource audioSource;

    void Start()
    {
        // Get the necessary components
        rb2d = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Only the Master Client should start the ball movement
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("GoBall", 2);
        }
    }

    // The GoBall method applies an initial force to the ball to start its movement
    void GoBall()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        float rand = Random.Range(0, 2);

        // Apply force based on the random value
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -15));
        }
        else
        {
            rb2d.AddForce(new Vector2(-20, -15));
        }

        // Sync ball's movement across the network
        photonView.RPC("SyncBallMovement", RpcTarget.OthersBuffered, rb2d.velocity);
    }

    // Synchronize ball movement across clients
    [PunRPC]
    void SyncBallMovement(Vector2 velocity)
    {
        rb2d.velocity = velocity;
    }

    // Reset ball to its initial state and stop movement
    void ResetBall()
    {
        if (!PhotonNetwork.IsMasterClient) return;  // Only the Master Client resets the ball

        // Reset position and velocity
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;

        // Disable trail effect on all clients
        photonView.RPC("SetTrailActive", RpcTarget.All, false);
        if (trailRenderer != null)
        {
            trailRenderer.Clear();
        }

        // Sync the reset with other clients
        photonView.RPC("SyncResetBall", RpcTarget.Others);
    }

    // Synchronize ball reset across clients
    [PunRPC]
    void SyncResetBall()
    {
        // Reset position and velocity without calling another RPC
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;

        // Disable the trail locally
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
            trailRenderer.Clear();
        }
    }

    // Restart the ball and enable the trail after a delay
    public void RestartGame(float delay)
    {
        if (!PhotonNetwork.IsMasterClient) return;  // Only the Master Client restarts the game

        ResetBall();
        StartCoroutine(EnableTrailAfterDelay(delay));
    }

    // Coroutine to enable the trail after a delay and restart the ball movement
    private IEnumerator EnableTrailAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Enable the trail effect on all clients
        photonView.RPC("SetTrailActive", RpcTarget.All, true);

        // Restart the ball movement
        GoBall();
    }

    // Handle ball collision events
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Play sound on collision
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // If the ball collides with a player paddle, adjust its velocity
        if (collision.collider.CompareTag("Player"))
        {
            Vector2 vel = rb2d.velocity;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (collision.collider.attachedRigidbody.velocity.y / 3);
            rb2d.velocity = vel;

            // Sync the new ball velocity across the network
            photonView.RPC("SyncBallMovement", RpcTarget.Others, rb2d.velocity);
        }
    }

    // RPC to control the trail rendering on all clients
    [PunRPC]
    void SetTrailActive(bool active)
    {
        if (trailRenderer != null)
        {
            trailRenderer.emitting = active;
        }
    }
}
