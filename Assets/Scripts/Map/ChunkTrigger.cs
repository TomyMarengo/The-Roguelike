using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTrigger : MonoBehaviour
{
    MapController mapController;
    public GameObject targetMap;

    // Start is called before the first frame update
    void Start()
    {
        mapController = FindObjectOfType<MapController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            mapController.currentChunk = targetMap;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            if(mapController.currentChunk == targetMap) {
                mapController.currentChunk = null;
            }
        }
    }
}
