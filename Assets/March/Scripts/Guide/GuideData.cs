using March.Scene;
using System;
using System.Collections.Generic;

namespace March.Core.Guide
{
    [Serializable]
    public class GuideData
    {
        public string Name;
        public List<GuideItem> ItemList;

        public List<GuideCondition> ConditionList;
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
    public class GuideCondition
    {
        public int NodeIndex;
        public SWAP_DIRECTION Direction;
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

    [Serializable]
    public class GuideLevelData
    {
        public int Level;
        public int Step;

        public string GuideDataPath
        {
            get { return string.Format("Guide_{0}_{1}", Level, Step); }
        }
    }

    [Serializable]
    public class GuideLevelManagerData
    {
        public List<GuideLevelData> GuideLevelList;

        public Dictionary<int, List<GuideLevelData>> GuideManagerDict
        {
            get
            {
                if (guideManagerDict == null)
                {
                    guideManagerDict = new Dictionary<int, List<GuideLevelData>>();
                    GuideLevelList.ForEach(guideLevel =>
                    {
                        if (!guideManagerDict.ContainsKey(guideLevel.Level))
                            guideManagerDict.Add(guideLevel.Level, new List<GuideLevelData>());

                        guideManagerDict[guideLevel.Level].Add(guideLevel);
                    });
                }
                return guideManagerDict;
            }
        }

        private Dictionary<int, List<GuideLevelData>> guideManagerDict;

        public int CurrentLevel { get; set; }
        /// <summary>
        /// Current step which is one based.
        /// </summary>
        public int CurrentStep { get; set; }

        public GuideData CurrentGuideData { get; set; }

        public GuideLevelData CurrentGuideLevelData
        {
            get
            {
                if (CurrentStep - 1 >= 0 && CurrentStep - 1 < GuideManagerDict[CurrentLevel].Count)
                    return GuideManagerDict[CurrentLevel][CurrentStep - 1];
                return null;
            }
        }

        public bool IsFinalStep
        {
            get { return CurrentStep == GuideManagerDict[CurrentLevel].Count; }
        }

        public void Reset()
        {
            CurrentStep = 1;
        }

        public GuideLevelManagerData()
        {
            CurrentStep = 1;
            CurrentLevel = 1;
        }
    }
}