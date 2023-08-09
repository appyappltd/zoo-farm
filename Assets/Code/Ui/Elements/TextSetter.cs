using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using TMPro;
using UnityEngine;

namespace Ui.Elements
{
    public class TextSetter : MonoCache
    {
        [SerializeField] private bool _isUi;

        [HideIf("_isUi")] [SerializeField] private TextMeshPro _textSprite;
        [ShowIf("_isUi")] [SerializeField] private TextMeshProUGUI _textUi;

        private Action<string> _setTextAction;

        private void Awake()
        {
            Init();
            OnAwake();
        }

        private void Init()
        {
            if (_isUi)
            {
                _setTextAction = (text) => _textUi.SetText(text);
            }
            else
            {
                _setTextAction = (text) => _textSprite.SetText(text);
            }
        }

        protected virtual void OnAwake() { }

        public void SetText(string text)
        {
            if (_setTextAction is null)
                Init();

            _setTextAction.Invoke(text);
        }

        public void SetText(int text)
        {
            if (_setTextAction is null)
                Init();
            
            _setTextAction.Invoke(text.ToString());
        }
    }
}