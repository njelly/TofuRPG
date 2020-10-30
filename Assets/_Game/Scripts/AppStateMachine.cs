using System;
using System.Collections.Generic;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class AppStateMachine : MonoBehaviour
    {
        [Header("Development")]
        [SerializeField] private bool _skipSplash;

        public LogService log;
        private AppState<SplashScreenStateController> _splashState;
        private AppState<StartScreenStateController> _startScreenState;

        private void Start()
        {
            _splashState = new AppState<SplashScreenStateController>(log, AppConsts.Scenes.Splash);
            _splashState.OnComplete += SplashState_OnComplete;

            _startScreenState = new AppState<StartScreenStateController>(log, AppConsts.Scenes.StartScreen);
            _startScreenState.OnComplete += StartScreenState_OnComplete;

            _splashState.Enter();
        }

        private void OnDestroy()
        {
            _splashState.OnComplete -= SplashState_OnComplete;
            _startScreenState.OnComplete -= StartScreenState_OnComplete;
        }

        private void SplashState_OnComplete(object sender, EventArgs e)
        {
            _splashState.Exit();
            _startScreenState.Enter();
        }

        private void StartScreenState_OnComplete(object sender, EventArgs e)
        {
            _startScreenState.Exit();
        }
    }
}