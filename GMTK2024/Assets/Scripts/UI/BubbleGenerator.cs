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
        bubblesToGenerate = GameManager.instance.currentCharacterJudged.facts.Count;
        GenerateBubble();
    }



    void GenerateBubble()
{
    for (int i = 1; i < bubblesToGenerate; i++)
    {
        GameObject bubble = Instantiate(bubblePrefab, gameObject.transform);
        ThoughtBubble thoughtBubble = bubble.GetComponent<ThoughtBubble>();
        bubble.name = $"bubble{i:00}";

        thoughtBubble.question = GameManager.instance.currentCharacterJudged.facts[i].question;

        thoughtBubble.index = i;

        RectTransform bubbleRect = bubble.GetComponent<RectTransform>();

        Vector2 newPoint;
        bool isValidPoint;
        int attempts = 0;
        int maxAttempts = 1000; // Safety limit

        do
        {
            // Generate a new point within the unit circle in 2D space
            newPoint = Random.insideUnitCircle * bubbleSpawnAreaRadius;
            isValidPoint = true;

            // Exclude the point if it is in the exclusion zone or the bottom half of the circle
            if (newPoint.magnitude < exclusionZoneRadius || newPoint.y < -0.5f)
            {
                isValidPoint = false;
            }

            // Check against all existing bubbles
            for (int j = 0; j < bubbles.Count && isValidPoint; j++)
            {
                float distFromOtherBubble = Vector2.Distance(bubbles[j].GetComponent<RectTransform>().anchoredPosition, newPoint);

                if (distFromOtherBubble < bubbleMinDistanceRadius)
                {
                    isValidPoint = false;
                    break;
                }
            }

            attempts++;
            if (attempts > maxAttempts)
            {
                Debug.LogWarning($"Could not find a valid point for bubble {i} after {attempts} attempts.");
                isValidPoint = true; // Force accept the point to avoid infinite loops
                break;
            }

        } while (!isValidPoint);

        // Set the bubble's position to the valid point in screen space
        bubbleRect.anchoredPosition = newPoint;

        // Add the bubble to the list
        bubbles.Add(bubble);

        UIManager.instance.questionsRemaining += 1;
    }
}
    
    
}
