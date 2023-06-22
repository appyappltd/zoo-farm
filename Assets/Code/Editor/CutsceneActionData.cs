using System;
using UnityEngine.Events;

namespace Code.Editor
{
    [Serializable]
    public class CutsceneActionData : ICutsceneData
    {
        public UnityEvent Action;
    }
}