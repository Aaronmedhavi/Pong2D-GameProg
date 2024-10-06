using System.Collections;
using UnityEngine;
using Photon.Pun;  // Import Photon PUN

public class SideWall : MonoBehaviourPunCallbacks
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Ball"))
        {
            Debug.Log("Ball hit the wall: " + transform.name);

            if (PhotonNetwork.IsMasterClient)
            {
                string wallName = transform.name;

                GameManager.instance.Score(wallName);
                hitInfo.gameObject.GetComponent<BallControl>().RestartGame(1.0f);
            }
        }
    }

}
