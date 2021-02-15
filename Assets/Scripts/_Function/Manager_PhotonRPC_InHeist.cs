using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;
using System;

public class Manager_PhotonRPC_InHeist : MonoBehaviourPunCallbacks
{
    // : Dictator
    private Dictator Dictator;

    // : Chief
    private InHeist_GOChief GOChief;
    private InHeist_UIChief UIChief;
    private InHeist_Activist_Map MAPActivist;

    // : Function
    private Manager_Dim DIMManager;

    // : Status
    private Vector3 ADJUSTMENT_POSITION = new Vector3(0, 0.25f, 0);

    // : Init
    [PunRPC]
    public void Init_InHeist()
    {
        // :: To Use
        this.Dictator = GameObject.FindObjectOfType<Dictator>();
        this.GOChief = GameObject.FindObjectOfType<InHeist_GOChief>();
        this.UIChief = GameObject.FindObjectOfType<InHeist_UIChief>();
        this.MAPActivist = GameObject.FindObjectOfType<InHeist_Activist_Map>();
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Start Count Down
        if(PhotonNetwork.IsMasterClient)
            this.Do_CountDown(15);
    }

    // : Do
    [PunRPC]
    public void Do_AutoBattle()
    {
        this.photonView.RPC("ShowButton_StartBattle", RpcTarget.All);
    }
    public void Do_CountDown(float limit = 10f)
    {
        float timeLimit = limit;
        this.StartCoroutine(this.Do_CountDownImplement(timeLimit));
    }
    public System.Action Callback_CompleteCountDown = null;
    private IEnumerator Do_CountDownImplement(float timeLimit)
    {
        while (true)
        {
            timeLimit -= Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(timeLimit);
            string timeString = string.Format("{0}", time.ToString(@"ss\:ff"));

            if (PhotonNetwork.IsConnectedAndReady)
                this.photonView.RPC("Set_CountDown", RpcTarget.All, timeString);
            else
                this.Set_CountDown(timeString);

            if (timeLimit <= 0)
                break;

            yield return null;
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            this.photonView.RPC("Set_CountDown", RpcTarget.All, "BATTLE\nStart!");
            this.Callback_CompleteCountDown?.Invoke();
        }
        else
            this.Set_CountDown("BATTLE\nStart!");
    }

    // : Show
    [PunRPC]
    public void ShowButton_StartBattle()
    {
        this.UIChief.ShowButton_StartBattle(false);
    }

    // : Win
    public System.Action<EnumAll.eScene> Please_MoveScene;
    [PunRPC]
    public void Win(EnumAll.eTeam eTeam)
    {
        Dictator.Debug_All(this.ToString(), string.Format("{0} Win!", eTeam));
        this.DIMManager.Show(true);
        this.StartCoroutine(this.WaitAndDo(() => {
            this.DIMManager.Fade(1f, 1f, () =>
            {
                this.Dictator.LoadScene(EnumAll.eScene.RESULT);
            });
        }));
    }
    // : Wait
    private IEnumerator WaitAndDo(System.Action action)
    {
        yield return new WaitForSeconds(3f);
        action?.Invoke();
    }

    // : Set
    public System.Action<EnumAll.eTeam> Please_DoScenarioWin;
    [PunRPC]
    public void Set_NewCharacter(EnumAll.eCharacter eCharacter, EnumAll.eTeam eTeam, Vector3 tileCoordinate, string guid)
    {
        // :: Set for Use
        InHeist_Class_Character characterStatus = new InHeist_Class_Character(eCharacter, eTeam);
        InHeist_Leader_Tile tile = this.MAPActivist.GetTile(tileCoordinate);

        // :: Get
        var characterGO = this.GOChief.GetCharacterFromPool();
        var character = characterGO.GetComponent<InHeist_Leader_Character>();

        // :: Set : Character
        character.Init(characterStatus, tile.transform.localPosition + ADJUSTMENT_POSITION, guid); // ::: Init
        character.Please_CheckWin = (winTeam) => { this.Please_DoScenarioWin?.Invoke(winTeam); }; // ::: Game Over : Win
        character.Please_ShootBeam = (characterTransform) => // :: Answer
        {
            Transform beam = this.GOChief.GetBeamPool().GetChild(0);
            beam.SetParent(this.GOChief.GetField());
            beam.transform.position = characterTransform.position;
            beam.transform.rotation = characterTransform.rotation;

            InHeist_Component_EffectBeam beamScript = beam.GetComponent<InHeist_Component_EffectBeam>();
            beamScript.Callback_EndBeam = () =>
            {
                beam.SetParent(this.GOChief.GetBeamPool());
            };
            beamScript.Shoot(0.5f);
        };
        character.Please_ShootMissile = (targetRawPosition) =>
        {
            Transform missile = this.GOChief.GetMissilePool().GetChild(0);
            missile.gameObject.SetActive(true);
            missile.SetParent(this.GOChief.GetField());
            missile.transform.position = character.transform.position + ADJUSTMENT_POSITION + new Vector3(0, 0.5f, 0);
            Vector3 targetPosition = targetRawPosition + ADJUSTMENT_POSITION + new Vector3(0, 0.5f, 0);
            missile.LookAt(targetPosition);

            missile.transform.DOMove(targetPosition, 0.6f)
            .SetDelay(0.1f)
            .onComplete = () =>
            {
                missile.gameObject.SetActive(false);
                missile.SetParent(this.GOChief.GetMissilePool());
            };
        };
        character.SetTile(tile);
        this.GOChief.SetCharacterInField(characterGO);
        characterGO.SetActive(true);

        // :: Set : Tile
        tile.SetCharacter(character);
    }
    [PunRPC]
    public void Set_Character(string guid, Vector3 tileCoordinate)
    {
        // :: Easy to Use
        InHeist_Leader_Character character = this.GOChief.GetCharacterInField(guid);
        InHeist_Leader_Tile tile = this.MAPActivist.GetTile(tileCoordinate);

        // :: Release
        character.GetCurrentTile().ReleaseCharacter();
        character.ReleaseTile();

        // :: Set : Character
        character.SetTile(tile);
        character.transform.localPosition = tile.transform.localPosition + ADJUSTMENT_POSITION;

        // :: Set : Tile
        tile.SetCharacter(character);
    }
    [PunRPC]
    public void Set_CountDown(string text)
    {
        this.UIChief.SetText_RemainingTime(text);
    }

    // : Kill
    [PunRPC]
    public void Kill_Character(string guid)
    {
        // :: EXIT
        var character = this.GOChief.GetCharacterInField(guid);
        if (character == null)
            return;
        
        // :: Release
        character.transform.SetParent(this.GOChief.GetCharacterPool());
        character.transform.gameObject.SetActive(false);
    }

    // : Load
    [PunRPC]
    public void LoadLevel_InHeist()
    {
        // :: Callback
        this.StartCoroutine(this.Load_LevelImplement(() =>
        {
            this.photonView.RPC("Init_InHeist", RpcTarget.All);
        }));
    }
    private IEnumerator Load_LevelImplement(System.Action action)
    {
        while (PhotonNetwork.LevelLoadingProgress < 1)
            yield return null;
        action?.Invoke();
    }
}