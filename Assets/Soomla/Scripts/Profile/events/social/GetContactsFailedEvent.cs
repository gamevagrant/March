using System;
using System.Collections;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetContactsFailedEvent : BaseSocialActionEvent
    {
        public readonly String ErrorDescription;
		public readonly bool fromStart;
		public readonly String payLoad;

        public GetContactsFailedEvent(Provider provider, String errorDescription, bool fromStart, String payLoad): base(provider)
        {
            this.ErrorDescription = errorDescription;
			this.fromStart = fromStart;
			this.payLoad = payLoad;
        }
    }
}
