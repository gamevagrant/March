using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetFeedStartedEvent : BaseSocialActionEvent
    {
        public GetFeedStartedEvent(Provider provider) : base(provider)
        {

        }
    }
}
