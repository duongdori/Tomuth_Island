using DR.InventorySystem;
using UnityEngine;

namespace DR.CombatSystem.Weapons
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon()
        {
            if(currentWeaponModel == null) return;
            currentWeaponModel.SetActive(false);
        }

        public void UnloadWeaponAndDestroy()
        {
            if (currentWeaponModel == null) return;
            Destroy(currentWeaponModel);
        }
    
        public void LoadWeaponModel(ItemData weaponItem)
        {
            UnloadWeaponAndDestroy();
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.itemPrefab);
        
            model.transform.parent = parentOverride != null ? parentOverride : transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            currentWeaponModel = model;
        }
    }
}
