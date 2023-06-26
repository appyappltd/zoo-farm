using System.Collections.Generic;
using UnityEngine;

namespace StaticData.Windows
{
  [CreateAssetMenu(menuName = "Static Data/Window Configs", fileName = "WindowConfigs")]
  public class WindowStaticData : ScriptableObject
  {
    public List<WindowConfig> Configs;
  }
}