/*
 * Copyright (C) 2012-2015 Soomla Inc. - All Rights Reserved
 *
 *   Unauthorized copying of this file, via any medium is strictly prohibited
 *   Proprietary and confidential
 *
 *   Written by Refael Dakar <refael@soom.la>
 */

using Grow.Sync;
using Grow.Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Grow.Highway {

	public class ServicesEvents : CodeGeneratedSingleton {

		#if UNITY_IOS
		//&& !UNITY_EDITOR
		[DllImport ("__Internal")]
		private static extern int unityServicesEventDispatcher_Init();
		#endif

		private const string TAG = "SOOMLA ServicesEvents";

		public static ServicesEvents Instance = null;

		protected override bool DontDestroySingleton
		{
			get { return true; }
		}

		/// <summary>
		/// Initializes the different native event handlers in Android / iOS
		/// </summary>
		public static void Initialize() {
			if (Instance == null) {
				Instance = GetSynchronousCodeGeneratedInstance<ServicesEvents>();

				GrowUtils.LogDebug (TAG, "Initializing ServicesEvents...");
#if UNITY_ANDROID && !UNITY_EDITOR
				AndroidJNI.PushLocalFrame(100);
				using(AndroidJavaClass jniEventHandler = new AndroidJavaClass("com.soomla.highway.unity.ServicesEventHandler")) {
					jniEventHandler.CallStatic("initialize");
				}
				AndroidJNI.PopLocalFrame(IntPtr.Zero);

#elif UNITY_IOS && !UNITY_EDITOR
				unityServicesEventDispatcher_Init();
#endif
			}
		}

		public void onGrowSyncInitialized() {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onGrowSyncInitialized");
			ServicesEvents.OnGrowSyncInitialized(new GrowSyncInitializedEvent());
		}

		public void onModelSyncStarted() {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onModelSyncStarted");
			ServicesEvents.OnModelSyncStarted(new ModelSyncStartedEvent());
		}

		public void onModelSyncFinished(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onModelSyncFinished: " + message);

			JSONObject eventJSON = new JSONObject(message);
			List<JSONObject> changedComponentsJSON = eventJSON["changedComponents"].list;

			List<string> changedComponents = new List<string>();
			foreach (var changedComponentJSON in changedComponentsJSON) {
				changedComponents.Add(changedComponentJSON.str);
			}

			GrowSync.HandleModelSyncFinised();

			ServicesEvents.OnModelSyncFinished(new ModelSyncFinishedEvent(changedComponents));
		}

		public void onModelSyncFailed(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onModelSyncFailed:" + message);

			JSONObject eventJSON = new JSONObject(message);
			int errorCode = (int)eventJSON["errorCode"].n;
			string errorMessage = eventJSON["errorMessage"].str;

			ServicesEvents.OnModelSyncFailed(new ModelSyncFailedEvent((ModelSyncErrorCode)errorCode, errorMessage));
		}

		public void onStateSyncStarted() {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateSyncStarted");
			ServicesEvents.OnStateSyncStarted(new StateSyncStartedEvent());
		}

		public void onStateSyncFinished(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateSyncFinished: " + message);

			JSONObject eventJSON = new JSONObject(message);

			List<JSONObject> changedComponentsJSON = eventJSON["changedComponents"].list;
			List<string> changedComponents = new List<string>();
			foreach (var changedComponentJSON in changedComponentsJSON) {
				changedComponents.Add(changedComponentJSON.str);
			}

			List<JSONObject> failedComponentsJSON = eventJSON["failedComponents"].list;
			List<string> failedComponents = new List<string>();
			foreach (var failedComponentJSON in failedComponentsJSON) {
				failedComponents.Add(failedComponentJSON.str);
			}

			GrowSync.HandleStateSyncFinised();

			ServicesEvents.OnStateSyncFinished(new StateSyncFinishedEvent(changedComponents, failedComponents));
		}

		public void onStateSyncFailed(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateSyncFailed:" + message);

			JSONObject eventJSON = new JSONObject(message);
			int errorCode = (int)eventJSON["errorCode"].n;
			string errorMessage = eventJSON["errorMessage"].str;

			GrowSync.HandleStateSyncFailed();

			ServicesEvents.OnStateSyncFailed(new StateSyncFailedEvent((StateSyncErrorCode)errorCode, errorMessage));
		}

		public void onStateResetStarted() {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateResetStarted");
			ServicesEvents.OnStateResetStarted(new StateResetStartedEvent());
		}

		public void onStateResetFinished() {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateResetFinished");

			GrowSync.HandleStateSyncFinised();

			ServicesEvents.OnStateResetFinished(new StateResetFinishedEvent());
		}

		public void onStateResetFailed(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onStateResetFailed:" + message);

			JSONObject eventJSON = new JSONObject(message);
			int errorCode = (int)eventJSON["errorCode"].n;
			string errorMessage = eventJSON["errorMessage"].str;

			ServicesEvents.OnStateResetFailed(new StateResetFailedEvent((StateSyncErrorCode)errorCode, errorMessage));
		}

		/// <summary>
		/// Fired when Grow Sync is intialized.
		/// </summary>
		public static Action<GrowSyncInitializedEvent> OnGrowSyncInitialized = delegate {};
		/// <summary>
		/// Fired when the model sync process has began.
		/// </summary>
		public static Action<ModelSyncStartedEvent> OnModelSyncStarted = delegate {};
		/// <summary>
		/// Fired when the model sync process has finished.
		/// Provides a list of modules which were synced.
		/// </summary>
		public static Action<ModelSyncFinishedEvent> OnModelSyncFinished = delegate {};
		/// <summary>
		/// Fired when the model sync process has failed.
		/// Provides an error code and reason.
		/// </summary>
		public static Action<ModelSyncFailedEvent> OnModelSyncFailed = delegate {};
		/// <summary>
		/// Fired when the state sync process has began.
		/// </summary>
		public static Action<StateSyncStartedEvent> OnStateSyncStarted = delegate {};
		/// <summary>
		/// Fired when the state sync process has finished.
		/// Provides a list of modules which had their state updated, and a list of
		/// modules which failed to update.
		/// </summary>
		public static Action<StateSyncFinishedEvent> OnStateSyncFinished = delegate {};
		/// <summary>
		/// Fired when the state sync process has failed.
		/// Provides an error code and reason.
		/// </summary>
		public static Action<StateSyncFailedEvent> OnStateSyncFailed = delegate {};
		/// <summary>
		/// Fired when the state reset process has began.
		/// </summary>
		public static Action<StateResetStartedEvent> OnStateResetStarted = delegate {};
		/// <summary>
		/// Fired when the state reset process has finished.
		/// </summary>
		public static Action<StateResetFinishedEvent> OnStateResetFinished = delegate {};
		/// <summary>
		/// Fired when the state reset process has failed.
		/// Provides an error code and reason.
		/// </summary>
		public static Action<StateResetFailedEvent> OnStateResetFailed = delegate {};


		/* Internal SOOMLA events ... Not meant for public use */

		public void onConflict(string message) {
			GrowUtils.LogDebug(TAG, "SOOMLA/UNITY onConflict:" + message);

			JSONObject eventJSON = new JSONObject(message);
			string remoteStateStr = eventJSON["remoteState"].str;
			string currentStateStr = eventJSON["currentState"].str;
			string stateDiffStr = eventJSON["stateDiff"].str;

			JSONObject remoteState = new JSONObject(remoteStateStr);
			JSONObject currentState = new JSONObject(currentStateStr);
			JSONObject stateDiff = new JSONObject(stateDiffStr);

			GrowSync.HandleStateSyncConflict(remoteState, currentState, stateDiff);
		}
	}

	/// <summary>
	/// Enumeration of model sync failure codes
	/// </summary>
	public enum ModelSyncErrorCode
	{
		/// <summary>
		/// General error has occured
		/// </summary>
		GeneralError = 0,
		/// <summary>
		/// Failed due to server communication failure
		/// </summary>
		ServerError = 1,
		/// <summary>
		/// The model was not able to update
		/// </summary>
		UpdateModelError = 2
	}

	/// <summary>
	/// Enumeration of state sync failure codes
	/// </summary>
	public enum StateSyncErrorCode
	{
		/// <summary>
		/// General error has occured
		/// </summary>
		GeneralError = 0,
		/// <summary>
		/// Failed due to server communication failure
		/// </summary>
		ServerError = 1,
		/// <summary>
		/// The state was not able to update
		/// </summary>
		UpdateStateError = 2,
		/// <summary>
		/// The user is already connected on another device
		/// </summary>
		UserAlreadyConnected = 3
	}

}
