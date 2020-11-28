using System.Threading.Tasks;
using UnityEngine;

namespace Tofunaut.TofuRPG.UI
{
    public abstract class ViewController : MonoBehaviour
    {
        public bool IsShowing { get; private set; }

        public virtual void Show()
        {
            IsShowing = true;
        }

        public virtual void Hide()
        {
            IsShowing = false;
        }
    }
}