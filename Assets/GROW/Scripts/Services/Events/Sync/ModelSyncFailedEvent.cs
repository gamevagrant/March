using Grow.Highway;
using Grow;
using System;

namespace Grow.Sync {
	public class ModelSyncFailedEvent : GrowEvent {

		public readonly ModelSyncErrorCode ErrorCode;
		public readonly string ErrorMessage;

		public ModelSyncFailedEvent(ModelSyncErrorCode errorCode, string errorMessage) {
			this.ErrorCode = errorCode;
			this.ErrorMessage = errorMessage;
		}
	}
}