using Logic.Animals.AnimalsBehaviour.Emotions;
using Ui.Services;
using Ui.Windows;

namespace Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    Emotion EmotionById(EmotionId emotionId);
    WindowBase WindowById(WindowId windowId);
  }
}