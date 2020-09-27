using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class StartScreenRootView : MonoBehaviour
    {
        public interface IListener
        {
            void StartScreenRootView_OnPlayClicked();
            void StartScreenRootView_OnQuitClicked();
        }

        public IListener listener;

        public void OnPlayClicked()
        {
            listener?.StartScreenRootView_OnPlayClicked();
        }

        public void OnQuitClicked()
        {
            listener?.StartScreenRootView_OnQuitClicked();
        }
    }
}