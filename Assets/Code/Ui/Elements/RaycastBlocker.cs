using UnityEngine;
using UnityEngine.UI;

namespace Ui.Elements
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class RaycastBlocker : Graphic
    {
        public override void SetMaterialDirty() { return; }
        public override void SetVerticesDirty() { return; }
    }
}