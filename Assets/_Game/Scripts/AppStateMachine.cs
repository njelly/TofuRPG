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
        private IInitializationController _initalizationState;
        private IStartScreenController _startScreenState;
        private IInGameState _inGameState;

        protected override void Awake()
        {
            base.Awake();

            _initalizationState = _states.OfType<IInitializationController>().FirstOrDefault();
            _startScreenState = _states.OfType<IStartScreenController>().FirstOrDefault();
            _inGameState = _states.OfType<IInGameState>().FirstOrDefault();

            EnterState(EAppState.Initialization);

            DontDestroyOnLoad(gameObject);
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
                        _initalizationState.OnComplete += InitalizationState_OnComplete;
                        TransitionTo((_initalizationState as MonoBehaviour).name);
                    }
                    break;
                case EAppState.StartScreen:
                    _startScreenState.EnterGameRequested += StartScreenState_EnterGameRequested;
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
                switch (_currentAppState)
                {
                    case EAppState.Initialization:
                        _initalizationState.OnComplete -= InitalizationState_OnComplete;
                        break;
                    case EAppState.StartScreen:
                        _startScreenState.EnterGameRequested -= StartScreenState_EnterGameRequested;
                        break;
                }

                _currentAppState = state;
            }
        }

        private void InitalizationState_OnComplete(object sender, EventArgs e)
        {
            EnterState(EAppState.StartScreen);
        }

        private void StartScreenState_EnterGameRequested(object sender, EventArgs e)
        {
            EnterState(EAppState.InGame);
        }
    }
}