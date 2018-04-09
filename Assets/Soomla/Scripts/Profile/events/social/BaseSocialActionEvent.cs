using System;

namespace Soomla.Profile
{
	public abstract class BaseSocialActionEvent : SoomlaEvent
    {
		public readonly Provider Provider;

		protected BaseSocialActionEvent(Provider provider, string payload = "", Object sender = null) : base(sender, payload) {
			this.Provider = provider;
		}
    }
}
