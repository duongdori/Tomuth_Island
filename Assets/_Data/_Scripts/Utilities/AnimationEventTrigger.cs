using DR.CombatSystem.Weapons;
using DR.SoundSystem;
using UnityEngine;

namespace DR.Utilities
{
    public class AnimationEventTrigger : MonoBehaviour
    {
        [SerializeField] private WeaponHandler weaponHandler;

        public void EnableWeapon()
        {
            weaponHandler.EnableWeapon();
        }
    
        public void DisableWeapon()
        {
            weaponHandler.DisableWeapon();
        }

        public void AnimationFinishTrigger()
        {
        }
    
        public void AnimationTrigger()
        {
        }

        public void FootStepFirstTrigger()
        {
            SoundManager.Instance.PlaySfx(Sound.FootStep01);
        }
        public void FootStepSecondTrigger()
        {
            SoundManager.Instance.PlaySfx(Sound.FootStep02);
        }

        public void EnableWeaponOnHand()
        {
        }
    
        public void DisableWeaponOnHand()
        {
        }
    }
}
