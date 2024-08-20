using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public List<Character> characters;
    public LoadCharacterDisplayText loadCharacterDisplayText;
    public Character currentCharacterJudged;
    public List<Character> charactersJudged;
    public AllUIRefs allUIRefs;

    public bool currentVerdict;

    public int feathersOfTruth;
    public int correctChoices;
    public int evilSoulsSentToHeaven;
    public int innocentSoulsSentToHell;
    public string rank;
    public BubbleGenerator bubbleGenerator;
    public static GameManager instance;
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        // if (feathersOfTruth <= 0 || innocentSoulsSentToHell; < -3)
        // {
        //     GameOver();
        // }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    //onloadscene
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadCharacterDisplayText = FindObjectOfType<LoadCharacterDisplayText>();
        allUIRefs = FindObjectOfType<AllUIRefs>();

        currentCharacterJudged = characters[0];
        charactersJudged.Add(currentCharacterJudged);
        loadCharacterDisplayText.LoadCharacterText(currentCharacterJudged);
    }

    public void Judge()
    {
        allUIRefs.HideUIJudge();
        UIManager.instance.inVerdictState = true;
        UIManager.instance.PlayJudgementAnimations();
    }

    public void GameOver()
    {
        //hide all the other game UI
        allUIRefs.FactsSummaryScalesObject.SetActive(false);
        allUIRefs.LiesCheckScalesObject.SetActive(false);
        allUIRefs.PlayerChatBoxCanvas.SetActive(false);
        allUIRefs.UIJudgeObj.SetActive(false);
        allUIRefs.factsContainer.SetActive(false);
        allUIRefs.nextLevelButton.SetActive(false);
        allUIRefs.checkLieButton.SetActive(false);
        allUIRefs.judgeButtonPanel.SetActive(false);
        allUIRefs.bubblesContainer.SetActive(false);
        allUIRefs.toResultsButton.SetActive(false);
        UIManager.instance.feathersOfTruthRemainingText.gameObject.SetActive(false);
        UIManager.instance.badVerdictsText.gameObject.SetActive(false);

        // TODO: IMPLEMENT ACTUAL GAME OVER LOGIC
        GetRank();
        UIManager.instance.UpdateEndGameTexts();

        Debug.Log("Game Over");
    }

    public void GetRank()
    {
        int score = 0;

        Debug.Log("Get Rank");
        Debug.Log("feathersOfTruth: " + feathersOfTruth);
        Debug.Log("correctChoices: " + correctChoices);
        Debug.Log("innocentSoulsSentToHell: " + innocentSoulsSentToHell);
        Debug.Log("evilSoulsSentToHeaven: " + evilSoulsSentToHeaven);

        score += feathersOfTruth * 100;
        score += correctChoices * 100;
        score -= innocentSoulsSentToHell * 100;
        score -= evilSoulsSentToHeaven * 100;

        Debug.Log("Score: " + score);


        if (score >= 800)
        {
            rank = "The Almighty";
        }
        else if (score >= 700)
        {
            rank = "Master of the Gate";
        }
        else if (score >= 600)
        {
            rank = "Wise Angel";
        }
        else if (score >= 500)
        {
            rank = "Soul Keeper";
        }
        else if (score >= 400)
        {
            rank = "Drowzy Night Watcher";
        }
        else if (score >= 300)
        {
            rank = "Corruptible Judge";
        }
        else if (score >= 200)
        {
            rank = "Devil's Advocate";
        }
        else
        {
            rank = "Forsaken";
        }
    }

    public void HeavenOrHellChoose(bool isHeaven)
    {

        if (isHeaven && currentCharacterJudged.shouldGoToHeaven)
        {
            correctChoices += 1;
            currentVerdict = true;
        }
        else if (!isHeaven && currentCharacterJudged.shouldGoToHeaven)
        {
            innocentSoulsSentToHell += 1;
            currentVerdict = false;
        }
        else if (isHeaven && !currentCharacterJudged.shouldGoToHeaven)
        {
            evilSoulsSentToHeaven += 1;
            currentVerdict = false;
        }
        else if (!isHeaven && !currentCharacterJudged.shouldGoToHeaven)
        {
            correctChoices += 1;
            currentVerdict = true;
        }

        allUIRefs.ReactionState(isHeaven);
    }

    public void ShowStageCompleteMenu()
    {
        loadCharacterDisplayText.characterSR.enabled = false;
        allUIRefs.judgementResponseTextBox.SetActive(false);
        allUIRefs.factsContainer.SetActive(true);
        UIManager.instance.UpdateVerdictsText();
        ShowNextLevelButton();

        FindObjectOfType<Facts_Summary_Text_Sorter>().ShowTrueFacts();
        bubbleGenerator.GenerateBubblesOnStart();
    }


    public void ShowNextLevelButton()
    {
        allUIRefs.ShowNextLevelButton();

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
