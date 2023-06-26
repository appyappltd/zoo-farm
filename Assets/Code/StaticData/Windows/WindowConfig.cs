using System;
using Ui.Services;
using Ui.Windows;

namespace StaticData.Windows
{
  [Serializable]
  public class WindowConfig
  {
    public WindowId WindowId;
    public WindowBase Template;
  }
}