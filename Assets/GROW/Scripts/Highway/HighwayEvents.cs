/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

using Grow.Insights;
using Grow.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Grow.Highway {

	public class HighwayEvents : CodeGeneratedSingleton {

		#if UNITY_IOS
		//&& !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern int unityHighwayEventDispatcher_Init();
		#endif

		private const string TAG = "SOOMLA HighwayEvents";

		public static HighwayEvents Instance = null;

		protected override bool DontDestroySingleton
		{
			get { return true; }
		}

		/// <summary>
		/// Initializes the different native event handlers in Android / iOS
		/// </summary>
		public static void Initialize() {
			if (Instance == null) {
				Instance = GetSynchronousCodeGeneratedInstance<HighwayEvents>();

				GrowUtils.LogDebug (TAG, "Initializing HighwayEvents...");
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.highway.unity.HighwayEventHandler")) {
					jniEventHandler.CallStatic("initialize");
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);

#elif UNITY_IOS && !UNITY_EDITOR
				unityHighwayEventDispatcher_Init();
#endif
			}
		}

		public void onGrowInsightsInitialized(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onGrowInsightsInitialized");

			GrowInsights.I_SyncWithNative ();

			HighwayEvents.OnGrowInsightsInitialized(new GrowInsightsInitializedEvent());
		}

		public void onInsightsRefreshFailed(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onInsightsRefreshFailed");

			HighwayEvents.OnInsightsRefreshFailed(new InsightsRefreshFailedEvent());
		}

		public void onInsightsRefreshStarted(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onInsightsRefreshStarted");

			HighwayEvents.OnInsightsRefreshStarted(new InsightsRefreshStartedEvent());
		}

		public void onInsightsRefreshFinished(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onInsightsRefreshFinished");

			GrowInsights.I_SyncWithNative ();

			HighwayEvents.OnInsightsRefreshFinished(new InsightsRefreshFinishedEvent());
		}

		public static Action<GrowInsightsInitializedEvent> OnGrowInsightsInitialized = delegate {};
		public static Action<InsightsRefreshFailedEvent> OnInsightsRefreshFailed = delegate {};
		public static Action<InsightsRefreshFinishedEvent> OnInsightsRefreshFinished = delegate {};
		public static Action<InsightsRefreshStartedEvent> OnInsightsRefreshStarted = delegate {};

	}

}
