using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LoadCharacterDisplayText : MonoBehaviour
{
    public List<TextMeshProUGUI> chat;
    public Character currentCharacter;
   

    HeartWeigher_LiesChecker heartWeigherLiesChecker;
    Facts_Summary_Text_Sorter factSorter;
    public Image characterSR;

    public int questionsRemaining;
    public int chatCallFrameDelay = 5;

    private void Awake()
    {
        heartWeigherLiesChecker = GetComponent<HeartWeigher_LiesChecker>();
        factSorter = GetComponent<Facts_Summary_Text_Sorter>();
    }

    void Start()
    {
        UpdateCurrentCharacter();
    }

    void UpdateCurrentCharacter()
    {
        currentCharacter = GameManager.instance.currentCharacterJudged;
    }

    void LoadCharacterText()
    {
        // first pos in the list is the front fact, there's no question
        if (UIManager.instance.characterTextLongform.text.Length <= 0)
        {
            UIManager.instance.characterTextLongform.text = currentCharacter.facts[0].frontFact;
        }
        else
        {
            UIManager.instance.characterTextLongform.text = currentCharacter.facts[0].frontFact;
        }

        UIManager.instance.characterNameLongform.text = currentCharacter.characterName;
        // SetAnswerTextRedOrGreen(0);


        for (int i = 1; i < chat.Count; i++)
        {
            if (chat[i] == null) return;

            //chat[i].text = currentCharacter.facts[i].question;
            CallTypeSentence(currentCharacter.facts[i].question, chatCallFrameDelay);
        }

        heartWeigherLiesChecker.currentFactInTheBigBox = currentCharacter.facts[0]; //obj ref for call lie button
    }

    public void LoadCharacterText(Character character)
    {
        currentCharacter = character;
        LoadCharacterText();
        factSorter.SortFrontFact(currentCharacter.facts[0], isFrontFact: true);
        ChangeCurrentCharacterSprite(currentCharacter.characterSprite);
    }

    public void ChangeCurrentCharacterSprite(Sprite sprite)
        
    {
        characterSR.sprite = sprite;
    }
    public void PressQuestion(int index)
    {
        //shows the answer to your question
        //chat[0].text = currentCharacter.facts[index].frontFact;
        CallTypeSentence(currentCharacter.facts[index].frontFact, chatCallFrameDelay); 

        ChangeCurrentCharacterSprite(currentCharacter.characterSprite); // sets it to normal again


        //puts this fact near the summarizing scales (green or red)
        heartWeigherLiesChecker.currentFactInTheBigBox = currentCharacter.facts[index];
        factSorter.SortFrontFact(currentCharacter.facts[index], isFrontFact: true);



        UIManager.instance.questionsRemaining -= 1;



    }

    public void DisplayStringInBigTextBox(string text)
    {
       // chat[0].text = text;
        CallTypeSentence(text, chatCallFrameDelay);
    }

    // void SetAnswerTextRedOrGreen(int factIndex)
    // {
    //     if (currentCharacter.facts[factIndex].frontWeight >= 0)
    //     {
    //         chat[0].color = Color.green;
    //     }
    //     else
    //     {
    //         chat[0].color = Color.red;
    //     }
    // }

    public string SetAnswerTextRedOrGreen(int factIndex)
    {
        if (currentCharacter.facts[factIndex].frontWeight >= 0)
        {
            return "<color=green>";
        }
        else
        {
            return "<color=red>";
        }
    }

    public void CallTypeSentence(string sententence, int callFrameDelay)
    {
        StopAllCoroutines(); // stops any running typesentences
        StartCoroutine(TypeSentence(sententence, callFrameDelay));
    }
    IEnumerator TypeSentence(string sentence, int callFrameDelay)
    {
        //add speaking sound effect here?
        chat[0].text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            chat[0].text += letter;
            for (int i = 0; i < callFrameDelay; i++)
            {
                yield return null;
            }
        }

    }
}



