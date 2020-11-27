using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class AppStateMachine : MonoBehaviour
    {
        [Header("Development")]
        public bool skipSplash;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void Start()
        {
            await EnterSplash();
        }

        private async Task EnterSplash()
        {
            if (!skipSplash)
            {
                var splashState = new AppState<SplashScreenStateController>(AppConsts.Scenes.Splash);
                await splashState.Enter();
                while(!splashState.IsComplete)
                    await Task.Yield();
            }

            await EnterStart();
        }

        private async Task EnterStart()
        {
            var startScreenState = new AppState<StartScreenStateController>(AppConsts.Scenes.StartScreen);
            await startScreenState.Enter();
            while(!startScreenState.IsComplete)
                await Task.Yield();

            await EnterGame();
        }

        private async Task EnterGame()
        {
            var inGameState = new AppState<InGameStateController>(AppConsts.Scenes.Game);
            await inGameState.Enter();
            while (!inGameState.IsComplete)
                await Task.Yield();

            await EnterStart();
        }
    }
}