using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Soomla.Profile
{
	public static class ModalDialog
	{
		public static void CreateModalWindow (string dscrpText, UnityEngine.Events.UnityAction call)
		{
			bool myEventSystem = false;
			if (EventSystem.current == null) {
				GameObject eventSystem = new GameObject ("EventSystem");
#if UNITY_4 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
				eventSystem.AddComponent<TouchInputModule> ();
#endif
				eventSystem.AddComponent<StandaloneInputModule> ();
				myEventSystem = true;
			}

			GameObject canvasGO = new GameObject ("ModalWindow");
			RectTransform canvasRT = canvasGO.AddComponent<RectTransform> ();
			Canvas canvasCv = canvasGO.AddComponent<Canvas> ();
			canvasCv.renderMode = RenderMode.ScreenSpaceOverlay;
			canvasGO.AddComponent<UnityEngine.UI.CanvasScaler> ();
			canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster> ();

			GameObject bgRaycastBlocker = new GameObject ("Panel");
			bgRaycastBlocker.transform.SetParent (canvasGO.transform);
			RectTransform bgRaycastBlockerRT = bgRaycastBlocker.AddComponent<RectTransform> ();
			bgRaycastBlockerRT.position = canvasRT.transform.position;
			bgRaycastBlockerRT.sizeDelta = canvasRT.sizeDelta;
			Image bgRaycastBlockerIm = bgRaycastBlocker.AddComponent<Image> ();
			bgRaycastBlockerIm.color = new Color(0.2f,0.2f,0.2f,0.3f);

			GameObject modalWindow = new GameObject ("ModalWindow");
			modalWindow.transform.SetParent (bgRaycastBlocker.transform);
			RectTransform modalWindowRT = modalWindow.AddComponent<RectTransform> ();
			modalWindowRT.localPosition = Vector3.zero;
			modalWindowRT.sizeDelta = new Vector2 (canvasRT.sizeDelta.x / 3, canvasRT.sizeDelta.y / 5);
			Image modalWindowIm = modalWindow.AddComponent<Image> ();
			modalWindowIm.color = new Color(1.0f,1.0f,1.0f,1.0f);

			GameObject titlePanel = new GameObject ("TitlePanel");
			titlePanel.transform.SetParent (modalWindow.transform);
			RectTransform titlePanelRT = titlePanel.AddComponent<RectTransform> ();
			titlePanelRT.anchorMax = new Vector2 (1.0f, 1.0f);
			titlePanelRT.anchorMin = new Vector2 (0.0f, 0.7f);
			titlePanelRT.offsetMax = new Vector2 (0, 0);
			titlePanelRT.offsetMin = new Vector2 (0, 0);
			Image titlePanelIm = titlePanel.AddComponent<Image> ();
			titlePanelIm.color = new Color(0.7f,0.7f,0.7f,1.0f);

			GameObject title = new GameObject ("Title");
			title.transform.SetParent (titlePanel.transform);
			RectTransform titleRT = title.AddComponent<RectTransform> ();
			titleRT.anchorMax = new Vector2 (1.0f, 1.0f);
			titleRT.anchorMin = new Vector2 (0.0f, 0.0f);
			titleRT.offsetMax = new Vector2 (0, 0);
			titleRT.offsetMin = new Vector2 (0, 0);
			Text titleText = title.AddComponent<Text> ();
			titleText.text = "Confirmation";
			titleText.color = Color.black;
			titleText.fontSize = 10;
			titleText.resizeTextForBestFit = true;
			titleText.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
			titleText.alignment = TextAnchor.MiddleCenter;

			GameObject description = new GameObject ("Desscription");
			description.transform.SetParent (modalWindow.transform);
			RectTransform descriptionRT = description.AddComponent<RectTransform> ();
			descriptionRT.anchorMax = new Vector2 (1.0f, 0.7f);
			descriptionRT.anchorMin = new Vector2 (0.0f, 0.4f);
			descriptionRT.offsetMax = new Vector2 (0, 0);
			descriptionRT.offsetMin = new Vector2 (0, 0);
			Text descriptionText = description.AddComponent<Text> ();
			descriptionText.text = dscrpText;
			descriptionText.color = Color.black;
			descriptionText.fontSize = 8;
			descriptionText.resizeTextForBestFit = true;
			descriptionText.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
			descriptionText.alignment = TextAnchor.MiddleCenter;

			GameObject yes = new GameObject ("YesButton");
			yes.transform.SetParent (modalWindow.transform);
			RectTransform yesRT = yes.AddComponent<RectTransform> ();
			yesRT.anchorMax = new Vector2 (1.0f, 0.3f);
			yesRT.anchorMin = new Vector2 (0.51f, 0.0f);
			yesRT.offsetMax = new Vector2 (0, 0);
			yesRT.offsetMin = new Vector2 (0, 0);
			Image yesIm = yes.AddComponent<Image> ();
			yesIm.color = new Color(0.6f,0.6f,0.6f,1.0f);
			Button yesBtn = yes.AddComponent<Button> ();
			yesBtn.onClick.AddListener (call);
			yesBtn.onClick.AddListener (() => GameObject.Destroy (canvasGO));

			GameObject yesText = new GameObject ("YesText");
			yesText.transform.SetParent (yes.transform);
			RectTransform yesTextRT = yesText.AddComponent<RectTransform> ();
			yesTextRT.anchorMax = new Vector2 (1.0f, 1.0f);
			yesTextRT.anchorMin = new Vector2 (0.0f, 0.0f);
			yesTextRT.offsetMax = new Vector2 (0, 0);
			yesTextRT.offsetMin = new Vector2 (0, 0);
			Text yesTextT = yesText.AddComponent<Text> ();
			yesTextT.text = "Post";
			yesTextT.fontSize = 12;
			yesTextT.resizeTextForBestFit = true;
			yesTextT.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
			yesTextT.alignment = TextAnchor.MiddleCenter;
			yesTextT.color = new Color(0.0f,0.2f,0.4f,1.0f);

			GameObject no = new GameObject ("NoButton");
			no.transform.SetParent (modalWindow.transform);
			RectTransform noRT = no.AddComponent<RectTransform> ();
			noRT.anchorMax = new Vector2 (0.49f, 0.3f);
			noRT.anchorMin = new Vector2 (0.0f, 0.0f);
			noRT.offsetMax = new Vector2 (0, 0);
			noRT.offsetMin = new Vector2 (0, 0);
			Image noIm = no.AddComponent<Image> ();
			noIm.color = new Color(0.6f,0.6f,0.6f,1.0f);
			Button noBtn = no.AddComponent<Button> ();
			noBtn.onClick.AddListener (() => GameObject.Destroy (canvasGO));
			if (myEventSystem)
				noBtn.onClick.AddListener (() => GameObject.Destroy (EventSystem.current.gameObject));

			GameObject noText = new GameObject ("NoText");
			noText.transform.SetParent (no.transform);
			RectTransform noTextRT = noText.AddComponent<RectTransform> ();
			noTextRT.anchorMax = new Vector2 (1.0f, 1.0f);
			noTextRT.anchorMin = new Vector2 (0.0f, 0.0f);
			noTextRT.offsetMax = new Vector2 (0, 0);
			noTextRT.offsetMin = new Vector2 (0, 0);
			Text noTextT = noText.AddComponent<Text> ();
			noTextT.text = "Cancel";
			noTextT.fontSize = 12;
			noTextT.resizeTextForBestFit = true;
			noTextT.font = Resources.GetBuiltinResource (typeof(Font), "Arial.ttf") as Font;
			noTextT.alignment = TextAnchor.MiddleCenter;
			noTextT.color = new Color(0.0f,0.2f,0.4f,1.0f);
		}
	}
}
