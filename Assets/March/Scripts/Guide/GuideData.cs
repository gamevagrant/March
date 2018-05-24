using March.Scene;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace March.Core.Guide
{
    [Serializable]
    public class GuideData
    {
        public string Name;
        public List<GuideItem> ItemList;

        public GuideWindowData Window;
        public GuideHandData Hand;
    }

    [Serializable]
    public class GuideItem
    {
        public string ObjectName;
        public Position AnchorMin;
        public Position AnchorMax;
        public Position Pivot;
        public Position AnchorPosition;
        public Position Size;
    }

    [Serializable]
    public class GuideWindowData
    {
        public Position Position;
        public string Content;
        public string NextButtonContent;
        public bool HasNextButton;
        public bool HasHead;
    }

    [Serializable]
    public class GuideHandData
    {
        public Position Position;
        /// <summary>
        /// Direction of hand animation.
        /// </summary>
        /// <remarks>Base unit as guide hand image size.</remarks>
        public Position Direction;
        public float Duration;
        public bool Show;
    }
}