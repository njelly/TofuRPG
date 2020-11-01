using System;
using Tofunaut.TofuUnity;
using Tofunaut.TofuUnity.Interfaces;
using UnityEngine;
using Version = Tofunaut.TofuUnity.Version;

namespace Tofunaut.TofuRPG
{
    [CreateAssetMenu(fileName = "AppVersionProviderAsset", menuName = "TofuRPG/DataAsset/AppVersionProviderAsset", order = 1)]
    public class AppVersionProviderAsset : ScriptableObject, IVersionProvider
    {
        public Version Version =>
            _version ?? (_version =
                new Version($"{Application.version}{Version.Delimiter}{BuildNumberUtil.ReadBuildNumber()}"));

        private Version _version;
    }
}