﻿using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class AppContext : SingletonBehaviour<AppContext>
    {
        public static ITofuLogger Log => _instance._log;
        public static TofuVersion AppVersion => _instance._appVersionProvider.Version;
        public static IAppStateMachine StateMachine => _instance._appStateMachine;

        private ITofuLogger _log;
        private ITofuVersionProvider _appVersionProvider;
        private IAppStateMachine _appStateMachine;

        protected override void Awake()
        {
            base.Awake();

            MonoBehaviour[] components = GetComponents<MonoBehaviour>();
            _log = components.OfType<ITofuLogger>().FirstOrDefault();
            _appVersionProvider = components.OfType<ITofuVersionProvider>().FirstOrDefault();
            _appStateMachine = components.OfType<IAppStateMachine>().FirstOrDefault();

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            Log.Info($"TofuRPG {_appVersionProvider.Version} (c) Tofunaut 2020");
        }
    }
}