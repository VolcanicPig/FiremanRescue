using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VolcanicPig.Mobile;
using Random = UnityEngine.Random;

namespace Game
{
    public class WindowController : MonoBehaviour
    {
        public Window[] windows;
        [SerializeField] private float minActivateTime, maxActivateTime;

        private float _lastActivateTime;
        private float _currentCooldown;

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            GameManager.OnGameStateChanged -= OnGameStateChanged;
        }

        private void Update()
        {
            if (GameManager.Instance.GetGameState != GameState.InGame) return;
            if (Time.time > _lastActivateTime + _currentCooldown)
            {
                ActivateRandomWindow();
            }
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.Start)
            {
                ActivateRandomWindow();
            }
        }

        public void ActivateRandomWindow()
        {
            _lastActivateTime = Time.time;
            _currentCooldown = Random.Range(minActivateTime, maxActivateTime); 
            
            Window window = GetRandomInactiveWindow();
            window.SetWindowActive(true);
        }

        private Window GetRandomInactiveWindow()
        {
            List<Window> inactiveWindows = new List<Window>(windows.Where(w => !w.WindowActive));

            if (inactiveWindows.Count == 0) return windows[0];

            int rand = Random.Range(0, inactiveWindows.Count);
            return inactiveWindows[rand]; 
        }
    }
}
