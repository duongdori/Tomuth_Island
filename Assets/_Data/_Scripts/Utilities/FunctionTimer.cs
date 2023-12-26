using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DR.Utilities
{
    public class FunctionTimer
    {
        private static List<FunctionTimer> _activeTimerList;
        private static GameObject _initGameObj;

        private static void InitIfNeeded()
        {
            if (_initGameObj == null)
            {
                _initGameObj = new GameObject("FunctionTimer_InitGameObj");
                _activeTimerList = new List<FunctionTimer>();
            }
        }
        public static FunctionTimer Create(UnityAction action, float timer, string timerName = null)
        {
            InitIfNeeded();
            GameObject gameObj = new GameObject("FunctionTimer", typeof(MonoBehaviourHook));
            FunctionTimer functionTimer = new FunctionTimer(action, timer, timerName, gameObj);
            gameObj.GetComponent<MonoBehaviourHook>().OnUpdate = functionTimer.Update;
            
            _activeTimerList.Add(functionTimer);
            
            return functionTimer;
        }

        private static void RemoveTimer(FunctionTimer functionTimer)
        {
            InitIfNeeded();
            _activeTimerList.Remove(functionTimer);
        }

        private static void StopTimer(string timerName)
        {
            for (int i = 0; i < _activeTimerList.Count; i++)
            {
                if (_activeTimerList[i]._timerName == timerName)
                {
                    _activeTimerList[i].DestroySelf();
                    i--;
                }
            }
        }
        public class MonoBehaviourHook : MonoBehaviour
        {
            public UnityAction OnUpdate;

            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }
        
        private UnityAction _action;
        private float _timer;
        private string _timerName;
        private GameObject _gameObj;
        private bool _isDestroyed;

        private FunctionTimer(UnityAction action, float timer, string timerName, GameObject gameObj)
        {
            _action = action;
            _timer = timer;
            _timerName = timerName;
            _gameObj = gameObj;
            _isDestroyed = false;
        }

        private void Update()
        {
            if(_isDestroyed) return;
            
            _timer -= Time.deltaTime;
            if (_timer < 0)
            {
                _action?.Invoke();
                DestroySelf();
            }
        }

        public float GetTimer()
        {
            return _timer;
        }

        private void DestroySelf()
        {
            _isDestroyed = true;
            Object.Destroy(_gameObj);
            RemoveTimer(this);
        }
    }
}