using System.Collections.Generic;
using UnityEngine;

namespace DR.ResourceSystem
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] protected float regrowthTime = 60f;
        [SerializeField] protected float checkRepeatTime = 60f;
        protected Dictionary<GameObject, float> resourceUsedList = new Dictionary<GameObject, float>();
        protected List<GameObject> tempList = new List<GameObject>();

        public AudioClip soundFX;
        public ParticleSystem effectFX;
        private void Start()
        {
            InvokeRepeating(nameof(CheckRegrowth), 0f, checkRepeatTime);
        }
        
        public void UsedResource(GameObject obj)
        {
            if (!resourceUsedList.ContainsKey(obj))
            {
                resourceUsedList.Add(obj, 0f);
            }
            resourceUsedList[obj] = Time.time;
            obj.SetActive(false);
        }
        
        private void CheckRegrowth()
        {
            float currentTime = Time.time;
            
            if(resourceUsedList.Count == 0) return;
            
            foreach (KeyValuePair<GameObject, float> resource in resourceUsedList)
            {
                float time = resource.Value;

                if (currentTime - time >= regrowthTime)
                {
                    resource.Key.SetActive(true);
                    tempList.Add(resource.Key);
                }
            }
            
            if(tempList.Count == 0) return;
            foreach (var obj in tempList)
            {
                resourceUsedList.Remove(obj);
            }
            tempList.Clear();
        }
    }
}