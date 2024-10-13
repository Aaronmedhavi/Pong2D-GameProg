using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerControl : NetworkBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite player1Sprite;
    public Sprite player2Sprite;
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDown = KeyCode.S;
    public float speed = 10.0f;
    public float boundY = 2.25f;
    public Rigidbody2D rb2d;
    private float player1SpawnPosition = -5.813324f;
    private float player2SpawnPosition = 5.813324f;
    private NetworkVariable<int> playerSpriteIndex = new NetworkVariable<int>(0);
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerSpriteIndex.OnValueChanged += OnPlayerSpriteChanged;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            if (OwnerClientId == 0)
            {
                transform.position = new Vector2(player1SpawnPosition, 0);
                playerSpriteIndex.Value = 0;
            }
            else
            {
                // Player 2
                transform.position = new Vector2(player2SpawnPosition, 0);
                playerSpriteIndex.Value = 1;
            }
        }
        UpdatePlayerSprite(playerSpriteIndex.Value);
    }

    private void OnPlayerSpriteChanged(int oldSpriteIndex, int newSpriteIndex)
    {
        UpdatePlayerSprite(newSpriteIndex);
    }

    private void UpdatePlayerSprite(int spriteIndex)
    {
        if (spriteIndex == 0)
        {
            spriteRenderer.sprite = player1Sprite;
            Debug.Log("Player 1 Sprite Assigned");
        }
        else if (spriteIndex == 1)
        {
            spriteRenderer.sprite = player2Sprite;
            Debug.Log("Player 2 Sprite Assigned");
        }
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        var vel = rb2d.velocity;
        if (Input.GetKey(moveUp))
        {
            vel.y = speed;
        }
        else if (Input.GetKey(moveDown))
        {
            vel.y = -speed;
        }
        else
        {
            vel.y = 0;
        }
        rb2d.velocity = vel;

        var pos = transform.position;
        if (pos.y > boundY)
        {
            pos.y = boundY;
        }
        else if (pos.y < -boundY)
        {
            pos.y = -boundY;
        }
        transform.position = pos;
    }
}
