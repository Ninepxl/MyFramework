using System;
using UnityEngine;
namespace ActGame
{
    [Serializable]
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