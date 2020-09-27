using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tofunaut.TofuRPG
{
    public class InGameState : MonoBehaviour, IInGameState
    {
        private void OnEnable()
        {
            if (SceneManager.GetActiveScene().buildIndex != AppConsts.Scene.Game)
            {
                SceneManager.LoadScene(AppConsts.Scene.Game);
            }
        }
    }
}