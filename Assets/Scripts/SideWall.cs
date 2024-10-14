using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SideWall : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!IsServer) return;

        if (hitInfo.CompareTag("Ball"))
        {
            string wallName = transform.name;
            GameManager.Instance.ScoreServerRpc(wallName);
            BallControl BallControl = hitInfo.GetComponent<BallControl>();
            if (BallControl != null)
            {
                BallControl.ResetBallServerRpc();
            }
        }
    }
}