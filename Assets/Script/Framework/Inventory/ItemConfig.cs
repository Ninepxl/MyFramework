using System.Collections.Generic;
using UnityEngine;

namespace ActGame
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "SO/Inventory/ItemConfig", order = 0)]
    public class ItemSo : ScriptableObject
    {
        public InventoryType type;
        public bool canUse = true;
    }

    [CreateAssetMenu(fileName = "ItemList", menuName = "SO/Inventory/ItemList", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public Dictionary<int, ItemSo> items;
    }
}