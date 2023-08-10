using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Data.ItemsData;
using Logic.Medical;
using Logic.Storages.Items;
using StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ui
{
    public class MedicineBedView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _toolIndicator;
        [SerializeField] private HealingIndicator _healingIndicator;
        [FormerlySerializedAs("_medicineBed")]
        [SerializeField] private MedicalBed _medicalBed;
        [SerializeField] private MedToolStandConfig[] _medTools;

        private Dictionary<MedicalToolId, Sprite> _toolSprites;

        private void Awake()
        {
            _toolSprites = _medTools.ToDictionary(key => key.Type, tool => tool.Icon);
            _toolIndicator.enabled = false;
            _background.enabled = false;
            _healingIndicator.Hide();
        }

        private void OnEnable()
        {
            _medicalBed.Added += OnAdded;
            _medicalBed.Healed += OnHealed;
        }
        
        private void OnDisable()
        {
            _medicalBed.Added -= OnAdded;
            _medicalBed.Healed -= OnHealed;
        }

        private void OnHealed(AnimalId _)
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