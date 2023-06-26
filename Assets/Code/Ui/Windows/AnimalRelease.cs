using Services.Animals;

namespace Ui.Windows
{
    public class AnimalRelease : WindowBase
    {
        private IAnimalsService _animalsService;

        public void Construct(IAnimalsService animalsService)
        {
            _animalsService = animalsService;
        }
        
        
    }
}