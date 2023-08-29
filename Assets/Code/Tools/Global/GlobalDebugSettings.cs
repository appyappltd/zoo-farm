using Services;

namespace Tools
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