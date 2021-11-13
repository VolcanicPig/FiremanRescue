using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolcanicPig.Mobile.Gestures;

namespace Game
{
    public class PlayerLadderController : MonoBehaviour
    {
        [SerializeField] private Transform ladder;
        [SerializeField] private float ladderGrowSpeed, maxLadderYPos; 
        
        private bool _ladderActive;
        private Vector3 _ladderStartPos; 
        
        private void OnEnable()
        {
            GestureController.OnTouchDown += OnTouchDown;
            GestureController.OnTouchUp += OnTouchUp;
        }

        private void OnDisable()
        {
            GestureController.OnTouchUp -= OnTouchUp;
            GestureController.OnTouchDown -= OnTouchDown;
        }

        private void Start()
        {
            _ladderStartPos = ladder.localPosition; 
        }

        private void OnTouchDown(Vector2 pos)
        {
            transform.forward = Vector3.forward;   
            _ladderActive = true;
        }
        
        private void OnTouchUp(Vector2 pos)
        {
            ResetLadder();
        }

        public bool IsMovingLadder()
        {
            return ladder.position.y > 0; 
        }

        public void ResetLadder()
        {
            _ladderActive = false;
            ladder.localPosition = _ladderStartPos; 
        }

        private void Update()
        {
            if (_ladderActive)
            {
                ladder.localPosition += Vector3.up * ladderGrowSpeed * Time.deltaTime;

                if (ladder.localPosition.y >= maxLadderYPos)
                {
                    ResetLadder();
                }
            }
        }
    }
}
