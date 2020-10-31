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

        private AppState<SplashScreenStateController> _splashState;
        private AppState<StartScreenStateController> _startScreenState;
        private AppState<InGameStateController> _inGameState;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _splashState = new AppState<SplashScreenStateController>(AppConsts.Scenes.Splash);
            _splashState.OnComplete += SplashState_OnComplete;

            _startScreenState = new AppState<StartScreenStateController>(AppConsts.Scenes.StartScreen);
            _startScreenState.OnComplete += StartScreenState_OnComplete;

            _inGameState = new AppState<InGameStateController>(AppConsts.Scenes.Game);
            _inGameState.OnComplete += InGameState_OnComplete;

            _splashState.Enter();
        }

        private void OnDestroy()
        {
            _splashState.OnComplete -= SplashState_OnComplete;
            _startScreenState.OnComplete -= StartScreenState_OnComplete;
            _inGameState.OnComplete -= InGameState_OnComplete;
        }

        private void SplashState_OnComplete(object sender, EventArgs e)
        {
            _splashState.Exit();
            _startScreenState.Enter();
        }

        private void StartScreenState_OnComplete(object sender, EventArgs e)
        {
            _startScreenState.Exit();
            _inGameState.Enter();
        }

        private void InGameState_OnComplete(object sender, EventArgs e)
        {
            _inGameState.Exit();
            _startScreenState.Enter();
        }
    }
}