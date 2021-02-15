using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using DG.Tweening;

public class InHeist_Component_CharacterRPCController : MonoBehaviourPunCallbacks
{
    // : Leader
    private InHeist_Leader_Character CHARACTERLeader;

    // : Initialise
    public void Init()
    {
        this.CHARACTERLeader = this.GetComponent<InHeist_Leader_Character>();
    }

    // : Set
    [PunRPC]
    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    // : DOTween
    [PunRPC]
    public void DOLookAt(Vector3 targetPosition, float moving_speed)
    {
        this.transform.DOLookAt(targetPosition, moving_speed);
    }

    // : Add
    [PunRPC]
    public void AddHP(int hp)
    {
        this.CHARACTERLeader.AddHPImplement(hp);
    }
    [PunRPC]
    public void AddMP(int mp)
    {
        this.CHARACTERLeader.AddMPImplement(mp);
    }

    // : Do
    [PunRPC]
    public void DoAnimation(EnumAll.eAnimation animation)
    {
        this.CHARACTERLeader.DOAnimationImplement(animation);
    }

    // : Show
    [PunRPC]
    public void ShowEffect(EnumAll.eEffect eEffect, bool check)
    {
        this.CHARACTERLeader.ShowEffectImplement(eEffect, check);
    }
    [PunRPC]
    public void ShowEffect_Zone(EnumAll.eEffect eEffect, int range, bool check = true)
    {
        this.CHARACTERLeader.ShowEffect_ZoneImplement(eEffect, range, check);
    }
}
