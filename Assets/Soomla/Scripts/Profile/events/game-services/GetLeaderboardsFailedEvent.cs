using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
	public class GetLeaderboardsFailedEvent : BaseSocialActionEvent
	{
		public readonly string ErrorDescription;

		public GetLeaderboardsFailedEvent(Provider provider, string errorDescription, string payload) : base(provider, payload) {
			this.ErrorDescription = errorDescription;
		}
	}
}