using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public TMP_Text characterText;
    public TMP_Text characterName;
    public TMP_Text turnsRemainingText;
    public TMP_Text badVerdictsText;
    public LoadCharacterDisplayText loadCharacterDisplayText;

    public GameObject lieDetectorObject;

    public bool inVerdictState;

    public int questionsRemaining;
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }             
    }

    private void Start()
    {
        UpdateTurnsText();
    }
    private void Update()
    {
        DisplayLieChecker();
    }

    private void DisplayLieChecker()
    {
        if (questionsRemaining <= 0 && !inVerdictState)
        {
            lieDetectorObject.SetActive(true);
        }
    }

    public void UpdateTurnsText()
    {
        turnsRemainingText.text = "Turns Remaining: " + GameManager.instance.turns;
        
    }

    public void UpdateVerdictsText()
    {
        badVerdictsText.text = "Bad Verdicts: " + GameManager.instance.incorrectChoices;
    }

    public void UpdateCharacterText()
    {
        
    }
}
