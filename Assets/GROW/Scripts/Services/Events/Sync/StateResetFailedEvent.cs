using Grow.Highway;
using Grow;
using System;
using System.Collections.Generic;

namespace Grow.Sync {
	public class StateResetFailedEvent : GrowEvent {

		public readonly StateSyncErrorCode ErrorCode;
		public readonly string ErrorMessage;

		public StateResetFailedEvent(StateSyncErrorCode errorCode, string errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}
	}
}