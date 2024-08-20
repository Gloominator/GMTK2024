using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;


public class HeartWeigher_LiesChecker : MonoBehaviour
{
    Facts_Summary_Text_Sorter facts_Summary_Text_Sorter;
    public GameObject checkLiesButton;
    public SpawnTestWeights heartWeigherScalesSpawner;
    [SerializeField] bool previousHeartDespawned = true;
    [SerializeField] float howLongWaitForNewHeartSpawn = 4;
    public Fact currentFactInTheBigBox;
    [SerializeField] LoadCharacterDisplayText loadCharacterDisplayText;

    private Dictionary<SpriteRenderer, int> originalOrderInLayer = new(); // fixing a stupid bug as I can

    public List<Fact> factsChecked = new List<Fact>();

    public int feathersForGuessRight = 0;

    public GameObject liesScalesParent;
    public Volume postProcessingVolume; // Reference to the Volume component
    Vignette vignette; // Reference to the Vignette effect
    Bloom bloom;
    public float checkFactHowLongVignetteMovesInward = 2;



    void Start()
    {
        facts_Summary_Text_Sorter = GetComponent<Facts_Summary_Text_Sorter>();

        if (postProcessingVolume.profile.TryGet<Vignette>(out var vignetteEffect))
        {
            vignette = vignetteEffect;
        }
        //get bloom component
        if (postProcessingVolume.profile.TryGet<Bloom>(out var bloom))
        {
            this.bloom = bloom;
        }

        HideLiesScales();
    }

  

    //checks if the player has been lied to
    public void CheckLies()
    {
        Debug.Log("Checking for lies");

        if (facts_Summary_Text_Sorter.HasLiedToThePlayer())
        {
            Debug.Log("The player has been lied to");
            checkLiesButton.GetComponent<TextMeshProUGUI>().text = "They have lied to you";
            checkLiesButton.GetComponent<TextMeshProUGUI>().color = Color.red;
            //heart is heavier than feather
            heartWeigherScalesSpawner.SpawnHeartLeft(isHeavy: true);
        }
        else
        {
            Debug.Log("The player has not been lied to");
            checkLiesButton.GetComponent<TextMeshProUGUI>().text = "They have been honest";
            checkLiesButton.GetComponent<TextMeshProUGUI>().color = Color.green;
            //heart is the same as feather
            heartWeigherScalesSpawner.SpawnHeartLeft(isHeavy: false);
        }
        checkLiesButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }


    public void CheckThisFactDisplayFront(TextMeshProUGUI tmp)
    {

        //send its front big text box fact to big text box
        currentFactInTheBigBox = facts_Summary_Text_Sorter.currentFactAndTmpPairs.FirstOrDefault(x => x.Value == tmp).Key;
        loadCharacterDisplayText.DisplayStringInBigTextBox(currentFactInTheBigBox.frontFact);
    }

    //connects to "press x to doubt" button
    public void CheckThisFactButton()
    {
        if (previousHeartDespawned)
        {
            if (factsChecked.Contains(currentFactInTheBigBox))
            {
                Debug.Log("You have already checked this fact");
                return;
            }

            StartCoroutine(CheckThisFactWithDelay(howLongWaitForNewHeartSpawn, currentFactInTheBigBox));
            factsChecked.Add(currentFactInTheBigBox);
        }
        else
        {
            Debug.Log("You have to wait for the heart to despawn");
        }
       
    }
    
   
    public void CheckThisFactTMP(TextMeshProUGUI tmp)
    {
        if (previousHeartDespawned)
        {
            CheckThisFact(facts_Summary_Text_Sorter.currentFactAndTmpPairs.FirstOrDefault(x => x.Value == tmp).Key);
        }
        else
        {
            Debug.Log("You have to wait for the heart to despawn");
        }
    }

    void CheckThisFact(Fact fact)
    {
        facts_Summary_Text_Sorter.currentFactAndTmpPairs[fact].GetComponent<Button>().interactable = false;
        if (fact.isLie)
        {
            // player gets a bonus feather for finding a lie
            GameManager.instance.feathersOfTruth += feathersForGuessRight;
            UIManager.instance.UpdateFeathersOfTruthText(1); //just a visualiser that you got right and "got something" (test)

            //find this fact in the original list, make it different color, strike it through
            //remove its weight from the scales

            facts_Summary_Text_Sorter.currentFactAndTmpPairs[fact].color = Color.black;
            facts_Summary_Text_Sorter.currentFactAndTmpPairs[fact].fontStyle = FontStyles.Strikethrough;
           
           
            facts_Summary_Text_Sorter.factWeightObjectPairs[fact].SetActive(false);

            //remove associated valuepairs
            facts_Summary_Text_Sorter.currentFactAndTmpPairs.Remove(fact);
            facts_Summary_Text_Sorter.factWeightObjectPairs.Remove(fact);
            heartWeigherScalesSpawner.SpawnHeartLeft(isHeavy: true);
            // spawn the true fact, spawn its weight
            facts_Summary_Text_Sorter.SortFrontFact(fact, isFrontFact: false);

            loadCharacterDisplayText.DisplayStringInBigTextBox(fact.reactionBeingCaught);
            loadCharacterDisplayText.ChangeCurrentCharacterSprite(loadCharacterDisplayText.currentCharacter.spriteSad);

            
            //change sprite to sad
        }
        else
        {
            //find this fact in the original list, make it different color
            // player loses a feather for questioning a truth
             GameManager.instance.feathersOfTruth -= 1;
             UIManager.instance.UpdateFeathersOfTruthText(-1);
            facts_Summary_Text_Sorter.currentFactAndTmpPairs[fact].color = Color.green;
            //increase its size 1.2x
            //facts_Summary_Text_Sorter.currentFactAndTmpPairs[fact].fontSize *= 1.2f;
            heartWeigherScalesSpawner.SpawnHeartLeft(isHeavy: false);

            loadCharacterDisplayText.DisplayStringInBigTextBox(fact.reactionCalledLieWrong);
            // change sprite to happy?
            loadCharacterDisplayText.ChangeCurrentCharacterSprite(loadCharacterDisplayText.currentCharacter.spriteHappy);
            
        }
        StartCoroutine(CanSpawnNextHeartCounter());
        StartCoroutine(HideLiesScalesAndMoveVignetteOutward(howLongWaitForNewHeartSpawn, fact.isLie));
    }

