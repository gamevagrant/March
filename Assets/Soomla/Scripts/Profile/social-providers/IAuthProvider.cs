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

	/// <summary>
	/// This class represents a auth provider (for example, Facebook, Twitter, etc).
	/// Each auth provider needs to implement the functions in this class.
	/// </summary>

	public delegate void LoginSuccess();
	public delegate void LoginFailed(string message);
	public delegate void LoginCancelled();
	public delegate void GetUserProfileSuccess(UserProfile userProfile);
	public delegate void GetUserProfileFailed(string message);
	public delegate void LogoutFailed(string message);
	public delegate void LogoutSuccess();

	public interface IAuthProvider : IProvider
	{
		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Logout"/>
		/// </summary>
		void Logout(LogoutSuccess success, LogoutFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Login"/>
		/// </summary>
		void Login(LoginSuccess success, LoginFailed fail, LoginCancelled cancel);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.GetUserProfile"/>
		/// </summary>
		void GetUserProfile(GetUserProfileSuccess success, GetUserProfileFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.IsLoggedIn"/>
		/// </summary>
		bool IsLoggedIn();

		/// <summary>
		/// Return value of autoLogin setting of the provider.
		/// </summary>
		/// <returns>value of autoLogin
		bool IsAutoLogin();
	}
}

