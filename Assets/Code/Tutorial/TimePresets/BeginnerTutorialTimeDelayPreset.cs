using UnityEngine;

namespace Tutorial.TimePresets
{
    [CreateAssetMenu(menuName = "Presets/Beginner Tutorial Time Delay Preset", fileName = "NewBeginnerTutorialTimeDelayPreset", order = 0)]
    public class BeginnerTutorialTimeDelayPreset : ScriptableObject
    {
        [Range(0, 6f)] public float MedicalToolSpawnedToVolunteerFocus = 1f;
        [Range(0, 6f)] public float VolunteerFocusToArrowMoveToInteractionZone = 4f;
        [Range(0, 6f)] public float ArrowMoveToInteractionZoneToPlayerFocus = 1f;
        [Range(0, 6f)] public float MedicalBedFocusToPlayerFocus = 1f;
        [Range(0, 6f)] public float HouseFocusToPlayerFocus = 3f;
        [Range(0, 6f)] public float HouseBuiltToAnimalFocus = 0.2f;
        [Range(0, 6f)] public float AnimalFocusToPlantFocus = 2f;
        [Range(0, 6f)] public float PlantFocusToPlayerFocus = 1.5f;
        [Range(0, 6f)] public float PlantBuiltToPlantBuilt = 0.75f;
        [Range(0, 6f)] public float PlantBuiltToPlantFocus = 0.5f;
        [Range(0, 6f)] public float BowlEmptyToReleaserFocus = 1f;
        [Range(0, 6f)] public float AnimalReleaserFocusToPlayerFocus = 3f;
        [Range(0, 6f)] public float ThirdVolunteerFocusToPlayerFocus = 4f;
        [Range(0, 6f)] public float KeeperGridFocusToPlayerFocus = 3f;
        [Range(0, 6f)] public float BreedingBeginToBreedingComplete = 3f;
        [Range(0, 6f)] public float BreedingZoneFocusToPlayerFocus = 3f;
        [Range(0, 6f)] public float BreedingZoneSpawnedToArrowMove = 1f;
    }
}