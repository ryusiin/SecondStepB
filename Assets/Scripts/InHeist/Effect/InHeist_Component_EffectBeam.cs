using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InHeist_Component_EffectBeam : MonoBehaviour
{
    // :: for Use
    public LineRenderer LINE_RENDERER_Beam;
    public Transform TRANSFORM_Circle;

    // :: Const
    const int CLOSE_DIVISION = 2;

    public System.Action Callback_EndBeam = null;
    public void Shoot(float duration)
    {
        // :: First Set
        this.LINE_RENDERER_Beam.startWidth = 0.1f; // :: 1.0까지
        this.LINE_RENDERER_Beam.endWidth = 0.1f; // :: 1.0까지
        this.TRANSFORM_Circle.localScale = new Vector3(0.5f, 0.5f, 0.5f); // :: 1.5까지

        // :: Show
        this.gameObject.SetActive(true);

        // :: DO
        DOTween.To(() => this.LINE_RENDERER_Beam.startWidth,
        ele => this.LINE_RENDERER_Beam.startWidth = ele,
        1.0f, duration)
            .SetEase(Ease.OutQuint);
        DOTween.To(() => this.LINE_RENDERER_Beam.endWidth,
        ele => this.LINE_RENDERER_Beam.endWidth = ele,
        1.0f, duration)
            .SetEase(Ease.OutQuint);
        this.TRANSFORM_Circle.DOScaleX(1.5f, duration)
            .SetEase(Ease.OutQuint);
        this.TRANSFORM_Circle.DOScaleY(1.5f, duration)
            .SetEase(Ease.OutQuint)
            .onComplete = () =>
            {
                DOTween.To(() => this.LINE_RENDERER_Beam.startWidth,
                ele => this.LINE_RENDERER_Beam.startWidth = ele,
                0.1f, duration/CLOSE_DIVISION)
                    .SetEase(Ease.OutQuint);
                DOTween.To(() => this.LINE_RENDERER_Beam.endWidth,
                ele => this.LINE_RENDERER_Beam.endWidth = ele,
                0.1f, duration/CLOSE_DIVISION)
                .SetEase(Ease.OutQuint);
                this.TRANSFORM_Circle.DOScaleX(1.7f, duration/CLOSE_DIVISION)
                .SetEase(Ease.OutQuint);
                this.TRANSFORM_Circle.DOScaleY(1.7f, duration/CLOSE_DIVISION)
                    .SetEase(Ease.OutQuint)
                    .onComplete = () =>
                    {
                        // :: Callback
                        this.gameObject.SetActive(false);
                        this.Callback_EndBeam?.Invoke();
                    };
            };
    }
}
