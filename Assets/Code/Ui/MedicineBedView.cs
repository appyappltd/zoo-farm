using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Logic.Medicine;
using Logic.Storages.Items;
using StaticData;
using UnityEngine;

namespace Ui
{
    public class MedicineBedView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _toolIndicator;
        [SerializeField] private HealingIndicator _healingIndicator;
        [SerializeField] private MedicineBed _medicineBed;
        [SerializeField] private MedToolStandConfig[] _medTools;

        private Dictionary<MedicineToolId, Sprite> _toolSprites;

        private void Awake()
        {
            _toolSprites = _medTools.ToDictionary(key => key.Type, tool => tool.Icon);
            _toolIndicator.enabled = false;
            _background.enabled = false;
            _healingIndicator.Hide();
        }

        private void OnEnable()
        {
            _medicineBed.Added += OnAdded;
            _medicineBed.Healed += OnHealed;
        }
        
        private void OnDisable()
        {
            _medicineBed.Added -= OnAdded;
            _medicineBed.Healed -= OnHealed;
        }

        private void OnHealed()
        {
            _healingIndicator.Hide();
            _background.enabled = false;
        }

        private void OnAdded(IItem item)
        {
            if ((item.ItemId & ItemId.Animal) != 0)
            {
                AnimalItemData animalItemData = (AnimalItemData) item.ItemData;
                _toolIndicator.sprite = _toolSprites[animalItemData.TreatToolId];
                _toolIndicator.enabled = true;
                _background.enabled = true;
            }
            else
            {
                _toolIndicator.enabled = false;
                _healingIndicator.Show();
            }
        }
    }
}