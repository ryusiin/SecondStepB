using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Component_CharacterSkillController : MonoBehaviour
{
    // :: for Use
    private Skill_Data skill_data;
    private InHeist_Leader_Character LEADERCharacter;

    // :: LAYER MASK
    const int LAYER_MASK_CHAMPION = 1 << 9; // :: Champion

    // :: Init
    public void Init(EnumAll.eSkill eSkill, InHeist_Leader_Character LEADERCharacter)
    {
        // :: First Set
        this.skill_data = PVPBattle_Sing_DataController.Instance().DictSkillData[(int)eSkill];
        this.LEADERCharacter = LEADERCharacter;

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // :: Do
    // :: Callback
    public System.Action Callback_CantSkill = null;
    public System.Action Callback_DidSkill = null;
    public void Do()
    {
        // :: Get
        EnumAll.eSkillType eSkillType = (EnumAll.eSkillType)this.skill_data.type;

        // :: Find
        switch (eSkillType)
        {
            case EnumAll.eSkillType.SELF:
                this.DoSkill_Self();
                break;
            case EnumAll.eSkillType.RANGE:
                this.DoSkill_Range();
                break;
            case EnumAll.eSkillType.DIRECTION:
                this.DoSkill_Direction();
                break;
        }
    }

    // :: Skill : Self
    private void DoSkill_Self()
    {
        // :: Set
        int duration = skill_data.duration;
        EnumAll.eStatus eStatus = (EnumAll.eStatus)skill_data.effective_status;
        int effectiveValue = skill_data.effective_value;

        // :: Routine
        this.StartCoroutine(this.DoSkill_SelfImplement(duration, eStatus, effectiveValue));
    }
    private IEnumerator DoSkill_SelfImplement(int duration, EnumAll.eStatus eStatus, int effectiveValue)
    {
        // :: for Use
        int seconds = 0;

        // :: Effect Start
        this.LEADERCharacter.ShowEffect(EnumAll.eEffect.HP_RECOVERY, true);

        // :: Routine
        while(true)
        {
            // :: EXIT
            if (seconds >= duration)
                break;

            // :: Do
            switch (eStatus)
            {
                case EnumAll.eStatus.HP:
                    this.LEADERCharacter.AddHP(effectiveValue);
                    break;
            }

            // :: Wait
            yield return new WaitForSeconds(1f);
            seconds += 1;
        }

        // :: Effect End
        this.LEADERCharacter.ShowEffect(EnumAll.eEffect.HP_RECOVERY, false);

        // :: Callback
        Callback_DidSkill?.Invoke();
    }

    // ::: Skill : Range
    private void DoSkill_Range()
    {
        this.StartCoroutine(this.DoSkill_RangeImplement());
    }
    private IEnumerator DoSkill_RangeImplement()
    {
        // :: for Use
        int seconds = 0;
        EnumAll.eStatus eStatus = (EnumAll.eStatus)this.skill_data.effective_status;

        // :: Effect Start
        this.LEADERCharacter.ShowEffect_Zone(EnumAll.eEffect.CURSE, this.skill_data.range, true);

        while (true)
        {
            // :: EXIT
            if (seconds >= this.skill_data.duration)
                break;

            // :: Find & Do
            List<InHeist_Leader_Character> listEnemies = this.LEADERCharacter.GetEnemiesInRange(this.skill_data.range);
            if(listEnemies != null && listEnemies.Count > 0)
            {
                foreach(var itm in listEnemies)
                {
                    switch(eStatus)
                    {
                        case EnumAll.eStatus.HP:
                            itm.AddHP(-this.skill_data.effective_value);
                            itm.ShowEffect(EnumAll.eEffect.CURSE);
                            break;
                    }
                }
            } 

            // :: Wait
            yield return new WaitForSeconds(1f);
            seconds += 1;
        }

        // :: Effect Start
        this.LEADERCharacter.ShowEffect_Zone(EnumAll.eEffect.CURSE, this.skill_data.range, false);

        // :: Callback
        Callback_DidSkill?.Invoke();
    }
    private void DoSkill_Direction()
    {
        InHeist_Leader_Character enemy = this.LEADERCharacter.GetEnemyInRange_MostNear(skill_data.range);
        if(enemy == null)
        {
            Callback_CantSkill?.Invoke();
        } else
        {
            // :: Look
            this.transform.LookAt(enemy.transform.position);

            // :: Shoot
            List<InHeist_Leader_Character> listEnemies = new List<InHeist_Leader_Character>();
            RaycastHit[] hits;
            Ray rayA = new Ray(this.transform.position + new Vector3(0, 0.5f, 0), this.transform.TransformDirection(Vector3.forward + new Vector3(0, 0.2f, 0)) * 20);
            Ray rayB = new Ray(this.transform.position + new Vector3(-0.5f, 0.5f, 0), this.transform.TransformDirection(Vector3.forward + new Vector3(0, 0.2f, 0)) * 20);
            Ray rayC = new Ray(this.transform.position + new Vector3(0.5f, 0.5f, 0), this.transform.TransformDirection(Vector3.forward + new Vector3(0, 0.2f, 0)) * 20);

            // :: Find
            hits = Physics.RaycastAll(rayA, 20f, LAYER_MASK_CHAMPION);
            foreach(var itm in hits)
            {
                // :: Get
                InHeist_Leader_Character hitEnemy = itm.transform.GetComponent<InHeist_Leader_Character>();
                
                // :: Add
                if(listEnemies.Contains(hitEnemy) == false)
                    if(hitEnemy.GetTeamColor() != this.LEADERCharacter.GetTeamColor())
                        listEnemies.Add(hitEnemy);
            }
            hits = Physics.RaycastAll(rayB, 20f, LAYER_MASK_CHAMPION);
            foreach(var itm in hits)
            {
                // :: Get
                InHeist_Leader_Character hitEnemy = itm.transform.GetComponent<InHeist_Leader_Character>();

                // :: Add
                if (listEnemies.Contains(hitEnemy) == false)
                    if (hitEnemy.GetTeamColor() != this.LEADERCharacter.GetTeamColor())
                        listEnemies.Add(hitEnemy);
            }
            hits = Physics.RaycastAll(rayC, 20f, LAYER_MASK_CHAMPION);
            foreach(var itm in hits)
            {
                // :: Get
                InHeist_Leader_Character hitEnemy = itm.transform.GetComponent<InHeist_Leader_Character>();

                // :: Add
                if (listEnemies.Contains(hitEnemy) == false)
                    if (hitEnemy.GetTeamColor() != this.LEADERCharacter.GetTeamColor())
                        listEnemies.Add(hitEnemy);
            }

            // :: Damage
            foreach(var itm in listEnemies)
            {
                switch((EnumAll.eStatus)this.skill_data.effective_status)
                {
                    case EnumAll.eStatus.HP:
                        itm.Damaged(this.skill_data.effective_value);
                        break;
                }
            }

            // :: Effect
            this.LEADERCharacter.ShowEffect(EnumAll.eEffect.BEAM);

            Callback_DidSkill?.Invoke();
        }
    }
}
