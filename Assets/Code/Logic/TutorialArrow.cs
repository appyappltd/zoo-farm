using Logic.Translators;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(RunTranslator))]
    public class TutorialArrow : MonoBehaviour
    {
        [SerializeField] private RunTranslator _translator;
        [SerializeField] private TranslatableAgent _translatableAgent;

        private void Start()
        {
            _translator.AddTranslatable(_translatableAgent.MainTranslatable);
        }
    }
}