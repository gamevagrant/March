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

	public interface IProvider
	{
		/// <summary>
		/// The place, where you can configure the provider, using params passed by user.
		/// It's relevant for non-native providers only.
		/// </summary>
		/// <param name="providerParams">Params of this provider, passed during Profile initialization.</param>
		void Configure(Dictionary<string, string> providerParams);

		bool IsNativelyImplemented();
	}
}

