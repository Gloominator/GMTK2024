using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Fact", menuName = "Fact")]
public class Fact : ScriptableObject
{
    public string question;
    public string frontFact;
    public bool isLie;
    public string frontFactForSummaryScales;
    public string actualFactForSummaryScales;
    public string reactionBeingCaught;
    public string reactionCalledLieWrong;
    public int frontWeight;
    public int actualWeight;


}
