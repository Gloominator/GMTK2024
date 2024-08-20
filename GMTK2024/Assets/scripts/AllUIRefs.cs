using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AllUIRefs : MonoBehaviour
{
    public GameObject FactsSummaryScalesObject;
    public GameObject LiesCheckScalesObject;
    public GameObject PlayerChatBoxCanvas;
    public GameObject UIJudgeObj;
    public GameObject factsContainer;
    public GameObject toEndScreenButton;
    public GameObject backToMenuButton;
    public GameObject nextLevelButton;
    public GameObject checkLieButton;
    public GameObject judgeButtonPanel;
    public GameObject bubblesContainer;
    public GameObject toResultsButton;

    public GameObject heavenPortal;
    public GameObject hellvoid;

    public TMP_Text verdictResultText;

    public GameObject judgementResponseTextBox;
    public TMP_Text characterNameJudgement;
    public TMP_Text characterTextJudgement;

    public GameManager gameManager;

   Facts_Summary_Text_Sorter facts_Summary_Text_Sorter;
    HeartWeigher_LiesChecker heartWeigher_LiesChecker;

    public BubbleGenerator bubbleGenerator;



    private void Start()
    {
        heartWeigher_LiesChecker = GetComponent<HeartWeigher_LiesChecker>();
        facts_Summary_Text_Sorter = GetComponent<Facts_Summary_Text_Sorter>();
    }

    public void ReactionState(bool isHeaven)
    {
        judgementResponseTextBox.SetActive(true);
        UIJudgeObj.SetActive(false);
        factsContainer.SetActive(false);
        toResultsButton.SetActive(true);

        characterNameJudgement.text = GameManager.instance.currentCharacterJudged.characterName;
        
        if (isHeaven)
        {
            characterTextJudgement.text = GameManager.instance.currentCharacterJudged.heavenResponse;
            UIManager.instance.loadCharacterDisplayText.ChangeCurrentCharacterSprite(GameManager.instance.currentCharacterJudged.spriteHappy);
        }
        else
        {
            characterTextJudgement.text = GameManager.instance.currentCharacterJudged.hellResponse;
            UIManager.instance.loadCharacterDisplayText.ChangeCurrentCharacterSprite(GameManager.instance.currentCharacterJudged.spriteSad);
        }
    }

    public void ResetUIToDefaultState()
    {
        UIManager.instance.lieDetectorObject.SetActive(true);
        UIManager.instance.loadCharacterDisplayText.characterSR.enabled = true;
        FactsSummaryScalesObject.SetActive(true);
        PlayerChatBoxCanvas.SetActive(true);
        judgeButtonPanel.SetActive(true);
        bubblesContainer.SetActive(true);
        UIJudgeObj.SetActive(false);
        checkLieButton.SetActive(true);
        facts_Summary_Text_Sorter.ResetFactsSummaryScaleThing();
        judgementResponseTextBox.SetActive(false);
    }

    public void HideUIJudge()
    {
        UIManager.instance.lieDetectorObject.SetActive(false);
        LiesCheckScalesObject.GetComponentInChildren<SpawnTestWeights>().currentFeather.SetActive(false);
        // gets current feather

        FactsSummaryScalesObject.SetActive(false);
        checkLieButton.SetActive(false);
        PlayerChatBoxCanvas.SetActive(false);
        judgeButtonPanel.SetActive(false);
        bubblesContainer.SetActive(false);
        UIJudgeObj.SetActive(true);
      
    }

    public void ShowNextLevelButton()
    {
        UIJudgeObj.SetActive(false);
        nextLevelButton.SetActive(true);

        if (GameManager.instance.currentVerdict)
        {
            verdictResultText.text = "Good Verdict!";
        }
        else
        {
            verdictResultText.text = "Bad Verdict";
        }
    }

    public void Judge()
    {
        gameManager.Judge();
    }

    public void HeavenOrHellChoose(bool isHeaven)
    {
        gameManager.HeavenOrHellChoose(isHeaven);
        UIJudgeObj.SetActive(false);


        if (isHeaven)
            StartCoroutine( heartWeigher_LiesChecker.VignetteHeaven() );
        else
            StartCoroutine(heartWeigher_LiesChecker.VignetteHell());
    }

    // REPLACED NEXT LEVEL FUNCTION
    public void AdvanceToNextCharacter()
    {
        // Move on to next character

        // 1. Do animation (IEnumerator wait for animation time) to show character leaving off screen


        // 2. Populate next character to be judged
        // 4. Update all text to reflect info in new character?
        ResetUIToDefaultState();
        UIManager.instance.characterTextLongform.text = "";

        if (GameManager.instance.charactersJudged.Count == GameManager.instance.characters.Count)
        {
           
            GameManager.instance.GameOver();
            return;
        }

       
        //Characters now go in fixed order 0-list.count
        for (int i = 0; i < GameManager.instance.characters.Count; i++)
        {
            var character = GameManager.instance.characters[i];
            if (!GameManager.instance.charactersJudged.Contains(character))
            {
                GameManager.instance.currentCharacterJudged = character;
                break;
            }
        }

        GameManager.instance.charactersJudged.Add(GameManager.instance.currentCharacterJudged);

        GameManager.instance.loadCharacterDisplayText.LoadCharacterText(GameManager.instance.currentCharacterJudged);

        bubbleGenerator.GenerateBubblesOnStart();
        // 3. Reset state of all UI Elements
        



        UIManager.instance.inVerdictState = false;
        //
        // I don't know if im missing any steps, I am falling asleep at my desk right now.
        // If any questions are had, just send me a DM and I'll answer what I can when I
        // wake up.
        
        // I was missing this step... duh
        UIManager.instance.characterSpriteAnimator.SetBool("isJudging", false);
        UIManager.instance.bookAnimator.SetBool("isJudging", false);
        UIManager.instance.decisionAnimator.SetBool("isJudging", false);
        
        nextLevelButton.SetActive(false);

       
    }

    public void ResetVignette()
    {
        StartCoroutine(heartWeigher_LiesChecker.ResetVignetteLerpZeroWhite ());
    }
}
