using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnClick : MonoBehaviour
{
    public float myWeight;
    public SpawnTestWeights.SpawnPlace mySpawnPlace;

    public Sprite featherSprite;
    // Start is called before the first frame update


    //on create object, check if it has feather sprite, then after 2 sec change its spriterenderer layer to -2 (bugfix maybe)
    void Start()
    {

        if (GetComponent<SpriteRenderer>().sprite == featherSprite)
        {
            StartCoroutine(AfterSecHideFeather(2));
        }
    }
    IEnumerator AfterSecHideFeather(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<SpriteRenderer>().sortingOrder = -2;
    }

    public void SetWeightAndSpawnPlace(float weight,  SpawnTestWeights.SpawnPlace spawnPlace)
    {
        myWeight = weight;
        GetComponent<Rigidbody2D>().mass = weight;
        mySpawnPlace = spawnPlace;

        gameObject.name = spawnPlace.ToString()  + weight + " kg";
    }
}
