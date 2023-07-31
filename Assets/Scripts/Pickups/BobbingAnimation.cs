using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    public float frequency; // Speed of movement
    public float magnitude; // Range of movement
    public Vector3 direction; // Direction of movement
    Vector3 initialPosition;

    private void Start() {
        initialPosition = transform.position;
    }

    private void Update() {
        transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
    }
}
