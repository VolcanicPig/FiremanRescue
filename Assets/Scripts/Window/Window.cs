using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Window : MonoBehaviour
    {
        private bool _windowActive;
        public bool WindowActive => _windowActive;

        [SerializeField] private Color windowOnColor = Color.white; 
        [SerializeField] private Color windowOffColor = Color.white;
        [SerializeField] private Renderer windowMesh;
        
        private static readonly int Col = Shader.PropertyToID("_BaseColor");

        private void Start()
        {
            windowMesh.material.SetColor(Col, windowOffColor);   
        }

        public void SetWindowActive(bool active)
        {
            _windowActive = active; 
            windowMesh.material.SetColor(Col, active ? windowOnColor : windowOffColor);            
        }
    }
}
