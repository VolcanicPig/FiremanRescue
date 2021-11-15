using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolcanicPig.Mobile; 

namespace Game
{
    public class GameManager : MobileGameManagerTemplate<GameManager>
    {
        private int _points;
        public int Points => _points;


        public void AddPoints(int points)
        {
            _points += points; 
        }
        
        protected override void SpawnPlayer()
        {
            base.SpawnPlayer();
            CameraController.Instance.SetFollowTarget(GetCurrentPlayer.transform);
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R))
            {
                EndGame(true);
            }
#endif
        }
    }
}
