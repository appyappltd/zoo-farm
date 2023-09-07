using System;

namespace Tools
{
    public interface IShowable
    {
        void Show(Action OnShowCallback = null);
    }
}