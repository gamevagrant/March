using System;
using System.Collections.Generic;

namespace March.Core.WindowManager
{
    [Serializable]
    public class WindowManagerConfig
    {
        public List<WindowConfig> WindowList = new List<WindowConfig>();

        public Dictionary<string, WindowConfig> WindowMap
        {
            get
            {
                if (windowMap == null)
                {
                    windowMap = new Dictionary<string, WindowConfig>(); 
                }

                windowMap.Clear();
                WindowList.ForEach(window =>
                {
                    windowMap.Add(window.Name, window);
                });
                return windowMap;
            }
        }

        private Dictionary<string, WindowConfig> windowMap;
    }

    [Serializable]
    public class WindowConfig
    {
        public string Name;
        public string Path;
    }
}