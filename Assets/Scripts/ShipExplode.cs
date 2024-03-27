using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipExplode : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
