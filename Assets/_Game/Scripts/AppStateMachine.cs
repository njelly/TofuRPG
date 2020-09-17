using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class AppStateMachine : TofuStateMachine, IAppStateMachine
    {
        EAppState IAppStateMachine.CurrentAppState => _currentAppState;

        private EAppState _currentAppState;
        private IInitializationState _initalizationState;
        private string _initializationStateKey;
        private IStartScreenState _startScreenState;
        private string _startScreenStateKey;
        private IInGameState _inGameState;
        private string _inGameStateKey;

        protected override void Awake()
        {
            base.Awake();

            MonoBehaviour[] children = GetComponentsInChildren<MonoBehaviour>();
            _initalizationState = children.OfType<IInitializationState>().FirstOrDefault();
            _initializationStateKey = (_initalizationState as MonoBehaviour).gameObject.name;
            _startScreenState = children.OfType<IStartScreenState>().FirstOrDefault();
            _startScreenStateKey = (_startScreenState as MonoBehaviour).gameObject.name;
            _inGameState = children.OfType<IInGameState>().FirstOrDefault();
            _startScreenStateKey = (_inGameState as MonoBehaviour).gameObject.name;

            EnterState(EAppState.Initialization);
        }

        public void EnterState(EAppState state)
        {
            if (_currentAppState == state)
            {
                return;
            }

            bool succesful = true;
            switch (state)
            {
                case EAppState.Initialization:
                    if (_initalizationState.IsComplete)
                    {
                        AppContext.Log.Error($"Initalization state has already completed");
                        succesful = false;
                    }
                    else
                    {
                        TransitionTo(_initializationStateKey);
                    }
                    break;
                case EAppState.StartScreen:
                    TransitionTo(_startScreenStateKey);
                    break;
                case EAppState.InGame:
                    TransitionTo(_inGameStateKey);
                    break;
                default:
                    AppContext.Log.Error($"Failed to enter state {state}");
                    succesful = false;
                    break;
            }

            if (succesful)
            {
                _currentAppState = state;
            }
        }
    }
}