using UnityEngine;

namespace DR.CombatSystem
{
    public class BackStabCollider : MyMonobehaviour
    {
        public Collider backStabBoxCollider;
        public Transform backStabberStandPoint;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            //LoadBackStabBoxCollider();
        }

        private void LoadBackStabBoxCollider()
        {
            if(backStabBoxCollider != null) return;
            backStabBoxCollider = GetComponent<Collider>();
            Debug.LogWarning(transform.name + ": LoadBackStabBoxCollider", gameObject);
        }
    }
}