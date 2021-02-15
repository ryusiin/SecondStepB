using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Manager_Dim
{
    private Image IMAGE_Dim;

    public Manager_Dim(Image IMAGE_Dim)
    {
        this.IMAGE_Dim = IMAGE_Dim;
    }

    public void Fade(float endValue, float duration, System.Action action = null)
    {
        this.IMAGE_Dim.DOFade(endValue, duration)
            .onComplete = () => { action?.Invoke(); };
    }
    public void Show(bool check)
    {
        this.IMAGE_Dim.gameObject.SetActive(check);
    }
}
