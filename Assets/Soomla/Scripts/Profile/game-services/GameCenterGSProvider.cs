/// Copyright (C) 2012-2015 Soomla Inc.
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
using System;
using UnityEngine;
using System.Collections.Generic;

namespace Soomla.Profile
{
#if UNITY_IOS || UNITY_EDITOR
	public class GameCenterGSProvider : IAuthProvider, IGameServicesProvider
	{
		
		public GameCenterGSProvider () {
			SoomlaProfile.ProviderBecameReady(this);
		}

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Logout"/>
		/// </summary>
		public void Logout(LogoutSuccess success, LogoutFailed fail) {}

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Login"/>
		/// </summary>
		public void Login(LoginSuccess success, LoginFailed fail, LoginCancelled cancel) {}

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.GetUserProfile"/>
		/// </summary>
		public void GetUserProfile(GetUserProfileSuccess success, GetUserProfileFailed fail) {}

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.IsLoggedIn"/>
		/// </summary>
		public bool IsLoggedIn() {return false;}

		/// <summary>
		/// See docs in <see cref="ISocialProvider.IsAutoLogin"/>
		/// </summary>
		/// <returns>value of autoLogin
		public bool IsAutoLogin() {
			return false;
		}

		public void GetLeaderboards(SocialPageDataSuccess<Leaderboard> success, FailureHandler fail) {}

		public void GetScores(Leaderboard owner, bool fromStart, SocialPageDataSuccess<Score> success, FailureHandler fail) {}

		public void SubmitScore(Leaderboard targetLeaderboard, int value, SingleObjectSuccess<Score> success, FailureHandler fail) {}

		public void ShowLeaderboards() {}

		public void Configure(Dictionary<string, string> providerParams) { }

		public bool IsNativelyImplemented() {
			return true;
		}
	}
#endif
}