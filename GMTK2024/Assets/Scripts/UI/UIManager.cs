using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public TMP_Text characterText;
    public TMP_Text characterName;
    public TMP_Text characterTextLongform;
    public TMP_Text characterNameLongform;
    public TMP_Text feathersOfTruthRemainingText;
    private Vector3 feathersOfTruthAnimationTextOriginalPosition;
    public TMP_Text badVerdictsText;
    public LoadCharacterDisplayText loadCharacterDisplayText;

    public AllUIRefs allUIRefs;

    public Button checkLieButton;

    public TMP_Text feathersOfTruthAnimationText;
    public Image feathersOfTruthAnimationImage;
    private DG.Tweening.Sequence animationSequence;
    private float feathersOfTruthAnimationImageOriginalAlpha;
    private float feathersOfTruthAnimationTextOriginalAlpha;

    public GameObject endGamePanel;
    public TMP_Text endGame_rankText;
    public TMP_Text endGame_correctSoulsText;
    public TMP_Text endGame_evilSoulsIntoHeavenText;
    public TMP_Text endGame_innoncentSoulsIntoHellText;
    public TMP_Text endGame_feathersRemainingText;
    public TMP_Text endGame_correctSoulsText_value;
    public TMP_Text endGame_evilSoulsIntoHeavenText_value;
    public TMP_Text endGame_innoncentSoulsIntoHellText_value;
    public TMP_Text endGame_feathersRemainingText_value;

    public GameObject lieDetectorObject;

    public bool inVerdictState;

    public int questionsRemaining;

    public Animator characterSpriteAnimator;
    public Animator bookAnimator;
    public Animator decisionAnimator;

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
        UpdateFeathersOfTruthText(0);

        feathersOfTruthAnimationTextOriginalPosition = feathersOfTruthAnimationText.transform.localPosition;
        feathersOfTruthAnimationImageOriginalAlpha = feathersOfTruthAnimationImage.color.a;
        feathersOfTruthAnimationTextOriginalAlpha = feathersOfTruthAnimationText.color.a;

    }
    private void Update()
    {
        DisplayLieChecker();
    }

    public void PlayJudgementAnimations()
    {
        bookAnimator.SetBool("isJudging", true);
        characterSpriteAnimator.SetBool("isJudging", true);
        decisionAnimator.SetBool("isJudging", true);

    }

    private void DisplayLieChecker()
    {
        if (questionsRemaining <= 0 && !inVerdictState)
        {
            lieDetectorObject.SetActive(true);
        }
    }

    public void UpdateEndGameTexts()
    {
        endGamePanel.SetActive(true);

        endGame_rankText.text = GameManager.instance.rank;
        // endGame_correctSoulsText.text = "Souls judged correctly: " + GameManager.instance.correctChoices.ToString();
        // endGame_evilSoulsIntoHeavenText.text = string.Format("You let {0} evil soul into heaven", GameManager.instance.evilSoulsSentToHeaven.ToString());
        // endGame_innoncentSoulsIntoHellText.text = string.Format("You condemned {0} innocent soul to hell", GameManager.instance.innocentSoulsSentToHell.ToString());
        endGame_feathersRemainingText.text = ": " + GameManager.instance.feathersOfTruth.ToString();

        endGame_correctSoulsText_value.text = "+" + (GameManager.instance.correctChoices * 100).ToString();
        endGame_evilSoulsIntoHeavenText_value.text = "-" + (GameManager.instance.evilSoulsSentToHeaven * 100).ToString();
        endGame_innoncentSoulsIntoHellText_value.text = "-" + (GameManager.instance.innocentSoulsSentToHell * 100).ToString();
        endGame_feathersRemainingText_value.text = "+" + (GameManager.instance.feathersOfTruth * 100).ToString();

        if (GameManager.instance.correctChoices == 0)
        {
            endGame_correctSoulsText_value.text = "0";
            endGame_correctSoulsText_value.color = Color.black;
        }
        if (GameManager.instance.evilSoulsSentToHeaven == 0)
        {
            endGame_evilSoulsIntoHeavenText_value.text = "0";
            endGame_evilSoulsIntoHeavenText_value.color = Color.black;
        }
        if (GameManager.instance.innocentSoulsSentToHell == 0)
        {
            endGame_innoncentSoulsIntoHellText_value.text = "0";
            endGame_innoncentSoulsIntoHellText_value.color = Color.black;
        }
        if (GameManager.instance.feathersOfTruth == 0)
        {
            endGame_feathersRemainingText_value.text = "0";
            endGame_feathersRemainingText_value.color = Color.black;
        }
    }

    public void UpdateFeathersOfTruthText(int change)
    {
        feathersOfTruthRemainingText.text = ": " + GameManager.instance.feathersOfTruth;

        if (change != 0) AnimateFeathersOfTruthText(change);
    }

    public void ShowStageCompleteMenu()
    {
        GameManager.instance.ShowStageCompleteMenu();


        allUIRefs.ResetVignette();
    }

    public void AnimateFeathersOfTruthText(int change)
    {
        //TODO: animate the text, green and +x if positive, red and -x if negative
        feathersOfTruthAnimationText.text = ": " + change.ToString();

        //change the color of the text
        if (change > 0)
        {
            feathersOfTruthAnimationText.color = Color.green;
        }
        else
        {
            feathersOfTruthAnimationText.color = Color.red;
        }

        PlayFeathersOfTruthAnimation();
    }

    public void PlayFeathersOfTruthAnimation()
    {
        // Kill any existing sequence with the same ID to restart it
        DOTween.Kill("FeathersOfTruthAnimation");

        // Reset text and image to initial states
        ResetTextAndImage();

        // Create a new sequence
        animationSequence = DOTween.Sequence();
        animationSequence.SetId("FeathersOfTruthAnimation");

        // set the text and image alpha to 1
        feathersOfTruthAnimationText.color = new Color(feathersOfTruthAnimationText.color.r, feathersOfTruthAnimationText.color.g, feathersOfTruthAnimationText.color.b, 1f);
        feathersOfTruthAnimationImage.color = new Color(feathersOfTruthAnimationImage.color.r, feathersOfTruthAnimationImage.color.g, feathersOfTruthAnimationImage.color.b, 1f);

        // Animate the text moving up
        animationSequence.Append(feathersOfTruthAnimationText.transform.DOLocalMoveY(feathersOfTruthAnimationTextOriginalPosition.y + 80, 0.5f));

        // Fade the text and image out
        animationSequence.Append(feathersOfTruthAnimationText.DOFade(0f, 0.5f));
        animationSequence.Join(feathersOfTruthAnimationImage.DOFade(0f, 0.5f));

        // OnComplete, reset the values
        animationSequence.OnComplete(() =>
        {
            ResetTextAndImage();
        });

        // Play the sequence
        animationSequence.Play();
    }

    private void ResetTextAndImage()
    {
        // Reset the text and image alphas
        feathersOfTruthAnimationText.color = new Color(feathersOfTruthAnimationText.color.r, feathersOfTruthAnimationText.color.g, feathersOfTruthAnimationText.color.b, feathersOfTruthAnimationTextOriginalAlpha);
        feathersOfTruthAnimationImage.color = new Color(feathersOfTruthAnimationImage.color.r, feathersOfTruthAnimationImage.color.g, feathersOfTruthAnimationImage.color.b, feathersOfTruthAnimationImageOriginalAlpha);

        // Reset the text position
        feathersOfTruthAnimationText.transform.localPosition = feathersOfTruthAnimationTextOriginalPosition;
    }

    public void UpdateVerdictsText()
    {
        badVerdictsText.text = "Bad Verdicts: " + (GameManager.instance.innocentSoulsSentToHell + GameManager.instance.evilSoulsSentToHeaven).ToString();
    }

    public void UpdateCharacterText()
    {

    }
}
