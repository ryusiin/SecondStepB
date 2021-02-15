using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InHeist_Receiver_Animation : MonoBehaviour
{
    public System.Action callback_Attack_Hit = null;
    public void Attack_Hit()
    {
        this.callback_Attack_Hit?.Invoke();
    }

    public System.Action callback_Attack_End = null;
    public void Attack_End()
    {
        this.callback_Attack_End?.Invoke();
    }
}
