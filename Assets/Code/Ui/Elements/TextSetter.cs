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
        
        [HideIf("_isUi")]
        [SerializeField] private TextMeshPro _textSprite;
        
        [ShowIf("_isUi")]
        [SerializeField] private TextMeshProUGUI _textUi;

        private Action<string> _setTextAction = s => {};

        private void Awake()
        {
            if (_isUi)
            {
                _setTextAction = (text) => _textUi.SetText(text);
            }
            else
            {
                _setTextAction = (text) => _textSprite.SetText(text);
            }

            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }

        public void SetText(string text) =>
            _setTextAction.Invoke(text);

        public void SetText(int text) =>
            _setTextAction.Invoke(text.ToString());
    }
}