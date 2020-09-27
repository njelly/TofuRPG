using System;
using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.TofuRPG
{
    public class StartScreenController : TofuStateMachine, IStartScreenController
    {
        public event EventHandler EnterGameRequested;

        EStartScreenState IStartScreenController.CurrentStartScreenState => _currentStartScreenState;

        private EStartScreenState _currentStartScreenState;
        private IStartScreenRootController _rootState;

        protected override void Awake()
        {
            base.Awake();

            _rootState = _states.OfType<IStartScreenRootController>().FirstOrDefault();
        }

        private void OnEnable()
        {
            if(SceneManager.GetActiveScene().buildIndex != AppConsts.Scene.Start)
            {
                SceneManager.LoadScene(AppConsts.Scene.Start);
            }

            EnterState(EStartScreenState.Root);
        }

        public void EnterState(EStartScreenState state)
        {
            if(state == _currentStartScreenState)
            {
                return;
            }

            bool succesful = true;
            switch (state)
            {
                case EStartScreenState.None:
                    AppContext.Log.Error($"Cannot transition to state {state}");
                    succesful = false;
                    break;
                case EStartScreenState.Root:
                    _rootState.QuitGameRequested += RootState_QuitGameRequested;
                    _rootState.PlayGameRequested += RootState_PlayGameRequested;
                    TransitionTo((_rootState as MonoBehaviour).name);
                    break;
                default:
                    AppContext.Log.Error($"transition to {state} has not been implemented");
                    succesful = false;
                    break;
            }

            if(succesful)
            {
                switch (_currentStartScreenState)
                {
                    case EStartScreenState.Root:
                        _rootState.QuitGameRequested -= RootState_QuitGameRequested;
                        break;
                }

                _currentStartScreenState = state;
            }
        }

        private void RootState_QuitGameRequested(object sender, EventArgs e)
        {
            AppContext.Log.Info($"RootState_QuitGameRequested");
        }

        private void RootState_PlayGameRequested(object sender, EventArgs e)
        {
            EnterGameRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}