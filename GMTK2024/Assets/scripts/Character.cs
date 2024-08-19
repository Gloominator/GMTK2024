using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class Character : ScriptableObject
{

    public List<Fact> facts; // 0 is opening fact?
   [SerializeField] int summedFrontKarma;
    [SerializeField] int summedActualKarma;
    public string characterName;
    public Sprite characterSprite;
    public Sprite spriteSad;
    public Sprite spriteHappy;
    public Sprite spriteJudgement;
    [HideInInspector] public int karma;

    public bool shouldGoToHeaven;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Fact fact in facts)
        {
            karma += fact.actualWeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
