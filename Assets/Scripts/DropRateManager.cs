using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;

    void OnDestroy() {
        if(!this.gameObject.scene.isLoaded) return;
        
        float randomNumber = UnityEngine.Random.Range(0f, 100f);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops drop in drops) {
            if(randomNumber <= drop.dropRate) {
                possibleDrops.Add(drop);
            }
        }
        if(possibleDrops.Count > 0) {
            Drops chosenDrop = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(chosenDrop.itemPrefab, transform.position, Quaternion.identity); 
        }
    }
}
