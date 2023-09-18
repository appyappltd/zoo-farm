using System;
using UnityEngine;

namespace Ui.Elements
{
    public class FadeOutPanel : ShowHide<float>
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        protected override Func<float, float, float, float> GetLerpFunction() =>
            Mathf.Lerp;

        protected override void ApplyLerpValue(float lerpValue) =>
            _canvasGroup.alpha = lerpValue;
    }
}