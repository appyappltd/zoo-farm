using UnityEngine;
using UnityEngine.UI;

namespace Ui.Elements.Buttons
{
    [RequireComponent(typeof(Button))]
    public class ButtonObserver : MonoBehaviour
    {
        [SerializeField] protected Button Button;

        private void Awake() =>
            Button ??= GetComponent<Button>();

        public void Subscribe() =>
            Button.onClick.AddListener(Call);

        public void Cleanup() =>
            Button.onClick.RemoveAllListeners();

        protected virtual void Call() { }
    }
}