using UnityEngine;

namespace Tofunaut.TofuRPG.Game
{
    [CreateAssetMenu(fileName = "new ItemSpec", menuName = "TofuRPG/ItemSpec")]
    public class ItemSpec : ScriptableObject
    {
        public string displayName;
        public Sprite itemSprite;
    }
}