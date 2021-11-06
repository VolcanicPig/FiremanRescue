using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolcanicPig.Mobile.Gestures;

namespace Game
{
    public class PlayerSwipeMovement : PlayerMovement
    {
        [SerializeField] private float sideSpacing, sideDuration;         
        
        private bool _canSwipe = true; 
        
        private void OnEnable()
        {
            GestureController.OnSwipe += OnSwipe; 
        }

        private void OnDisable()
        {
            GestureController.OnSwipe -= OnSwipe;
        }

        private void OnSwipe(Vector2 swipeDir)
        {
            if (!CanMoveSideways) return;
            if (!_canSwipe) return;

            Transform t = transform;
            Vector3 position = t.position;
            
            Vector3 endPos = position + Vector3.right * swipeDir.x * sideSpacing;

            if (IsInvalidMove(endPos)) return; 

            Vector3 relativePos = endPos - position;
            
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.rotation = rotation;
            
            StartCoroutine(CoMoveToPosition(endPos)); 
        }

        IEnumerator CoMoveToPosition(Vector3 endPos)
        {
            _canSwipe = false;
            Vector3 startPos = transform.position;
            float timer = 0; 
            while (timer < sideDuration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, timer / sideDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
            _canSwipe = true; 
        }

        private bool IsInvalidMove(Vector3 endPos)
        {
            Level currLevel = LevelManager.Instance.GetCurrentLevelObj;
            if (endPos.x > currLevel.maxXPos || endPos.x < currLevel.minXPos)
            {
                return true; 
            }

            return false; 
        }

        public override void HandleMovement() { }
    }
}
