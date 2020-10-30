using System.Linq;
using Tofunaut.TofuUnity;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class AppContext : SingletonBehaviour<AppContext>
    {
        private IVersionProvider _versionProvider;
        private LogService _log;
        private AppStateMachine _appStateMachine;

        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);

            var components = GetComponents<MonoBehaviour>();

            _log = new LogService();
            _log.Register(new UnityLogger());
            _log.Register(components.OfType<UnityGUILogger>().FirstOrDefault());

            _versionProvider = components.OfType<IVersionProvider>().FirstOrDefault();
            _log.Info($"TofuRPG {_versionProvider.Version} (c) Tofunaut 2020");

            _appStateMachine = components.OfType<AppStateMachine>().FirstOrDefault();
            _appStateMachine.log = _log;
        }
    }
}