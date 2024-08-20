using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SpawnTestWeights;

public class SpawnTestWeights : MonoBehaviour
{
    public GameObject weightPrefab;
    public GameObject heartPrefab;
    public GameObject featherPrefab;

    public float lightHeartWeight = 0;
    public float heavyHeartWeight = 5;

    public GameObject spawnPointLeft;
    public GameObject spawnPointRight;
    public Scales_Balance_Calc scales_Balance_Calc;

    [SerializeField] float howLongWaitForNewHeartSpawn = 4;
    [SerializeField] float howLongBeforeFeatherDespawns = 4;

    public GameObject currentFeather;
    public enum SpawnPlace
    {
        Left,
        Right,
    }

    private void Start()
    {
       //  SpawnObjRight(1f); // debug
    }
    public GameObject SpawnObjRight(float weight)
    {
     GameObject _spawnedWeight =    SpawnObj(weight, SpawnPlace.Right);
       
        return _spawnedWeight;
    }

    public GameObject SpawnObjLeft(float weight)
    {
        GameObject _spawnedWeight = SpawnObj(weight, SpawnPlace.Left);
        return _spawnedWeight;
    }

    public void SpawnHeartLeft(bool isHeavy)
    {
        GameObject instantiatedWeight = null;
        if (isHeavy)
        {
             instantiatedWeight = Instantiate(heartPrefab, spawnPointLeft.transform.position, Quaternion.identity);
           
             



            DeleteOnClick thisInstantiatedObjScript = instantiatedWeight.GetComponent<DeleteOnClick>();
            thisInstantiatedObjScript.SetWeightAndSpawnPlace(heavyHeartWeight, SpawnPlace.Left);
            scales_Balance_Calc.AddWeight(heavyHeartWeight, SpawnPlace.Left);
        }
        else
        {
            instantiatedWeight = Instantiate(heartPrefab, spawnPointLeft.transform.position, Quaternion.identity);



            DeleteOnClick thisInstantiatedObjScript = instantiatedWeight.GetComponent<DeleteOnClick>();
            thisInstantiatedObjScript.SetWeightAndSpawnPlace(lightHeartWeight, SpawnPlace.Left);
            scales_Balance_Calc.AddWeight(lightHeartWeight, SpawnPlace.Left);
        }

        instantiatedWeight.transform.parent = this.transform; // makes sure the heart disappears with scales

        //despawn heart, spawn feather after a while
        Destroy(instantiatedWeight, howLongWaitForNewHeartSpawn);
        Destroy(currentFeather, howLongBeforeFeatherDespawns);
        StartCoroutine(SpawnFeatherAfterHeartDespawn());

    }
    IEnumerator SpawnFeatherAfterHeartDespawn()
    {
        yield return new WaitForSeconds(howLongWaitForNewHeartSpawn);
        GameObject feather = Instantiate(featherPrefab, spawnPointRight.transform.position, Quaternion.identity);
        feather.transform.parent = this.transform.parent; // makes sure the feather disapepars with scales
        currentFeather = feather;
        DeleteOnClick thisInstantiatedObjScript = feather.GetComponent<DeleteOnClick>();
        thisInstantiatedObjScript.SetWeightAndSpawnPlace(0, SpawnPlace.Right);
       
    }
    GameObject SpawnObj(float weight, SpawnPlace spawnPlace)
    {
        GameObject instantiatedWeight = null;
        if (spawnPlace == SpawnPlace.Left)
        {
            instantiatedWeight = Instantiate(weightPrefab, spawnPointLeft.transform.position, Quaternion.identity);
           
            
           
            DeleteOnClick thisInstantiatedObjScript = instantiatedWeight.GetComponent<DeleteOnClick>();
            thisInstantiatedObjScript.SetWeightAndSpawnPlace(weight, spawnPlace);
            scales_Balance_Calc.AddWeight(weight, spawnPlace);
        }

        if (spawnPlace == SpawnPlace.Right)
        {
             instantiatedWeight = Instantiate(weightPrefab, spawnPointRight.transform.position, Quaternion.identity);


            DeleteOnClick thisInstantiatedObjScript = instantiatedWeight.GetComponent<DeleteOnClick>();
            thisInstantiatedObjScript.SetWeightAndSpawnPlace(weight, spawnPlace);
            scales_Balance_Calc.AddWeight(weight, spawnPlace);

        }
        return instantiatedWeight;
    }
}
