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

using System;
using UnityEngine;
using System.Collections.Generic;

namespace Soomla.Profile
{

	/// <summary>
	/// This class represents a specific social provider (for example, Facebook, Twitter, etc). 
	/// Each social provider needs to implement the functions in this class.
	/// </summary>

	public delegate void ContactsFailed(string message);
	public delegate void ContactsSuccess(SocialPageData<UserProfile> contactsData);
	public delegate void UserProfileFailed(string message);
	public delegate void UserProfileSuccess(UserProfile userProfile);
	public delegate void SocialActionFailed(string message);
	public delegate void SocialActionSuccess();
	public delegate void SocialActionCancel();
	public delegate void AppRequestSuccess(string requestId, List<string> recipients);
	public delegate void AppRequestFailed(string message);
	public delegate void InviteSuccess(string requestId, List<string> invitedIds);
	public delegate void InviteFailed(string message);
	public delegate void InviteCancelled();
	public delegate void FeedFailed(string message);
	public delegate void FeedSuccess(SocialPageData<String> feedData);

	public interface ISocialProvider : IProvider
	{
		/// <summary>
		/// See docs in <see cref="SoomlaProfile.UpdateStatus"/>
		/// </summary>
		void UpdateStatus(string status, SocialActionSuccess success, SocialActionFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.UpdateStatusDialog"/>
		/// </summary>
		void UpdateStatusDialog(string link, SocialActionSuccess success, SocialActionFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.UpdateStory"/>
		/// </summary>
		void UpdateStory(string message, string name, string caption, string description,
		                                 string link, string pictureUrl, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.UpdateStoryDialog"/>
		/// </summary>
		void UpdateStoryDialog(string name, string caption, string description, string link, string picture,
		                                       SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel);
		/// <summary>
		/// See docs in <see cref="SoomlaProfile.UploadImage"/>
		/// </summary>
		void UploadImage(byte[] texBytes, string fileName, string message, SocialActionSuccess success, SocialActionFailed fail, SocialActionCancel cancel);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.GetContacts"/>
		/// </summary>
		void GetContacts(bool fromStart, ContactsSuccess success, ContactsFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.GetFeed"/>
		/// </summary>
		void GetFeed(bool fromStart, FeedSuccess success, FeedFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Invite"/>
		/// </summary>
		void Invite(string inviteMessage, string dialogTitle, InviteSuccess success, InviteFailed fail, InviteCancelled cancel);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.AppRequest"/>
		/// </summary>
		void AppRequest(string message, string[] to, string extraData, string dialogTitle, AppRequestSuccess success, AppRequestFailed fail);

		/// <summary>
		/// See docs in <see cref="SoomlaProfile.Like"/>
		/// </summary>
		void Like(string pageId);
	}
}

