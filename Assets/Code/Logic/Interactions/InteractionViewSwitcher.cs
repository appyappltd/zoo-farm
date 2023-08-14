using NTC.Global.System;

namespace Logic.Interactions
{
    public class InteractionViewSwitcher
    {
        private readonly InteractionView _defaultView;
        private readonly InteractionView _viewWithBackground;
        private readonly IInteractionZone _interactionZone;

        private InteractionView _currentView;
        
        public InteractionViewSwitcher(InteractionView defaultView, InteractionView viewWithBackground)
        {
            _defaultView = defaultView;
            _viewWithBackground = viewWithBackground;

            _currentView = defaultView;
        }

        public void SwitchToDefault() =>
            Switch(_defaultView);

        public void SwitchToBackground() =>
            Switch(_viewWithBackground);

        private void Switch(InteractionView toView)
        {
            if (_currentView == toView)
                return;

            _currentView.gameObject.Disable();
            _currentView = toView;
            _currentView.SetDefault();
            _currentView.gameObject.Enable();
        }
    }
}