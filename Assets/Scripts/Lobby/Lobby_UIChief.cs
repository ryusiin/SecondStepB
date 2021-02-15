using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Lobby_UIChief : MonoBehaviour
{
    // : Holder
    private Lobby_UIHolder UIHolder;

    // : Initialise
    public void Init()
    {
        // :: Holder
        this.UIHolder = this.GetComponent<Lobby_UIHolder>();
        this.UIHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Button Listener
    public void AddButtonListener_Heist(System.Action action)
    {
        this.UIHolder.BUTTON_heist.GetComponent<Button>().onClick.AddListener(() => { action?.Invoke(); });
    }

    // : Fade
    public void Fade_Dim(float endValue, float duration, System.Action action = null)
    {
        this.UIHolder.IMAGE_dim.GetComponent<Image>().DOFade(endValue, duration)
            .onComplete = () => {
                action?.Invoke(); 
            };
    }

    // : Show
    public void Show_Dim(bool check)
    {
        this.UIHolder.IMAGE_dim.SetActive(check);
    }

    // : Get
    public Image GetImage_Dim()
    {
        return this.UIHolder.IMAGE_dim.GetComponent<Image>();
    }
}
