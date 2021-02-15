using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Intro_Ruler : MonoBehaviour
{
    // : Development
    public bool DoTest = false;

    // : 0 Awake
    private void Awake()
    {
        if (this.DoTest == false)
            Dictator.CheckDictator();
        else
            this.Init();
    }

    // : Assign
    public Image IMAGE_Logo;

    // : Please
    public System.Action<EnumAll.eScene> Please_MoveScene;

    // : Initialise
    public void Init()
    {
        // :: Fade
        this.Fade_Logo(() => {
            Please_MoveScene?.Invoke(EnumAll.eScene.LOBBY);
        });

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }
    // : Fade
    private void Fade_Logo(System.Action action)
    {
        this.IMAGE_Logo.DOFade(0f, 0f);
        this.IMAGE_Logo.gameObject.SetActive(true);
        this.IMAGE_Logo.DOFade(1f, 1f)
            .onComplete = () =>
            {
                this.IMAGE_Logo.DOFade(0f, 2f)
                .SetDelay(2f)
                .onComplete = () =>
                {
                    action?.Invoke();
                };
            };
    }
}
