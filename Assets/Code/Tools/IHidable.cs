using System;

namespace Tools
{
    public interface IHidable
    {
        void Hide(Action OnHideCallback = null);
    }
}