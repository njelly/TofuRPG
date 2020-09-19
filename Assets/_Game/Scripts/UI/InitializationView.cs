using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public class InitializationView : MonoBehaviour
    {
        public List<UISplashScreen> splashScreens;

        private int _currentIndex;

        private void OnEnable()
        {
            _currentIndex = -1;
            NextSplashScreen();
        }

        public void NextSplashScreen()
        {
            if (_currentIndex >= 0 && _currentIndex <= splashScreens.Count)
            {

            }

            _currentIndex++;
            for(int i = 0; i < splashScreens.Count; i++)
            {
                splashScreens[i].gameObject.SetActive(i == _currentIndex);
            }
        }
    }
}