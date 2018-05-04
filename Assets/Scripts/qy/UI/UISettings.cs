using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace qy.ui
{
    public class UISettings
    {

        public enum UIWindowID
        {
            UIDialogueWindow,
            UITaskWindow,
            UIRoleWindow,
            UIEndingWindow,
            UIModalBoxWindow,
            UICallBackWindow,
            UIMainSceneWindow,
            UINickNameWindow,
        }

        public enum UIWindowType
        {
            //Normal,    // 可推出界面(UIMainMenu,UIRank等)
            Fixed,     // 固定窗口(UITopBar等)
            PopUp,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
            Cover,      //覆盖效果
        }

        public enum UIWindowColliderMode
        {
            Normal,    // 背景板阻挡点击事件
            TouchClose,    // 点击背景板关闭面板
        }

        public enum UIWindowColliderType
        {
            Transparent,//透明的
            SemiTransparent,//半透
        }

        public enum UIWindowShowMode
        {
            DoNothing = 0,
            HideOtherWindow,
            DestoryOtherWindow,
        }


        // Main folder
        //public static string UIPrefabPath = "UIPrefab/";
        private static Dictionary<UIWindowID, string> windowPrefabPath = new Dictionary<UIWindowID, string>()
        {
            {UIWindowID.UIDialogueWindow,"UIDialogueWindow"},
            {UIWindowID.UITaskWindow,"UITaskWindow"},
            {UIWindowID.UIRoleWindow,"UIRoleWindow"},
            {UIWindowID.UIEndingWindow,"UIEndingWindow"},
            {UIWindowID.UIModalBoxWindow,"UIModalBoxWindow"},
            {UIWindowID.UICallBackWindow,"UICallBackWindow"},
            {UIWindowID.UIMainSceneWindow,"UIMainSceneWindow"},
            {UIWindowID.UINickNameWindow,"UINickNameWindow"},
        };



        public static string GetWindowName(UIWindowID id)
        {
            string name;
            windowPrefabPath.TryGetValue(id, out name);
            return name;
        }

    }
}

