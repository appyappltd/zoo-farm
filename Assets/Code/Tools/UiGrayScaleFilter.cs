using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tools
{
    public class UiGrayScaleFilter : MonoBehaviour
    {
        private readonly int EffectAmount = Shader.PropertyToID("_EffectAmount");
        
        private const string ImageHasNoMaterial = "Image has no material";

        [SerializeField] private Image _image;

        private void Awake()
        {
            Material material = _image.material;

            if (material is null) 
                throw new Exception(ImageHasNoMaterial);

            _image.material = Instantiate(material);
        }

        public void SetEffectAmount(float amount)
        {
            _image.material.SetFloat(EffectAmount, Mathf.Clamp01(amount));
        }
    }
}