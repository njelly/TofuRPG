using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class InitializationView : MonoBehaviour
    {
        public interface IListener
        {
            void OnCompleteSplashScreens();
        }

        public IListener listener;
        public List<UISplashScreen> splashScreens;

        private int _currentIndex;

        private void OnEnable()
        {
            _currentIndex = -1;
            NextSplashScreen();
        }

        public void NextSplashScreen()
        {
            if(_currentIndex >= 0 && _currentIndex < splashScreens.Count)
            {
                if(!splashScreens[_currentIndex].canSkip)
                {
                    return;
                }
            }

            _currentIndex++;
            for(int i = 0; i < splashScreens.Count; i++)
            {
                splashScreens[i].gameObject.SetActive(i == _currentIndex);
            }

            if(_currentIndex >= splashScreens.Count)
            {
                listener?.OnCompleteSplashScreens();
            }
        }
    }
}