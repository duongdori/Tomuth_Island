using System.Collections.Generic;
using DR.CombatSystem;
using Animancer;
using DR.Utilities;
using UnityEngine;

namespace DR.InventorySystem
{
    [CreateAssetMenu(menuName = "SO/Items/Weapons")]
    public class WeaponItemData : ItemData
    {
        public List<CustomClipTransition> attackAnimations;

        public List<Attack> attackList;
    }
}