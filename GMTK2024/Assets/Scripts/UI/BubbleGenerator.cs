using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleGenerator : MonoBehaviour
{

    public float bubbleSpawnAreaRadius;
    public float bubbleMinDistanceRadius;
    public float exclusionZoneRadius;

    public int bubblesToGenerate;

    public GameObject bubblePrefab;

    public List<GameObject> bubbles;




    // Start is called before the first frame update
    void Start()
    {
        GenerateBubblesOnStart();
    }



    public void GenerateBubblesOnStart()
    {
        GenerateBubble();
    }

    public void ClearBubbles()
    {
        bubbles.Clear();
    }

    void GenerateBubble()
    {
        for (int i = 0; i < bubblesToGenerate; i++)
        {
            bubbles[i].GetComponent<ThoughtBubble>().question = GameManager.instance.currentCharacterJudged.facts[i + 1].question;
            bubbles[i].GetComponent<ThoughtBubble>().UpdateText();
        }
    }


}
