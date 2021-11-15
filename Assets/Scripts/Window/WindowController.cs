using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class WindowController : MonoBehaviour
    {
        public Window[] windows;

        public void ActivateRandomWindow()
        {
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
