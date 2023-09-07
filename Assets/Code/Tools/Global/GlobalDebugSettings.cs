using Services;

namespace Tools.Global
{
    public class GlobalDebugSettings : IGlobalSettings
    {
        public bool CanLetHungryAnimalsRelease { get; set; }
    }

    public interface IGlobalSettings : IService
    {
        bool CanLetHungryAnimalsRelease { get; set; }
    }
}