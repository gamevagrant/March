using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Grow;

[CustomEditor(typeof(GrowEditorScript))]
public class GrowSettingsEditor : Editor
{

	public void OnEnable()
	{
		GrowEditorScript.OnEnable();
	}

	public override void OnInspectorGUI()
	{

		GrowEditorScript.OnInspectorGUI();
	}

	public void OnDisable() {
		// NOTE: nothing to do here..
	}

}
