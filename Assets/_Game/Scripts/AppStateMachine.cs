using System;
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
        private IStartScreenState _startScreenState;
        private IInGameState _inGameState;

        private void Start()
        {
            _initalizationState = _states.OfType<IInitializationState>().FirstOrDefault();
            _startScreenState = _states.OfType<IStartScreenState>().FirstOrDefault();
            _inGameState = _states.OfType<IInGameState>().FirstOrDefault();

            _initalizationState.OnComplete += InitalizationState_OnComplete;

            EnterState(EAppState.Initialization);
        }

        private void OnDestroy()
        {
            _initalizationState.OnComplete -= InitalizationState_OnComplete;
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
                case EAppState.None:
                    AppContext.Log.Error($"Cannot transition to state {state}");
                    succesful = false;
                    break;
                case EAppState.Initialization:
                    if (_initalizationState.IsComplete)
                    {
                        AppContext.Log.Error($"Initalization state has already completed");
                        succesful = false;
                    }
                    else
                    {
                        TransitionTo((_initalizationState as MonoBehaviour).name);
                    }
                    break;
                case EAppState.StartScreen:
                    TransitionTo((_startScreenState as MonoBehaviour).name);
                    break;
                case EAppState.InGame:
                    TransitionTo((_inGameState as MonoBehaviour).name);
                    break;
                default:
                    AppContext.Log.Error($"The state {state} has not not been implemented");
                    succesful = false;
                    break;
            }

            if (succesful)
            {
                _currentAppState = state;
            }
        }

        private void InitalizationState_OnComplete(object sender, EventArgs e)
        {
            EnterState(EAppState.StartScreen);
        }
    }
}