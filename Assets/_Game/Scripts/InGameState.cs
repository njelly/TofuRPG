using Tofunaut.TofuRPG.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.TofuRPG
{
    public class InGameState : MonoBehaviour, IInGameState
    {
        public GameContext gameContextPrefab;

        private GameContext _instantiatedGameContext;

        private void OnEnable()
        {
            _instantiatedGameContext = Instantiate(gameContextPrefab, transform);
            if (SceneManager.GetActiveScene().buildIndex != AppConsts.Scene.Game)
            {
                SceneManager.LoadScene(AppConsts.Scene.Game);
            }
        }

        private void OnDisable()
        {
            Destroy(_instantiatedGameContext.gameObject);
        }
    }
}