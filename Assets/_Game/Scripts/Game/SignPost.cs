using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    public class SignPost : MonoBehaviour, Interactor.IInteractable
    {
        [SerializeField] private DialogLineAsset _dialogLine;
        [SerializeField] private UIDialog _dialogPrefab;

        public void BeginInteraction(Interactor interactor)
        {
            DialogLine line = _dialogLine.GetDialogLine();
            line.source = gameObject;

            UIDialogManager.Queue(_dialogPrefab, line);
        }

        public void EndInteraction(Interactor interactor)
        {
        }
    }
}