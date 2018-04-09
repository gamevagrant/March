/// Copyright (C) 2012-2014 Soomla Inc.
///
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///      http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.

using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Soomla.Profile {

	//TODO: add invite push
	public class ProfileEventPusherAndroid : Soomla.Profile.ProfileEvents.ProfileEventPusher {

#if UNITY_ANDROID && !UNITY_EDITOR

		// event pushing back to native (when using FB Unity SDK)
		protected override void _pushEventLoginStarted(Provider provider, bool autoLogin, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginStarted", provider.ToString(), autoLogin, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginFinished(UserProfile userProfile, bool autoLogin, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(userProfile.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginFinished", userProfile.toJSONObject().print(), autoLogin, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginFailed(Provider provider, string message, bool autoLogin, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginFailed", provider.ToString(), message, autoLogin, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLoginCancelled(Provider provider, bool autoLogin, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLoginCancelled", provider.ToString(), autoLogin, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutStarted(Provider provider) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutStarted", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutFinished(Provider provider) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutFinished", provider.ToString());
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventLogoutFailed(Provider provider, string message) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventLogoutFailed",
				                                 provider.ToString(), message);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionStarted(Provider provider, SocialActionType actionType, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionStarted",
				                                 provider.ToString(), actionType.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionFinished(Provider provider, SocialActionType actionType, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionFinished",
				                                 provider.ToString(), actionType.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionCancelled(Provider provider, SocialActionType actionType, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionCancelled",
				                                 provider.ToString(), actionType.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventSocialActionFailed(Provider provider, SocialActionType actionType, string message, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSocialActionFailed", 
				                                 provider.ToString(), actionType.ToString(), message, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetContactsStarted (Provider provider, bool fromStart, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetContactsStarted", 
				                                 provider.ToString(), fromStart, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetContactsFinished (Provider provider, SocialPageData<UserProfile> contactsPage, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			List<JSONObject> profiles = new List<JSONObject>();
			foreach (var profile in contactsPage.PageData) {
				profiles.Add(profile.toJSONObject());
			}
			JSONObject contacts = new JSONObject(profiles.ToArray());

			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetContactsFinished", 
				                                 provider.ToString(), contacts.ToString(), payload, contactsPage.HasMore);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetContactsFailed (Provider provider, string message, bool fromStart, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetContactsFailed", 
				                                 provider.ToString(), message, fromStart, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetFeedFinished (Provider provider, SocialPageData<String> feedPage, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			List<JSONObject> feeds = new List<JSONObject>();
			foreach (var feed in feedPage.PageData) {
				feeds.Add(JSONObject.StringObject(feed));
			}
			JSONObject feedJson = new JSONObject(feeds.ToArray());

			AndroidJNI.PushLocalFrame(100);
			using (AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetFeedFinished", 
				                                 provider.ToString(), feedJson.ToString(), payload, feedPage.HasMore);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventGetFeedFailed(Provider provider, string message, bool fromStart, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using (AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetFeedFailed", 
				                                 provider.ToString(), message, fromStart, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventInviteStarted(Provider provider, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventInviteStarted",
				                                 provider.ToString(), SocialActionType.INVITE.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventInviteFinished(Provider provider, string requestId, List<string> invitedIds, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				List<JSONObject> invited = new List<JSONObject>();
				foreach (var id in invitedIds) {
					invited.Add(JSONObject.StringObject(id));
				}
				JSONObject jsonInvited = new JSONObject(invited.ToArray());
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventInviteFinished",
				                                 provider.ToString(), SocialActionType.INVITE.ToString(), requestId, jsonInvited.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventInviteCancelled(Provider provider, string payload) {
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventInviteCancelled",
				                                 provider.ToString(), SocialActionType.INVITE.ToString(), payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
		protected override void _pushEventInviteFailed(Provider provider, string message, string payload) { 
			if (SoomlaProfile.IsProviderNativelyImplemented(provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventInviteFailed", 
				                                 provider.ToString(), SocialActionType.INVITE.ToString(), message, payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetLeaderboardsStarted(GetLeaderboardsStartedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetLeaderboardsStarted",
						ev.Provider.ToString(), ev.Provider);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetLeaderboardsFinished(GetLeaderboardsFinishedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			List<JSONObject> leaderboardList = new List<JSONObject>();
			foreach (var lb in ev.Leaderboards.PageData) {
				leaderboardList.Add(lb.toJSONObject());
			}
			JSONObject jsonLbs = new JSONObject(leaderboardList.ToArray());
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetLeaderboardsFinished",
						ev.Provider.ToString(), jsonLbs.ToString(), ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetLeaderboardsFailed(GetLeaderboardsFailedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetLeaderboardsFailed",
						ev.Provider.ToString(), ev.ErrorDescription, ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetScoresStarted(GetScoresStartedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetScoresStarted",
						ev.Provider.ToString(), ev.From.toJSONObject().ToString(), ev.FromStart, ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetScoresFinished(GetScoresFinishedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			List<JSONObject> scoreList = new List<JSONObject>();
			foreach (var sc in ev.Scores.PageData) {
				scoreList.Add(sc.toJSONObject());
			}
			JSONObject jsonSc = new JSONObject(scoreList.ToArray());
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetScoresFinished",
						ev.Provider.ToString(), ev.From.toJSONObject().ToString(), jsonSc.ToString(), ev.Scores.HasMore, ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventGetScoresFailed(GetScoresFailedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventGetScoresFailed",
						ev.Provider.ToString(), ev.From.toJSONObject().ToString(), ev.ErrorDescription, ev.FromStart, ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventSubmitScoreStarted(SubmitScoreStartedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSubmitScoreStarted",
						ev.Provider.ToString(), ev.Destination.toJSONObject().ToString(), ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventSubmitScoreFinished(SubmitScoreFinishedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSubmitScoreFinished",
						ev.Provider.ToString(), ev.Destination.toJSONObject().ToString(), ev.Score.toJSONObject().ToString(), ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventSubmitScoreFailed(SubmitScoreFailedEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventSubmitScoreFailed",
						ev.Provider.ToString(), ev.Destination.toJSONObject().ToString(), ev.ErrorDescription, ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}

		protected override void _pushEventShowLeaderboards(ShowLeaderboardsEvent ev) {
			if (SoomlaProfile.IsProviderNativelyImplemented(ev.Provider)) return;
			AndroidJNI.PushLocalFrame(100);
			using(AndroidJavaClass jniSoomlaProfile = new AndroidJavaClass("com.soomla.profile.unity.ProfileEventHandler")) {
				ProfileJNIHandler.CallStaticVoid(jniSoomlaProfile, "pushEventShowLeaderboards",
						ev.Provider.ToString(), ev.Payload);
			}
			AndroidJNI.PopLocalFrame(IntPtr.Zero);
		}
#endif
	}
}
