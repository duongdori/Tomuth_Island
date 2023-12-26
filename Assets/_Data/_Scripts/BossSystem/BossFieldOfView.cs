using System;
using DR.PlayerSystem;
using DR.SoundSystem;
using UnityEngine;

namespace DR.BossSystem
{
    public class BossFieldOfView : MonoBehaviour
    {
        public BossHealthBar bossHealthBar;
        public Player player;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                this.player = player;
                bossHealthBar.gameObject.SetActive(true);
                SoundManager.Instance.PlayMusic(Sound.BossMusic);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                this.player = null;
                bossHealthBar.gameObject.SetActive(false);
                SoundManager.Instance.PlayMusic(Sound.DungeonMusic);
            }
        }
    }
}