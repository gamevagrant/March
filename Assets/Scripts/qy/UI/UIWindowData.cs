
namespace qy.ui
{
    public class UIWindowData
    {

        public UISettings.UIWindowID id;
        public UISettings.UIWindowType type;
        public UISettings.UIWindowShowMode showMode = UISettings.UIWindowShowMode.DoNothing;
        public UISettings.UIWindowColliderMode colliderMode = UISettings.UIWindowColliderMode.Normal;
        public UISettings.UIWindowColliderType colliderType = UISettings.UIWindowColliderType.SemiTransparent;
        /// <summary>
        /// 设置UI的排序的先后顺序，数值越大越靠上显示，数值相同的后打开的排在前面
        /// </summary>
        public float siblingNum = 1;
    }
}

