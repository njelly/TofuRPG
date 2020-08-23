using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, Actor.IInteractable
    {
        [SerializeField] private DialogLineAsset _dialogLine;
        [SerializeField] private UIDialog _dialogPrefab;

        public void BeginInteraction(Actor actor)
        {
            DialogLine line = _dialogLine.GetDialogLine();
            line.source = gameObject;

            UIDialogManager.Queue(_dialogPrefab, line);
        }

        public void EndInteraction(Actor actor)
        {
        }
    }
}