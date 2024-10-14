using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallControl : NetworkBehaviour
{
    private Rigidbody2D rb2d;
    private TrailRenderer trailRenderer;
    private AudioSource audioSource;

    public override void OnNetworkSpawn()
    {
        rb2d = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();

        if (IsServer)
        {
            Invoke(nameof(StartBallMovement), 2f);
        }
    }

    private void StartBallMovement()
    {
        if (IsServer)
        {
            float rand = Random.Range(0, 2);
            Vector2 force = rand < 1 ? new Vector2(20, -15) : new Vector2(-20, -15);
            rb2d.AddForce(force);
        }
    }

    [ServerRpc]
    public void ResetBallServerRpc()
    {
        ResetBallClientRpc();
    }

    [ClientRpc]
    private void ResetBallClientRpc()
    {
        ResetBall();
    }

    private void ResetBall()
    {
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;

        if (trailRenderer != null)
        {
            trailRenderer.Clear();
            trailRenderer.emitting = true;
        }

        if (IsServer)
        {
            Invoke(nameof(StartBallMovement), 2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        PlayBounceAudioClientRpc();

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 vel;
            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2) + (collision.rigidbody.velocity.y / 3);
            rb2d.velocity = vel;
        }
    }

    [ClientRpc]
    private void PlayBounceAudioClientRpc()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}