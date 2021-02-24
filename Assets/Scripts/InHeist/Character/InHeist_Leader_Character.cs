using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class InHeist_Leader_Character : MonoBehaviour
{
    // :: Control
    [Header("Status")]
    [SerializeField]
    private EnumAll.eCharacter characterType;
    [SerializeField]
    private EnumAll.eTeam teamColor;
    [SerializeField]
    private string guid;

    // :: Variables
    private GameObject currentCharacter;
    private Animator currentChracter_Animator;
    private Character_Data currentCharacter_Data;
    [SerializeField]
    private InHeist_Leader_Tile currentTile;
    [SerializeField]
    private int currentCharacter_CurHP;
    [SerializeField]
    private int currentCharacter_CurMP;

    // :: UI
    private Slider mpSlider;
    private Slider hpSlider;

    // :: Hash
    private int hash_BOOL_walking;
    private int hash_TRIGGER_attack;

    // :: Variables Adjustment
    private Vector3 adjustment_characterPosition = new Vector3(0, 0.25f, 0);

    // :: Constant
    const int ADD_MANA = 30;
    const float MIN_DAMAGE_PERSENT = 0.7f;
    const float MAX_DAMAGE_PERSENT = 1.3f;

    // :: Sing
    private PVPBattle_Sing_DataController DATAController;

    // :: Class
    private InHeist_Class_PathFinder CLASS_PathFinder;

    // :: Component
    private InHeist_Component_CharacterSkillController SKILLController;
    private InHeist_Component_CharacterRPCController RPCController;

    // :: Holder
    private InHeist_Holder_Character CHARACTERHolder;

    // :: Initialise
    public void Init(InHeist_Class_Character characterStatus, Vector3 characterPosition, string guid)
    {
        // :: Sing
        this.DATAController = PVPBattle_Sing_DataController.Instance();

        // :: Class
        this.CLASS_PathFinder = new InHeist_Class_PathFinder();

        // :: Component
        this.SKILLController = this.GetComponent<InHeist_Component_CharacterSkillController>();
        this.RPCController = this.GetComponent<InHeist_Component_CharacterRPCController>();
        this.RPCController.Init();

        // :: Holder
        this.CHARACTERHolder = this.GetComponent<InHeist_Holder_Character>();
        this.CHARACTERHolder.Init();

        // :: Set
        this.characterType = characterStatus.CharacterType;
        this.teamColor = characterStatus.Team;
        this.transform.position = characterPosition;
        if(this.teamColor == EnumAll.eTeam.RED)
            this.transform.localRotation = Quaternion.Euler(10, 180, 0);
        else if(this.teamColor == EnumAll.eTeam.BLUE)
            this.transform.localRotation = Quaternion.Euler(-10, 0, 0);
        this.currentCharacter_Data = DATAController.DictCharacterData[(int)this.characterType];
        this.InitSkill();

        // :: Set Variables
        this.currentCharacter_CurHP = currentCharacter_Data.max_hp;
        this.currentCharacter_CurMP = 0;

        // :: Set Status
        this.InitRequiredMP();
        this.InitMaxHP();
        this.guid = guid;

        // :: Render
        this.ShowCharacter(this.characterType);
        this.ShowSlider(true);

        // :: Animation
        this.currentChracter_Animator = this.currentCharacter.GetComponent<Animator>();
        this.hash_BOOL_walking = Animator.StringToHash("BOOL_walking");
        this.hash_TRIGGER_attack = Animator.StringToHash("TRIGGER_attack");

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }


    // : Show
    private void ShowResetCharacter()
    {
        this.CHARACTERHolder.character_1.SetActive(false);
        this.CHARACTERHolder.character_2.SetActive(false);
        this.CHARACTERHolder.character_3.SetActive(false);
        this.CHARACTERHolder.character_4.SetActive(false);
        this.CHARACTERHolder.character_5.SetActive(false);
        this.CHARACTERHolder.character_6.SetActive(false);
    }
    private void ShowCharacter(EnumAll.eCharacter characterType)
    {
        // :: Reset
        this.ShowResetCharacter();

        // :: Set
        switch (characterType)
        {
            case EnumAll.eCharacter.SAKIRI:
                this.currentCharacter = this.CHARACTERHolder.character_1;
                break;
            case EnumAll.eCharacter.AHURA:
                this.currentCharacter = this.CHARACTERHolder.character_2;
                break;
            case EnumAll.eCharacter.HARU:
                this.currentCharacter = this.CHARACTERHolder.character_3;
                break;
            case EnumAll.eCharacter.AMY:
                this.currentCharacter = this.CHARACTERHolder.character_4;
                break;
            case EnumAll.eCharacter.RAJESH:
                this.currentCharacter = this.CHARACTERHolder.character_5;
                break;
            case EnumAll.eCharacter.ARSENE:
                this.currentCharacter = this.CHARACTERHolder.character_6;
                break;
        }

        // :: Show
        this.currentCharacter.SetActive(true);
    }
    private void ShowSlider(bool check)
    {
        // :: Divide
        if (this.teamColor == EnumAll.eTeam.RED)
        {
            this.CHARACTERHolder.IMAGE_fillHP.GetComponent<Image>().color = Color.red;
        }
        else if (this.teamColor == EnumAll.eTeam.BLUE)
        {
            this.CHARACTERHolder.IMAGE_fillHP.GetComponent<Image>().color = Color.blue;
        }

        // :: Show
        this.CHARACTERHolder.SLIDER_mp.SetActive(check);
        this.CHARACTERHolder.SLIDER_hp.SetActive(check);
    }
    public System.Action<Transform> Please_ShootBeam = null;
    public void ShowEffect(EnumAll.eEffect eEffect, bool check = true)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("ShowEffect", RpcTarget.All, eEffect, check);
        else
            this.RPCController.ShowEffect(eEffect, check);
    }
    public void ShowEffectImplement(EnumAll.eEffect eEffect, bool check)
    {
        switch (eEffect)
        {
            case EnumAll.eEffect.HP_RECOVERY:
                this.CHARACTERHolder.EFFECT_heal.SetActive(check);
                break;
            case EnumAll.eEffect.CURSE:
                this.StartCoroutine(this.ShowEffect_Curse());
                break;
            case EnumAll.eEffect.BEAM:
                this.Please_ShootBeam?.Invoke(this.transform);
                break;
            case EnumAll.eEffect.HIT:
                var hitEffect = this.CHARACTERHolder.EFFECT_hit.GetComponent<ParticleSystem>();
                var angle = Random.Range(-60, 61);
                hitEffect.gameObject.transform.localRotation = Quaternion.Euler(angle, 90, 0);
                hitEffect.Play();
                break;
        }
    }
    private IEnumerator ShowEffect_Curse()
    {
        this.CHARACTERHolder.EFFECT_curse.SetActive(true);

        yield return new WaitForSeconds(1f);

        this.CHARACTERHolder.EFFECT_curse.SetActive(false);
    }
    public void ShowEffect_Zone(EnumAll.eEffect eEffect, int range, bool check = true)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("ShowEffect_Zone", RpcTarget.All, eEffect, range, check);
        else
            this.RPCController.ShowEffect_Zone(eEffect, range, check);
    }
    public void ShowEffect_ZoneImplement(EnumAll.eEffect eEffect, int range, bool check = true)
    {
        // :: Set
        float xScale = 2.5f * range;
        float yScale = 2.5f * range;

        // :: Show
        switch (eEffect)
        {
            case EnumAll.eEffect.CURSE:
                this.CHARACTERHolder.EFFECT_zoneCurse.transform.localScale = new Vector3(xScale, yScale, 1);
                this.CHARACTERHolder.EFFECT_zoneCurse.SetActive(check);
                break;
        }
    }

    // ::: Get
    public InHeist_Class_Character GetCharacterStatus()
    {
        return new InHeist_Class_Character(characterType, teamColor);
    }
    public InHeist_Leader_Tile GetCurrentTile()
    {
        return this.currentTile;
    }
    public EnumAll.eTeam GetTeamColor()
    {
        return this.teamColor;
    }
    public int GetCost()
    {
        return PVPBattle_Sing_DataController.Instance().DictCharacterData[(int)this.characterType].cost;
    }
    public int GetAttackRange()
    {
        return this.currentCharacter_Data.atk_range;
    }
    public string GetGUID()
    {
        return this.guid;
    }
    public LinkedList<Vector3> GetPath(InHeist_Leader_Tile targetTile)
    {
        this.CLASS_PathFinder.Init();
        var pathList = this.CLASS_PathFinder.GetPath(this, targetTile);

        return pathList;
    }
    public List<InHeist_Leader_Character> GetEnemiesInRange(int range)
    {
        return this.CLASS_PathFinder.GetEnemyList(this.teamColor)
            .FindAll(ele =>
            Controller_Coordinate.GetDistance(this.currentTile.Coordinate, ele.currentTile.Coordinate) <= range);
    }
    public InHeist_Leader_Character GetEnemyInRange_MostNear(int range)
    {
        InHeist_Leader_Character enemy = null;
        
        foreach(var itm in this.GetEnemiesInRange(range))
        {
            // :: Continue
            if (enemy == null)
            {
                enemy = itm;
                continue;
            }

            // :: Set
            int enemyCoordinate = Controller_Coordinate.GetDistance(this.currentTile.Coordinate, enemy.currentTile.Coordinate);
            int itmCoordinate = Controller_Coordinate.GetDistance(this.currentTile.Coordinate, itm.currentTile.Coordinate);
            if (enemyCoordinate > itmCoordinate)
                enemy = itm;
        }

        return enemy;
    }

    // :: Init
    private void InitRequiredMP()
    {
        // :: for Use
        this.mpSlider = this.CHARACTERHolder.SLIDER_mp.GetComponent<Slider>();

        // :: Set
        this.mpSlider.maxValue = this.currentCharacter_Data.required_mp;
        this.mpSlider.value = 0;
    }
    private void InitMaxHP()
    {
        // :: for USe
        this.hpSlider = this.CHARACTERHolder.SLIDER_hp.GetComponent<Slider>();

        // :: Set
        this.hpSlider.maxValue = this.currentCharacter_Data.max_hp;
        this.hpSlider.value = this.currentCharacter_CurHP;
    }
    public void InitSkill()
    {
        EnumAll.eSkill eSkill = (EnumAll.eSkill)this.currentCharacter_Data.skill_id;
        this.SKILLController.Init(eSkill, this);
    }

    // :: Set
    public void SetTile(InHeist_Leader_Tile tile)
    {
        this.currentTile = tile;
    }
    public void SetCurrentMP()
    {
        mpSlider.DOValue(currentCharacter_CurMP, 0.2f);
    }
    public void SetCurrentHP()
    {
        hpSlider.DOValue(currentCharacter_CurHP, 0.2f);
    }
    public void SetTeam(EnumAll.eTeam eTeam)
    {
        this.teamColor = eTeam;
    }
    public void SetCharacter(EnumAll.eCharacter eCharacter)
    {
        this.characterType = eCharacter;
    }

    // :: Move
    public void Move(InHeist_Leader_Tile targetTile, System.Action completeAction = null)
    {
        // :: Release
        this.currentTile.ReleaseCharacter();
        this.currentTile = null;

        // :: Set
        targetTile.SetCharacter(this);
        this.currentTile = targetTile;

        // :: Move
        Vector3 targetPosition = this.currentTile.transform.position + adjustment_characterPosition;
        float moving_speed = 2 / this.currentCharacter_Data.moving_speed;

        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("DOLookAt", RpcTarget.Others, targetPosition, moving_speed / 10);

        this.transform.DOLookAt(targetPosition, moving_speed / 10)
            .onComplete = () =>
            {
                this.StartCoroutine(this.MoveImplement(targetPosition, moving_speed, completeAction));
            };
    }
    public void LookImplement(Vector3 targetPosition, float movingSpeed)
    {

    }
    private IEnumerator MoveImplement(Vector3 targetPosition, float movingSpeed, System.Action completeAction = null)
    {
        while (true)
        {
            float distance = Vector3.Distance(this.transform.position, targetPosition);

            // :: EXIT
            if (distance <= 0) { break; }

            // :: Set
            Vector3 setPosition = Vector3.MoveTowards(this.transform.position, targetPosition, Time.deltaTime * movingSpeed);
            
            // :: RPC
            if (PhotonNetwork.IsConnectedAndReady)
                this.RPCController.photonView.RPC("SetPosition", RpcTarget.All, setPosition);
            else
                this.RPCController.SetPosition(setPosition);

            yield return null;
        }

        completeAction?.Invoke();
    }

    // :: 8 Update
    private bool waitNow = false;
    private void Update()
    {
        if (waitNow == true)
        {
            // :: Get
            InHeist_Leader_Character nearEnemy = this.AutoBattle_GetNearEnemy();

            // :: Check Win
            if (nearEnemy == null)
            {
                this.AutoBattle_Win();
                return;
            }

            // :: Skill
            if (this.CheckMP() && this.nowSkillHold == false)
                this.DoSkill();

            // :: Attack
            int distance = Controller_Coordinate.GetDistance(this.currentTile.Coordinate, nearEnemy.currentTile.Coordinate);
            if (distance <= this.currentCharacter_Data.atk_range)
            {
                this.AutoBattle();
                return;
            }

            // :: Get Path
            var listPath = this.AutoBattle_GetNearEnemyPath(nearEnemy);

            // :: 길이 있으면
            if (listPath != null)
            {
                // :: 다시 시작해
                this.AutoBattle();
                return;
            }
            else
            {
                // :: 가짜 경로라도 있으면
                listPath = this.CLASS_PathFinder.GetNearPath(this, nearEnemy.GetCurrentTile());

                if (listPath != null)
                {
                    // :: 다시 시작해
                    this.AutoBattle();
                    return;
                }
            }
        }
    }

    // :: Check
    public bool CheckMP()
    {
        return this.currentCharacter_CurMP >= this.currentCharacter_Data.required_mp;
    }

    // ::: Do
    private bool nowSkillHold;
    public void DoSkill()
    {
        this.nowSkillHold = true;

        // :: Set Callback
        this.SKILLController.Callback_DidSkill = () =>
        {
            this.AddMP(-99999);
            this.AutoBattle();

            this.nowSkillHold = false;
        };
        this.SKILLController.Callback_CantSkill = () =>
        {
            this.AutoBattle();

            this.nowSkillHold = false;
        };

        // :: Do
        this.SKILLController.Do();
    }
    private void DOAnimation(EnumAll.eAnimation animation)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("DoAnimation", RpcTarget.All, animation);
        else
            this.RPCController.DoAnimation(animation);
    }
    public void DOAnimationImplement(EnumAll.eAnimation animation)
    {
        switch (animation)
        {
            case EnumAll.eAnimation.IDLE:
                this.currentChracter_Animator.SetBool(hash_BOOL_walking, false);
                break;
            case EnumAll.eAnimation.WALK:
                this.currentChracter_Animator.SetBool(hash_BOOL_walking, true);
                break;
            case EnumAll.eAnimation.ATTACK:
                this.currentChracter_Animator.SetBool(hash_BOOL_walking, false);
                this.currentChracter_Animator.SetTrigger(hash_TRIGGER_attack);
                break;
        }
    }

    // :: Auto
    public void AutoBattle()
    {
        // :: 기다림 해제
        this.waitNow = false;

        // :: Get
        InHeist_Leader_Character nearEnemy = this.AutoBattle_GetNearEnemy();

        // :: Check Win
        if (nearEnemy == null)
        {
            this.AutoBattle_Win();
            return;
        }

        // :: Skill
        if (this.CheckMP() && this.nowSkillHold == false)
            this.DoSkill();

        // :: Attack
        // :: Check Null
        if(nearEnemy.currentTile == null)
        {
            this.AutoBattle();
            return;
        }
        int distance = Controller_Coordinate.GetDistance(this.currentTile.Coordinate, nearEnemy.currentTile.Coordinate);
        if (distance <= this.currentCharacter_Data.atk_range)
        {
            this.AutoBattle_Attack(nearEnemy);
            return;
        }

        // :: Get Path
        var listPath = this.AutoBattle_GetNearEnemyPath(nearEnemy);

        // :: 길이 없으면
        if (listPath == null)
        {
            Debug.Log("왜 길이 없어?");

            // :: 가짜 경로를 찾아서 근처로 가
            listPath = this.CLASS_PathFinder.GetNearPath(this, nearEnemy.GetCurrentTile());

            // :: 그래도 없으면
            if (listPath == null)
            {
                // :: 기다려
                this.AutoBattle_Wait();
                return;
            }
        }

        if (listPath.Count > 48)
            Debug.LogError("이렇게 많이 나오면 안되는데?!");

        // :: 아니면 움직여
        this.AutoBattle_Move(listPath.First.Value);
    }
    public InHeist_Leader_Character AutoBattle_GetNearEnemy(params InHeist_Leader_Character[] arrayExcepts)
    {
        // :: Get
        List<InHeist_Leader_Character> listEnemies = this.CLASS_PathFinder.GetEnemyList(this.teamColor);

        // :: Find Except
        if (listEnemies.Count > 0 && arrayExcepts.Length > 0)
        {
            foreach (var except in arrayExcepts)
            {
                listEnemies.Contains(except);
                listEnemies.Remove(except);
            }
        }

        // :: EXIT
        if (listEnemies.Count == 0) return null;

        // :: Init
        InHeist_Leader_Character nearEnemy = listEnemies[0];

        // :: Get
        for (int i = 1; i < listEnemies.Count; i++)
        {
            float distanceA = Vector3.Distance(this.currentTile.transform.position, nearEnemy.currentTile.transform.position);
            float distanceB = Vector3.Distance(this.currentTile.transform.position, listEnemies[i].currentTile.transform.position);

            if (distanceA > distanceB)
            {
                nearEnemy = listEnemies[i];
            }
        }

        // :: Return
        return nearEnemy;
    }

    // : Win
    public System.Action<EnumAll.eTeam> Please_CheckWin = null;
    private void AutoBattle_Win()
    {
        this.DOAnimation(EnumAll.eAnimation.IDLE);
        this.Please_CheckWin?.Invoke(this.teamColor);
    }
    private void AutoBattle_Wait()
    {
        Debug.Log("갈 수 있는 곳이 없어!");
        this.DOAnimation(EnumAll.eAnimation.IDLE);
        this.waitNow = true;
    }
    private void AutoBattle_Move(Vector3 movePosition)
    {
        // :: Get will move tile
        var willMoveTile = this.CLASS_PathFinder.GetTargetTile(movePosition);
        int willMoveDistance = Controller_Coordinate.GetDistance(this.currentTile.Coordinate, willMoveTile.Coordinate);

        // [[Error 입니다, 이게 무조건 Distance가 1이 나와야 함]]
        if (willMoveDistance > 1)
        {
            Debug.LogError("Pathfinder 에러");
            return;
        }

        // :: Animation
        this.DOAnimation(EnumAll.eAnimation.WALK);

        this.Move(willMoveTile,
            () => {
                // :: Callback
                this.AutoBattle();
            });
    }
    private LinkedList<Vector3> AutoBattle_GetNearEnemyPath(InHeist_Leader_Character nearEnemy)
    {
        // :: Get
        var listPath = this.GetPath(nearEnemy.GetCurrentTile());

        // :: for Use
        List<InHeist_Leader_Character> exceptEnemies = new List<InHeist_Leader_Character>();

        if (nearEnemy != null)
            exceptEnemies.Add(nearEnemy);

        // :: Find
        while (listPath == null)
        {
            var newNearEnemy = this.AutoBattle_GetNearEnemy(exceptEnemies.ToArray());
            if (newNearEnemy != null)
            {
                listPath = this.GetPath(newNearEnemy.GetCurrentTile());
                if (listPath == null)
                {
                    exceptEnemies.Add(newNearEnemy);
                }
            }
            else
            {
                // :: EXIT
                break;
            }
        }

        // :: Return
        return listPath;
    }
    private InHeist_Leader_Tile AutoBattle_GetNear(Vector3 targetPosition)
    {
        var nearTileOrVector = this.CLASS_PathFinder.GetNearTile(this.currentTile.Coordinate, targetPosition);
        var nearTile = this.CLASS_PathFinder.GetTargetTile(nearTileOrVector);
        if (nearTile == null)
        {
            nearTile = this.AutoBattle_GetNear(nearTileOrVector);
        }

        return nearTile;
    }
    public void AutoBattle_Attack(InHeist_Leader_Character enemy)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("DOLookAt", RpcTarget.Others, enemy.transform.position, 0f);

        // :: Animation
        this.transform.DOLookAt(enemy.transform.position, 0f);
        this.DOAnimation(EnumAll.eAnimation.ATTACK);

        if(this.characterType == EnumAll.eCharacter.SAKIRI)
            this.Shoot(enemy.transform.position);

        // :: Get
        InHeist_Receiver_Animation animationReceiver = this.currentChracter_Animator.GetComponent<InHeist_Receiver_Animation>();

        // :: Do
        animationReceiver.callback_Attack_Hit = () =>
        {
            // :: Damage
            enemy.Damaged(this.currentCharacter_Data.base_atk);

            // :: MP
            this.AddMP(ADD_MANA);
        };
        animationReceiver.callback_Attack_End = () =>
        {
            this.AutoBattle();
        };
    }

    // :: Wait
    private Coroutine coroutine_wait;
    public void Wait(float seconds, System.Action action)
    {
        if (coroutine_wait == null)
            this.coroutine_wait = this.StartCoroutine(this.WaitImplement(seconds, action));
    }
    private IEnumerator WaitImplement(float seconds, System.Action action = null)
    {
        yield return new WaitForSeconds(seconds);

        action?.Invoke();
        this.coroutine_wait = null;
    }

    // :: Add
    public void AddHP(int addHP)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("AddHP", RpcTarget.All, addHP);
        else
            this.RPCController.AddHP(addHP);
    }
    public void AddHPImplement(int addHP)
    {
        // :: Set
        this.currentCharacter_CurHP += addHP;

        // :: Limit
        this.currentCharacter_CurHP = this.currentCharacter_CurHP > this.currentCharacter_Data.max_hp ?
            this.currentCharacter_Data.max_hp
            : this.currentCharacter_CurHP;

        // :: Show
        this.SetCurrentHP();
    }
    public void AddMP(int mp)
    {
        // :: RPC
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCController.photonView.RPC("AddMP", RpcTarget.All, mp);
        else
            this.RPCController.AddMP(mp);
    }
    public void AddMPImplement(int mp)
    {
        this.currentCharacter_CurMP += mp;
        if (this.currentCharacter_CurMP < 0)
            this.currentCharacter_CurMP = 0;

        // :: Show
        this.SetCurrentMP();
    }

    // : Damage
    public void Damaged(float rawDamage)
    {
        int damage = this.GetRandomDamage(rawDamage);
        this.AddHP(-damage);

        // :: GAME OVER
        if (this.currentCharacter_CurHP <= 0)
        {
            this.Die();
            return;
        }

        this.ShowEffect(EnumAll.eEffect.HIT);
    }
    private int GetRandomDamage(float damage)
    {
        return (int)Random.Range(damage * MIN_DAMAGE_PERSENT, damage * MAX_DAMAGE_PERSENT);
    }

    // : Shoot
    public System.Action<Vector3> Please_ShootMissile = null; 
    public void Shoot(Vector3 targetPosition)
    {
        this.Please_ShootMissile?.Invoke(targetPosition);
    }

    // :: Die
    public System.Action<string> Please_KillThisCharacter = null;
    private void Die()
    {
        // :: Release
        if (currentTile != null)
            this.currentTile.ReleaseCharacter();
        this.ReleaseTile();

        // :: Please
        this.Please_KillThisCharacter?.Invoke(this.guid);
    }

    // :: Release
    public void ReleaseTile()
    {
        this.currentTile = null;
    }
}