    IEnumerator CanSpawnNextHeartCounter()
    {
        previousHeartDespawned = false;
        yield return new WaitForSeconds(howLongWaitForNewHeartSpawn);
        previousHeartDespawned = true;
    }

    //makes vignette black and moves it inward onto scales
    

    public IEnumerator CheckThisFactWithDelay(float delay, Fact fact)
    {
        StartCoroutine(ShowLiesScalesAndMoveVignetteInward(checkFactHowLongVignetteMovesInward));
        yield return new WaitForSeconds(delay);
        CheckThisFact(fact);
    }


    // fixing a stupid bug
    public void HideLiesScales()
    {
        SpriteRenderer[] spriteRenderers = liesScalesParent. GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            // Save the original order in layer
            if (!originalOrderInLayer.ContainsKey(sr))
            {
                originalOrderInLayer[sr] = sr.sortingOrder;
            }
            // Set the order in layer to -2
            sr.sortingOrder = -2;
        }
    }

   
    public void UnhideLiesScales()
    {
        foreach (KeyValuePair<SpriteRenderer, int> entry in originalOrderInLayer)
        {
            if (entry.Key != null)
            {
                entry.Key.sortingOrder = entry.Value;
            }
        }
        
        originalOrderInLayer.Clear();
    }



    #region vignette

    public IEnumerator ShowLiesScalesAndMoveVignetteInward(float time)
    {


        float currentIntensity = vignette.intensity.value;
        vignette.color.value = Color.black;
        float elapsedTime = 0;


        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, 1, elapsedTime / time);
            yield return null;
        }
        UnhideLiesScales();
    }

    public IEnumerator HideLiesScalesAndMoveVignetteOutward(float time, bool isLie)
    {
        if (isLie)
            vignette.color.value = Color.red;
        else
            vignette.color.value = Color.green;

        float currentIntensity = vignette.intensity.value;
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, 0, elapsedTime / time);
            yield return null;
        }


        yield return new WaitForSeconds(1);
        HideLiesScales();
        vignette.color.value = Color.white;
    }

    public IEnumerator VignetteHeaven()
    {
        //lerp bloom threshhold to 1.33 and intensity to 469.66, lerp vignette intensity to 0.273
        // set vignette color to FFF5a7, set bloom tint to FFFBAC
        vignette.color.value = new Color(1, 0.961f, 0.675f);
        bloom.tint.value = new Color(1, 0.984f, 0.675f);
        float currentIntensity = vignette.intensity.value;
        float currentBloomIntensity = bloom.intensity.value;
        float currentBloomThreshhold = bloom.threshold.value;

        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, 0.473f, elapsedTime);
            bloom.intensity.value = Mathf.Lerp(currentBloomIntensity, 469.66f, elapsedTime);
            bloom.threshold.value = Mathf.Lerp(currentBloomThreshhold, 1.33f, elapsedTime );
            yield return null;
        }


    }

   


    public IEnumerator VignetteHell()
    {
        //set vignette color to hexadecimal F1001F
        vignette.color.value = new Color(0.945f, 0, 0.121f);
        
        float currentIntensity = vignette.intensity.value;
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, 0.214f, elapsedTime );
            yield return null;
        }
    }    

    public IEnumerator ResetVignetteLerpZeroWhite()
    {
        //lerp intensity from current to 0
        float currentIntensity = vignette.intensity.value;
       
        float elapsedTime = 0;
                
            float currentBloomIntensity = bloom.intensity.value;
            float currentBloomThreshhold = bloom.threshold.value;
        
        while (elapsedTime < 1)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(currentIntensity, 0, elapsedTime );
            if (bloom.IsActive())
            {
            bloom.intensity.value = Mathf.Lerp(currentBloomIntensity, 0, elapsedTime );
            bloom.threshold.value = Mathf.Lerp(currentBloomThreshhold, 0, elapsedTime );
            }
            yield return null;
        }
        vignette.color.value = Color.white;
        
        bloom.intensity.value = 0;
    }
    #endregion
}
