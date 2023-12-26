using System.Collections.Generic;
using DR.InventorySystem;
using DR.SoundSystem;
using UnityEngine;

namespace DR.ResourceSystem
{
    public class ResourceHolder : MonoBehaviour
    {
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private InventorySlot holder;
        [SerializeField] private ResourceStats stats;
        public List<ItemData> toolsNeeded;
        [SerializeField] private GameItem gameItemPrefab;

        public ResourceManager ResourceManager => resourceManager;
        private void Start()
        {
            resourceManager = GetComponentInParent<ResourceManager>();
            stats.HealthSystem.OnDie += OnDie;
            stats.HealthSystem.OnTakeDamage += OnTakeDamage;
        }

        private void OnEnable()
        {
            stats.HealthSystem.OnDie += OnDie;
            stats.HealthSystem.ResetHealth();
        }

        private void OnDisable()
        {
            stats.HealthSystem.OnDie -= OnDie;
            stats.HealthSystem.OnTakeDamage -= OnTakeDamage;
        }

        private void OnTakeDamage()
        {
            SoundManager.Instance.PlaySfx(resourceManager.soundFX);
        }
        private void OnDie()
        {
            DropResourceOnGround();
            resourceManager.UsedResource(gameObject);
        }
        
        private void DropResourceOnGround()
        {
            Vector3 pos = transform.position + Vector3.left * 2f + Vector3.up * 2f;
            GameItem newItem = Instantiate(gameItemPrefab, pos, Quaternion.identity);
            //newItem.SetSlot(holder);
            newItem.ItemSlot.UpdateGameItem(holder.ItemData, holder.StackSize, holder.CurrentDurability);
        }
    }
}

