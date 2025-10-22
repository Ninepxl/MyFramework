using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;

namespace ActGame
{
    [Serializable]
    public class ItemSo : ScriptableObject
    {
        public InventoryType type;
        public bool canUse = true;
    }

    [CreateAssetMenu(fileName = "ItemList", menuName = "SO/Inventory/ItemList", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        // [OdinSerialize]
        public List< ItemSo> items = new();
    }
}