using System;
using Tofunaut.TofuRPG;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.TofuUnity
{
    public class AppState<T> where T : AppStateController
    {
        public event EventHandler OnComplete;

        private readonly LogService _log;
        private readonly int _sceneIndex;

        private T _stateController;

        public AppState(LogService log, int sceneIndex)
        {
            _log = log;
            _sceneIndex = sceneIndex;
        }

        public void Enter()
        {
            SceneManager.LoadScene(_sceneIndex);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.buildIndex != _sceneIndex)
                return;

            _stateController = UnityEngine.Object.FindObjectOfType<T>();
            _stateController.OnComplete.AddListener(() => { OnComplete?.Invoke(this, EventArgs.Empty); });
            _stateController.log = _log;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            if (scene.buildIndex != _sceneIndex || !_stateController)
                return;

            _stateController.OnComplete.RemoveAllListeners();
        }
    }
}