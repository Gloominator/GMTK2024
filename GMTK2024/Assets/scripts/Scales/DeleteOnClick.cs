using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnClick : MonoBehaviour
{
    public float myWeight;
    public SpawnTestWeights.SpawnPlace mySpawnPlace;
    // Start is called before the first frame update


    

    public void SetWeightAndSpawnPlace(float weight,  SpawnTestWeights.SpawnPlace spawnPlace)
    {
        myWeight = weight;
        GetComponent<Rigidbody2D>().mass = weight;
        mySpawnPlace = spawnPlace;

        gameObject.name = spawnPlace.ToString()  + weight + " kg";
    }
}
