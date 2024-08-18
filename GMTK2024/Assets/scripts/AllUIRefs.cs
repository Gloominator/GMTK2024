using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AllUIRefs : MonoBehaviour
{
    public GameObject FactsSummaryScalesObject;
    public GameObject LiesCheckScalesObject;
    public GameObject PlayerChatBoxCanvas;
    public GameObject UIJudgeObj;
    public GameObject nextLevelButton;
    public GameObject judgeButtonPanel;
    public GameObject bubblesContainer;

    public GameManager gameManager;

    public Facts_Summary_Text_Sorter facts_Summary_Text_Sorter;

    public BubbleGenerator bubbleGenerator;



    private void Start()
    {
       
    }

    public void ResetUIToDefaultState()
    {
        UIManager.instance.lieDetectorObject.SetActive(true);
        FactsSummaryScalesObject.SetActive(true);
        PlayerChatBoxCanvas.SetActive(true);
        judgeButtonPanel.SetActive(true);
        bubblesContainer.SetActive(true);
        UIJudgeObj.SetActive(false);
        facts_Summary_Text_Sorter.ResetFactsSummaryScaleThing();
    }

    public void HideUIJudge()
    {
        UIManager.instance.lieDetectorObject.SetActive(false);
        LiesCheckScalesObject.GetComponentInChildren<SpawnTestWeights>().currentFeather.SetActive(false);
        // gets current feather

        //FactsSummaryScalesObject.SetActive(false);
        PlayerChatBoxCanvas.SetActive(false);
        judgeButtonPanel.SetActive(false);
        bubblesContainer.SetActive(false);
        UIJudgeObj.SetActive(true);
      
    }

    public void ShowNextLevelButton()
    {
        UIJudgeObj.SetActive(false);
        nextLevelButton.SetActive(true);
    }

    public void Judge()
    {
        gameManager.Judge();
    }

    public void HeavenOrHellChoose(bool isHeaven)
    {
        gameManager.HeavenOrHellChoose(isHeaven);
    }

    // REPLACED NEXT LEVEL FUNCTION
    public void AdvanceToNextCharacter()
    {
        // Move on to next character

        // 1. Do animation (IEnumerator wait for animation time) to show character leaving off screen


        // 2. Populate next character to be judged
        // 4. Update all text to reflect info in new character?
        GameManager.instance.currentCharacterJudged =
            GameManager.instance.characters[Random.Range(0, GameManager.instance.characters.Count)];
        GameManager.instance.loadCharacterDisplayText.LoadCharacterText(GameManager.instance.currentCharacterJudged);
        bubbleGenerator.GenerateBubblesOnStart();
        // 3. Reset state of all UI Elements
        ResetUIToDefaultState();
        



        UIManager.instance.inVerdictState = false;
        //
        // I don't know if im missing any steps, I am falling asleep at my desk right now.
        // If any questions are had, just send me a DM and I'll answer what I can when I
        // wake up.
    }
}
