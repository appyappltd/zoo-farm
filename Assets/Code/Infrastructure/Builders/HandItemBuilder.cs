using Data;
using Data.ItemsData;
using Logic.Foods.FoodSettings;
using Logic.Medical;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class HandItemBuilder
    {
        private readonly IStaticDataService _staticData;

        public HandItemBuilder(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public HandItem Build(GameObject itemObject, AnimalAndTreatToolPair pair)
        {
            HandItem item = itemObject.GetComponent<HandItem>();
            item.Construct(new AnimalItemData(pair.AnimalStaticData, pair.TreatTool));
            return item;
        }

        public HandItem Build(GameObject itemObject, FoodId foodId)
        {
            HandItem item = itemObject.GetComponent<HandItem>();
            FoodItemData foodItemData = _staticData.FoodItemDataById(foodId);
            item.Construct(foodItemData);
            return item;
        }

        public HandItem Build(GameObject itemObject, MedicalToolId medicalToolId)
        {
            HandItem item = itemObject.GetComponent<HandItem>();
            MedicalToolItemData foodItemData = _staticData.MedicalItemDataById(medicalToolId);
            item.Construct(foodItemData);
            return item;
        }

        public HandItem Build(GameObject itemObject, ItemId itemId)
        {
            HandItem item = itemObject.GetComponent<HandItem>();
            DefaultItemData foodItemData = _staticData.DefaultItemDataById(itemId);
            item.Construct(foodItemData);
            return item;
        }
    }
}