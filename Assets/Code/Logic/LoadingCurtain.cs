using System.Collections;
using UnityEngine;

namespace Logic
{
    public class LoadingCurtain : MonoBehaviour
    {
        private const float StepFade = 0.03f;

        private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(StepFade);

        [SerializeField] private CanvasGroup _curtain;

        private Coroutine _fade;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = 1;
        }

        public void Hide() => StartFade();

        private void StartFade()
        {
            if (_fade != null)
            {
                StopCoroutine(_fade);
                _fade = null;
            }

            _fade = StartCoroutine(FadeIn());
        }

        private IEnumerator FadeIn()
        {
            while (_curtain.alpha > 0)
            {
                _curtain.alpha -= StepFade;
                yield return _waitForSeconds;
            }

            gameObject.SetActive(false);
        }
    }
}