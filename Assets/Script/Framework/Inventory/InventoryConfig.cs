using UnityEngine;
namespace ActGame
{
    public enum InventoryType
    {
        Props,
        Weapon,
    }
    [CreateAssetMenu(fileName = "InventoryConfig", menuName = "SO/Inventory/InventoryConfig", order = 0)]
    public class InventoryConfig : ScriptableObject
    {
        public InventoryType type;
        public uint length;
        public uint width;
    }
}