using UnityEngine;

namespace DR.CombatSystem.Weapons
{
    public class WeaponHandler : MonoBehaviour
    {
        [SerializeField] private GameObject weaponLogic;

        public void EnableWeapon()
        {
            weaponLogic.SetActive(true);
        }
    
        public void DisableWeapon()
        {
            weaponLogic.SetActive(false);
        }

    
    }
}
