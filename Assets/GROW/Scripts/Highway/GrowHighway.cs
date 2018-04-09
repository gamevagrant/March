/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Linq;
using Grow.Singletons;
using System.Reflection;
using Grow.Integrations;

namespace Grow.Highway
{
	public class GrowHighway : CodeGeneratedSingleton
	{
#if UNITY_IOS && !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern int growHighway_initialize(string gameKey, string envKey, bool debug);
		[DllImport("__Internal")]
		private static extern int growHighway_sendEvent(string eventJson);
		[DllImport("__Internal")]
		private static extern int growHighway_setIntegrationVersion(string ivJson);
		[DllImport("__Internal")]
		private static extern int growHighway_initNative(string initJson);
#endif

		protected override bool DontDestroySingleton { get { return true; }	}

		protected override void InitAfterRegisteringAsSingleInstance() {
			base.InitAfterRegisteringAsSingleInstance();
		}

		public static void Initialize() {
			Type coreType = Type.GetType ("Soomla.CoreEvents");
			if (coreType != null) {
				coreType.GetMethod ("Initialize", BindingFlags.Static | BindingFlags.Public).Invoke (null, null);
			}

			HighwayEvents.Initialize();
			GetSynchronousCodeGeneratedInstance<GrowHighway>();
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);

			using(AndroidJavaClass jniStoreConfigClass = new AndroidJavaClass("com.soomla.highway.HighwayConfig")) {
				jniStoreConfigClass.SetStatic("logDebug", DEBUG);
			}

			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#endif
			GrowUtils.LogDebug (TAG, "GROW/UNITY Initializing Highway");
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
			InitIntegrations (true);
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniGrowHighwayClass = new AndroidJavaClass("com.soomla.highway.GrowHighway")) {

				AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject jniGrowHighwayInstance = jniGrowHighwayClass.CallStatic<AndroidJavaObject>("getInstance");
				jniGrowHighwayInstance.Call("initialize", currentActivity, GrowEditorScript.HighwayGameKey, GrowEditorScript.HighwayEnvKey);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			growHighway_initialize(GrowEditorScript.HighwayGameKey, GrowEditorScript.HighwayEnvKey, DEBUG);
#else
			GrowUtils.LogWarning(TAG, "Highway only works on Android or iOS devices.");
#endif
		}

        void OnLevelWasLoaded(int level) {
			InitIntegrations (false);
        }

		private static void InitIntegrations(bool firstInit) {
			// Get all integrations and initialize them
			var integrations =
				from assembly in AppDomain.CurrentDomain.GetAssemblies ()
					from type in assembly.GetTypes ()
					where type.IsSubclassOf (typeof(GrowIntegration))
					select type;

			GrowUtils.LogDebug (TAG, "Initializing all classes implementing GrowIntegrations");
			foreach (var integration in integrations) {
				if (integration.IsAbstract) {
					continue;
				}
				GrowIntegration integrationInstance = integration.GetProperty ("Instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue (null, null) as GrowIntegration;
				integrationInstance.Initialize ();
				if (firstInit) {
					JSONObject ivJson = new JSONObject ();
					ivJson.SetField ("integration", integrationInstance.GetIntegrationName());
					ivJson.SetField ("version", integrationInstance.GetIntegrationVersion());
#if UNITY_ANDROID && !UNITY_EDITOR
					AndroidJNI.PushLocalFrame(100);
					using(AndroidJavaClass jniGrowHighwayClass = new AndroidJavaClass("com.soomla.highway.unity.GrowHighway")) {
						jniGrowHighwayClass.CallStatic("setIntegrationVersion", ivJson.ToString());
					}
					AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
					growHighway_setIntegrationVersion(ivJson.ToString());
#endif
				}
			}
		}

		private static void SendEventToHw(string eventJson) {
			#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniGrowHighwayClass = new AndroidJavaClass("com.soomla.highway.unity.GrowHighway")) {

			jniGrowHighwayClass.CallStatic("sendEvent", eventJson);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
			#elif UNITY_IOS && !UNITY_EDITOR
			growHighway_sendEvent(eventJson);
			#endif
		}

		public void SendEvent(string eventJson) {
			GrowHighway.SendEventToHw (eventJson);
		}

		private static void InitNativeLibs(string initJson) {
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniGrowHighwayClass = new AndroidJavaClass("com.soomla.highway.unity.GrowHighway")) {

				jniGrowHighwayClass.CallStatic("initNative", initJson);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
#elif UNITY_IOS && !UNITY_EDITOR
			growHighway_initNative(initJson);
#endif
		}

		public void InitNative(string initJson) {
			GrowHighway.InitNativeLibs (initJson);
		}

		private const string TAG = "GROW Highway";
		private const bool DEBUG = false;

	}
}
