using System;
using UnityEngine.UI;

namespace qy.ui
{
    public class Alert
    {

        public const uint OK = 1;
        public const uint CANCEL = 2;

        /// <summary>
        /// 打开一个有确认和取消按钮的模态窗口 
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="flags">控件中放置的按钮。有效值为 Alert.OK、Alert.CANCEL默认值为 Alert.OK。使用按位 OR 运算符可显示多个按钮。例如，传递 (Alert.OK | Alert.CANCEL) 显示“确认”和“取消”按钮。无论按怎样的顺序指定按钮，它们始终按照以下顺序从左到右显示：“确定”，“取消”。
        /// <param name="onClickBtn">点击按钮的回调，Alert.OK 或者 Alert.CANCEL的值</param>
        /// <param name="okBtnName"></param>
        /// <param name="cancelBtnName"></param>
        public static void Show(string content, uint flags = Alert.OK, Action<uint> onClickBtn = null, string okBtnName = "", string cancelBtnName = "", UnityEngine.TextAnchor alignment = UnityEngine.TextAnchor.MiddleCenter)
        {
            ModalBoxData data = new ModalBoxData();
            data.content = content;
            data.flags = flags;
            data.okName = okBtnName;
            data.cancelName = cancelBtnName;
            data.onClick = onClickBtn;
            data.alignment = alignment;
            GameMainManager.Instance.uiManager.OpenWindow(UISettings.UIWindowID.UIModalBoxWindow, data);

        }
    }

    public class ModalBoxData
    {
        public string content;
        public uint flags = Alert.OK;
        public string okName = "确认";
        public string cancelName = "取消";
        public Action<uint> onClick = null;
        public UnityEngine.TextAnchor alignment = UnityEngine.TextAnchor.MiddleCenter;//对齐
    }

}
