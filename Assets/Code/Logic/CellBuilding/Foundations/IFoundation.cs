using System;

namespace Logic.CellBuilding.Foundations
{
    public interface IFoundation : IDisposable
    {
        void ShowBuildChoice();
        void HideBuildChoice();
    }
}