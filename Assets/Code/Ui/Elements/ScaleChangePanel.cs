using System;
using UnityEngine;

namespace Ui.Elements
{
    public class ScaleChangePanel : ShowHidePanel<Vector3>
    {
        protected override Func<Vector3, Vector3, float, Vector3> GetLerpFunction() =>
            Vector3.Lerp;

        protected override void ApplyLerpValue(Vector3 lerpValue) =>
            transform.localScale = lerpValue;

        protected override void OnShow()
        {
            Debug.Log("Show");
        }
        
        protected override void OnHide()
        {
            Debug.Log("Hide");
        }
    }
}