namespace Logic.Plants.PlantSettings
{
    public class GrowthPlan
    {
        private readonly GrowStage[] _stages;

        private int _currentStageIndex;

        public GrowthPlan(GrowStage[] stages)
        {
            _stages = stages;
            Reset();
        }

        public bool TryGetNextStage(out GrowStage stage)
        {
            stage = null;
            
            if (_currentStageIndex >= _stages.Length)
            {
                return false;
            }

            stage = _stages[_currentStageIndex];
            _currentStageIndex++;
            return true;
        }

        public void Reset() =>
            _currentStageIndex = 0;
    }
}