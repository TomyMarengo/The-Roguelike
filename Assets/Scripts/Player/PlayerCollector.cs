using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CircleCollider2D playerCollector;
    public float pullSpeed;

    private void Start() {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        playerCollector.radius = player.CurrentMagnet;
    }

    private IEnumerator AttractToPlayer(Rigidbody2D pickupRigidbody)
    {
        while (pickupRigidbody != null)
        {
            Vector2 directionToPlayer = (transform.position - pickupRigidbody.transform.position).normalized;
            pickupRigidbody.AddForce(directionToPlayer * pullSpeed);
            yield return null;
        }
    }


    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Pickup")) {
            StartCoroutine(AttractToPlayer(other.gameObject.GetComponent<Rigidbody2D>()));
        }
    }
}
