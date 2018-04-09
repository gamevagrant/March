using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetFeedFailedEvent : BaseSocialActionEvent
    {
        public readonly String errorDescription;

        public GetFeedFailedEvent(Provider provider, String errorDescription) : base(provider)
        {
            this.errorDescription = errorDescription;
        }
    }
}
