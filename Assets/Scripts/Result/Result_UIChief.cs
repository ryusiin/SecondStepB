using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result_UIChief : MonoBehaviour
{
    // : Holder
    private Result_UIHolder UIHolder;

    // : Initialise
    public void Init()
    {
        // :: Holder
        this.UIHolder = this.GetComponent<Result_UIHolder>();
        this.UIHolder.Init();

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Add Button Listener
    public void AddButtonListener_OK(System.Action action)
    {
        this.UIHolder.Button_ok.GetComponent<Button>().onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

    // : Get
    public Image GetImage_Dim()
    {
        return this.UIHolder.IMAGE_dim.GetComponent<Image>();
    }
}
