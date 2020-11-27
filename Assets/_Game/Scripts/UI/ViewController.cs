using System.Threading.Tasks;
using UnityEngine;

namespace Tofunaut.TofuRPG.UI
{
    public abstract class ViewController : MonoBehaviour
    {
        public abstract Task Show();
        public abstract Task Hide();
    }
}