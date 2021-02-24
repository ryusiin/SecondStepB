using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Lobby_GOChief : MonoBehaviour
{
    [Header("Characters")]
    [SerializeField]
    private GameObject character_Amy;
    [SerializeField]
    private GameObject character_Rajesh;
    [SerializeField]
    private GameObject character_Arsene;

    // : Init
    public void Init()
    {
        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Show
    public void ShowCharacter_All(bool check)
    {
        this.character_Amy.SetActive(check);
        this.character_Arsene.SetActive(check);
        this.character_Rajesh.SetActive(check);
    }
    public void ShowCharacter(EnumAll.eCharacter eCharacter)
    {
        this.GetCharacter(eCharacter).SetActive(true);
    }

    // : Get
    public GameObject GetCharacter(EnumAll.eCharacter eChracter)
    {
        if (eChracter == EnumAll.eCharacter.AMY)
            return this.character_Amy;
        else if (eChracter == EnumAll.eCharacter.ARSENE)
            return this.character_Arsene;
        else if (eChracter == EnumAll.eCharacter.RAJESH)
            return this.character_Rajesh;

        return null;
    }
}
