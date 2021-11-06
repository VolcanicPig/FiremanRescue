using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolcanicPig.Mobile.Gestures;

namespace Game
{
    public class PlayerMovement : MonoBehaviour
    {
        public bool Moving => IsMoving;

        [SerializeField] protected float deltaMultiplier, forwardsSpeed, sideSpeed, minYRot, maxYRot;

        protected GestureController _gestures; 
        protected Player _player;
        protected Rigidbody _rb;

        protected bool CanMoveForwards;
        protected bool CanMoveSideways;
        protected bool AutomatedMovementActive;
        protected bool IsMoving; 

        private void Start()
        {
            _gestures = GestureController.Instance; 
            _rb = GetComponent<Rigidbody>();
            _player = GetComponent<Player>();
        }

        public void SetMovementEnabled(bool enabled)
        {
            CanMoveForwards = enabled;
            CanMoveSideways = enabled;
        }

        public virtual void Update()
        {
            if (_player.State != PlayerState.Moving)
            {
                _rb.angularVelocity = Vector3.zero;
                return;
            }

            HandleMovement();
        }

        private float _yRot = 0;

        public virtual void HandleMovement()
        {
            if (AutomatedMovementActive) return;

            if (CanMoveSideways)
            {
                Vector2 axis = _gestures.ScaledTouchDelta * deltaMultiplier;
                _yRot += axis.x * sideSpeed * Time.deltaTime;
                _yRot = Mathf.Clamp(_yRot, minYRot, maxYRot);

                transform.rotation = Quaternion.Euler(0, _yRot, 0);
            }
            else
            {
                if(_rb) _rb.angularVelocity = Vector3.zero;
            }

            if (CanMoveForwards)
            {
                transform.position += transform.forward * forwardsSpeed * Time.deltaTime;
                IsMoving = true; 
            }
            else
            {
                IsMoving = false;  
            }
        }

        public void AutomatedMovementToPosition(Transform target, Action onComplete)
        {
            AutomatedMovementActive = true;
            _yRot = 0;
            StartCoroutine(CoAutomatedMovement(target, onComplete));
        }

        private IEnumerator CoAutomatedMovement(Transform target, Action onComplete)
        {
            while (transform.position != target.position)
            {
                float step = forwardsSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
                transform.LookAt(target);
                yield return null;
            }

            transform.position = target.position;
            transform.rotation = target.rotation; 
            
            AutomatedMovementActive = false;
            if (onComplete != null) onComplete?.Invoke();
        }
    }
}
