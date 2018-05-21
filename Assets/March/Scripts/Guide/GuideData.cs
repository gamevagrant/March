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
        public Position Direction;
        public bool Show;
    }
}