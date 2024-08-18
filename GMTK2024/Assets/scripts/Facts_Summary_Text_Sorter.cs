using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Facts_Summary_Text_Sorter : MonoBehaviour
{
    public List<TextMeshProUGUI> goodFactsTMP;
    public List<TextMeshProUGUI> badFactsTMP;
    public List<Fact> currentFactsInGoodSlots;
    public List<Fact> currentFactsInBadSlots;
    public List<Fact> currentFactsAlreadyGivenToThePlayer;
    public LoadCharacterDisplayText loadCharacterDisplayText;
    public SpawnTestWeights scalesSpawnerFactsSummary;
    public Dictionary<Fact, TextMeshProUGUI> currentFactAndTmpPairs = new(); // keeps track of which fact is in which slot
    public Dictionary<Fact, GameObject> factWeightObjectPairs = new(); // keeps track what weights - facts are on the scales

    void Start()
    {
        loadCharacterDisplayText = GetComponent<LoadCharacterDisplayText>();
    }

    public void SortFrontFact(Fact fact, bool isFrontFact)
    {
        if (isFrontFact)
        {
            if (fact.frontWeight >= 0)
            {
                int slotNum = WhichTextSlotIsVacant(isGoodSlot: true);
                currentFactsInGoodSlots.Add(fact);
                goodFactsTMP[slotNum].text = fact.frontFactForSummaryScales;
                goodFactsTMP[slotNum].gameObject.SetActive(true);

                currentFactAndTmpPairs.Add(fact, goodFactsTMP[slotNum]);

                factWeightObjectPairs.Add(fact, scalesSpawnerFactsSummary.SpawnObjRight(Mathf.Abs(fact.frontWeight)));
            }
            else
            {
                int slotNum = WhichTextSlotIsVacant(isGoodSlot: false);
                currentFactsInBadSlots.Add(fact);
                badFactsTMP[slotNum].text = fact.frontFactForSummaryScales;
                badFactsTMP[slotNum].gameObject.SetActive(true);

                currentFactAndTmpPairs.Add(fact, badFactsTMP[slotNum]);

                factWeightObjectPairs.Add(fact, scalesSpawnerFactsSummary.SpawnObjLeft(Mathf.Abs(fact.frontWeight)));
            }
        }
        else if (!isFrontFact)
        {
            if (fact.actualWeight >= 0)
            {
                int slotNum = WhichTextSlotIsVacant(isGoodSlot: true);
                currentFactsInGoodSlots.Add(fact);
                goodFactsTMP[slotNum].text = fact.actualFactForSummaryScales;
                goodFactsTMP[slotNum].gameObject.SetActive(true);

                currentFactAndTmpPairs.Add(fact, goodFactsTMP[slotNum]);

                factWeightObjectPairs.Add(fact, scalesSpawnerFactsSummary.SpawnObjRight(Mathf.Abs(fact.actualWeight)));
            }
            else
            {
                int slotNum = WhichTextSlotIsVacant(isGoodSlot: false);
                currentFactsInBadSlots.Add(fact);
                badFactsTMP[slotNum].text = fact.actualFactForSummaryScales;
                badFactsTMP[slotNum].gameObject.SetActive(true);

                currentFactAndTmpPairs.Add(fact, badFactsTMP[slotNum]);

                factWeightObjectPairs.Add(fact, scalesSpawnerFactsSummary.SpawnObjLeft(Mathf.Abs(fact.actualWeight)));
            }
            currentFactAndTmpPairs[fact].GetComponent<Button>().interactable = false;
        }
        currentFactsAlreadyGivenToThePlayer.Add(fact);

    }



    public bool HasLiedToThePlayer()
    {

        foreach (Fact factGiven in currentFactsAlreadyGivenToThePlayer)
        {
            if (factGiven.isLie)
                return true;
            else
                return false;

        }
        return false;
    }




    public void ShowTrueFacts()
    {
        Character character = loadCharacterDisplayText.currentCharacter;
        DisableGoodAndBadTMPs();
        DisableButtonInteractabilityTMPs();

        foreach (GameObject obj in factWeightObjectPairs.Values)
        {
            obj.SetActive(false);
            Destroy(obj, 2);
        }
        factWeightObjectPairs.Clear();


        for (int i = 0; i < character.facts.Count; i++)
        {
            if (character.facts[i].actualWeight >= 0)
            {
                goodFactsTMP[WhichTextSlotIsVacant(true)].text = character.facts[i].actualFactForSummaryScales;
                goodFactsTMP[WhichTextSlotIsVacant(true)].gameObject.SetActive(true);

                //spawn a weight too to show the true balance
                factWeightObjectPairs.Add(character.facts[i], scalesSpawnerFactsSummary.SpawnObjRight(Mathf.Abs(character.facts[i].actualWeight)));
            }

        }

        for (int i = 0; i < character.facts.Count; i++)
        {
            if (character.facts[i].actualWeight < 0)
            {
                badFactsTMP[WhichTextSlotIsVacant(false)].text = character.facts[i].actualFactForSummaryScales;
                badFactsTMP[WhichTextSlotIsVacant(false)].gameObject.SetActive(true);

                //spawn a weight too to show the true balance
                factWeightObjectPairs.Add(character.facts[i], scalesSpawnerFactsSummary.SpawnObjLeft(Mathf.Abs(character.facts[i].actualWeight)));
            }

        }



    }

    void DisableGoodAndBadTMPs()
    {
        foreach (TextMeshProUGUI tmp in goodFactsTMP)
        {
            tmp.gameObject.SetActive(false);
        }
        foreach (TextMeshProUGUI tmp in badFactsTMP)
        {
            tmp.gameObject.SetActive(false);
        }

    }



    public TextMeshProUGUI FindTMPForFalseFact()
    {
        TextMeshProUGUI tmp = null;
        tmp = badFactsTMP[WhichTextSlotIsVacant(false)];
        return tmp;
    }

    public void StrikeThroughFactInTheList(Fact fact)
    {
        TextMeshProUGUI tmp = currentFactAndTmpPairs[fact];

        tmp.color = Color.black;
        tmp.fontStyle = FontStyles.Strikethrough;

        //remove its associated weight from the scales
    }


    // returns the index of the first empty slot in the good or bad list
    int WhichTextSlotIsVacant(bool isGoodSlot)
    {

        int slot = 0;

        if (isGoodSlot)
        {
            for (int i = 0; i < goodFactsTMP.Count; i++)
            {
                if (goodFactsTMP[i].gameObject.activeSelf == false)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < badFactsTMP.Count; i++)
            {
                if (badFactsTMP[i].gameObject.activeSelf == false)
                {
                    return i;
                }
            }
        }
        return slot;

    }


    void DisableButtonInteractabilityTMPs()
    {
        for (int i = 0; i < goodFactsTMP.Count; i++)
        {
            goodFactsTMP[i].GetComponent<Button>().interactable = false;
            badFactsTMP[i].GetComponent<Button>().interactable = false;
        }
    }

    public void ResetFactsSummaryScaleThing()
    {
        DisableGoodAndBadTMPs();

        //get objects from the scales and disable them
        foreach (GameObject obj in factWeightObjectPairs.Values)
        {
            obj.SetActive(false);
            Destroy(obj, 2);
        }


        for (int i = 0; i < goodFactsTMP.Count; i++)
        {
            goodFactsTMP[i].GetComponent<Button>().interactable = true;
            badFactsTMP[i].GetComponent<Button>().interactable = true;
        }

        factWeightObjectPairs.Clear();

        currentFactAndTmpPairs.Clear();
        currentFactsInGoodSlots.Clear();
        currentFactsInBadSlots.Clear();
        currentFactsAlreadyGivenToThePlayer.Clear();

    }
}
