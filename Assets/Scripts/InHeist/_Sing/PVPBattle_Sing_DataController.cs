using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

public class PVPBattle_Sing_DataController
{
    // :: Section : Singleton
    private static PVPBattle_Sing_DataController instance = null;
    public static PVPBattle_Sing_DataController Instance()
    {
        if (instance == null)
            instance = new PVPBattle_Sing_DataController();

        return instance;
    }

    // :: Section : Construction
    public PVPBattle_Sing_DataController()
    {
        this.LoadJSON_Character();
        this.LoadJSON_Skill();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: Section : Load All Data
    public Dictionary<int, Character_Data> DictCharacterData { get; private set; }
    private void LoadJSON_Character()
    {
        // :: for Use
        this.DictCharacterData = new Dictionary<int, Character_Data>();

        // :: Data
        string jsonCharacter = Resources.Load<TextAsset>("JSON/character_data").text;
        this.DictCharacterData = JsonConvert.DeserializeObject<Character_Data[]>(jsonCharacter).ToDictionary(ele => ele.model_type);
    }
    public Dictionary<int, Skill_Data> DictSkillData { get; private set; }
    private void LoadJSON_Skill()
    {
        // :: for Use
        this.DictSkillData = new Dictionary<int, Skill_Data>();

        // :: Data
        string jsonSkill = Resources.Load<TextAsset>("JSON/skill_data").text;
        this.DictSkillData = JsonConvert.DeserializeObject<Skill_Data[]>(jsonSkill).ToDictionary(ele => ele.id);
    }
}