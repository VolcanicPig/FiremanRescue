using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolcanicPig.Mobile.Gestures;

namespace Game
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerSlideMovement : PlayerMovement
    {
        public override void HandleMovement()
        {
            if (AutomatedMovementActive) return;

            Vector3 cachedPosition = transform.position;
            Vector2 touchDelta = _gestures.TouchDelta;

            if (CanMoveSideways)
            {
                cachedPosition.x += touchDelta.x * sideSpeed * Time.deltaTime;
                cachedPosition.x = Mathf.Clamp(cachedPosition.x, -5, 5);
            }

            if (CanMoveForwards)
            {
                cachedPosition.z += forwardsSpeed * Time.deltaTime;
                IsMoving = true;
            }
            else
            {
                IsMoving = false; 
            }

            float step = sideSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cachedPosition, step);
        }
    }
}
