using Tofunaut.TofuUnity;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Tofunaut.TofuRPG.Game.UI
{
    public class GameUIManager : SingletonBehaviour<GameUIManager>
    {
        private PlayerInput _playerInput;
        
        private void Start()
        {
            _playerInput = FindObjectOfType<PlayerInput>();
        }
    }
}