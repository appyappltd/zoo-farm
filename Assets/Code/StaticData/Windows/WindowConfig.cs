using System;
using Ui.Services;
using Ui.Windows;

namespace CodeBase.StaticData.Windows
{
  [Serializable]
  public class WindowConfig
  {
    public WindowId WindowId;
    public WindowBase Template;
  }
}