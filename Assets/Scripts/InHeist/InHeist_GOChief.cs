using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_GOChief : MonoBehaviour
{
    // : Holder
    private InHeist_GOHolder GOHolder;

    // : Init
    public void Init()
    {
        // :: Holder
        this.GOHolder = this.GetComponent<InHeist_GOHolder>();
        this.GOHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Get
    public GameObject GetCharacterFromPool()
    {
        // :: Set
        var go = this.GOHolder.TRANSFORM_poolCharacters.transform.GetChild(0).gameObject;
        go.transform.SetParent(null);

        // :: Return
        return go;
    }
    public Transform GetCharacterPool()
    {
        return this.GOHolder.TRANSFORM_poolCharacters.transform;
    }
    public Transform GetBeamPool()
    {
        return this.GOHolder.TRANSFORM_poolBeam.transform;
    }
    public Transform GetMissilePool()
    {
        return this.GOHolder.TRANSFORM_poolMissile.transform;
    }
    public Transform GetField()
    {
        return this.GOHolder.TRANSFORM_fieldCharacters.transform;
    }
    public InHeist_Leader_Character GetCharacterInField(string guid)
    {
        foreach(Transform child in this.GetField())
        {
            var character = child.GetComponent<InHeist_Leader_Character>();
            if (character != null)
                if (character.GetGUID() == guid)
                    return character;
        }

        // :: Error
        return null;
    }

    // : Set
    public void SetCharacterInField(GameObject character)
    {
        // :: Set
        character.transform.SetParent(this.GOHolder.TRANSFORM_fieldCharacters.transform);
    }

    // : Show
    public void ShowShowcase(EnumAll.eTeam eTeam)
    {
        this.GOHolder.TRANSFORM_showcaseBlue.SetActive(false);
        this.GOHolder.TRANSFORM_showcaseRed.SetActive(false);
        switch (eTeam)
        {
            case EnumAll.eTeam.RED:
                this.GOHolder.TRANSFORM_showcaseRed.SetActive(true);
                break;
            case EnumAll.eTeam.BLUE:
                this.GOHolder.TRANSFORM_showcaseBlue.SetActive(true);
                break;
        }
    }
}
