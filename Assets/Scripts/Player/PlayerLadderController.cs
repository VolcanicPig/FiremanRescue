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
        [SerializeField] private float ladderGrowSpeed, maxLadderYPos, ladderClimbDuration;
        [SerializeField] private LayerMask windowLayer; 
        
        private bool _ladderActive;
        private bool _isUsingLadder;
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
            if (_isUsingLadder) return; 
            transform.forward = Vector3.forward;   
            _ladderActive = true;
            _isUsingLadder = true; 
        }
        
        private void OnTouchUp(Vector2 pos)
        {
            if (IsMovingLadder())
            {
                PlaceLadder();
            }
        }

        public bool IsMovingLadder()
        {
            return ladder.position.y > 0; 
        }

        public void ResetLadder()
        {
            _ladderActive = false;
            _isUsingLadder = false;  
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

        private void PlaceLadder()
        {
            _ladderActive = false;

            RaycastHit hit;
            if (!Physics.Raycast(ladder.position, Vector3.forward, out hit, 5, windowLayer))
            {
                StartCoroutine(CoWaitThenResetLadder());
                return; 
            }

            Window window = hit.collider.GetComponent<Window>();
            
            if(window.WindowActive)
            {
                StartCoroutine(CoClimbLadder());
            }
            else
            {
                StartCoroutine(CoWaitThenResetLadder());
            }
        }

        IEnumerator CoWaitThenResetLadder()
        {
            yield return new WaitForSeconds(1);
            yield return CoResetLadder(); 
        }

        IEnumerator CoClimbLadder()
        {
            Vector3 startPos = transform.position; 
            ladder.SetParent(null);
            yield return CoLerpPlayerPosition(transform.position, ladder.position); 
            yield return new WaitForSeconds(1);
            GameManager.Instance.AddPoints(1);
            yield return CoLerpPlayerPosition(transform.position, startPos); 
            ladder.SetParent(transform);
            yield return CoResetLadder(); 
        }

        IEnumerator CoLerpPlayerPosition(Vector3 fromPos, Vector3 endPos)
        {
            float timer = 0;
            while (timer < ladderClimbDuration)
            {
                transform.position = Vector3.Lerp(fromPos, endPos, timer / ladderClimbDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos; 
        }
        
        IEnumerator CoResetLadder()
        {
            float timer = 0;
            Vector3 startPos = ladder.localPosition; 
            while (timer < ladderClimbDuration)
            {
                ladder.localPosition = Vector3.Lerp(startPos, _ladderStartPos, timer / ladderClimbDuration);
                timer += Time.deltaTime;
                yield return null;
            }

            ladder.localPosition = _ladderStartPos;
            yield return new WaitForSeconds(0.5f);
            _isUsingLadder = false;
            _ladderActive = false; 
        }
    }
}
