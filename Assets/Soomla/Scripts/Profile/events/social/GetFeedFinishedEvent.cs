using System;
using System.Collections.Generic;

namespace Soomla.Profile
{
    public class GetFeedFinishedEvent : BaseSocialActionEvent
    {
		public readonly SocialPageData<String> Posts;

		public GetFeedFinishedEvent(Provider provider, SocialPageData<String>  feedPosts) : base(provider)
        {
            this.Posts = feedPosts;
        }
    }
}
