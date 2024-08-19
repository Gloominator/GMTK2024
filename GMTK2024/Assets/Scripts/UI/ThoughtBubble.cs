using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThoughtBubble : MonoBehaviour
{
    public string question;
    public int index;
    
    public TMP_Text questionText;
    
    private LoadCharacterDisplayText _loadCharacterDisplayText;

    private void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        _loadCharacterDisplayText = UIManager.instance.loadCharacterDisplayText;
        questionText = GetComponentInChildren<TMP_Text>();
        UpdateText();
    }
    
    void TaskOnClick()
    {
        _loadCharacterDisplayText.PressQuestion(index);
        gameObject.SetActive(false);
    }

    public void UpdateText()
    {
        questionText.text = question;
    }

}
