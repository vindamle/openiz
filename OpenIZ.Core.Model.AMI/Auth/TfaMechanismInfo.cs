﻿/*
 * Copyright 2015-2017 Mohawk College of Applied Arts and Technology
 *
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: khannan
 * Date: 2016-11-30
 */
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace OpenIZ.Core.Model.AMI.Auth
{
	/// <summary>
	/// Represents two-factor authentication mechanism information
	/// </summary>
	[XmlType(nameof(TfaMechanismInfo), Namespace = "http://openiz.org/ami")]
	[XmlRoot(nameof(TfaMechanismInfo), Namespace = "http://openiz.org/ami")]
	[JsonObject(nameof(TfaMechanismInfo))]
	public class TfaMechanismInfo
	{
		/// <summary>
		/// Default serialization ctor
		/// </summary>
		public TfaMechanismInfo()
		{
		}

		/// <summary>
		/// Gets or sets the identifier
		/// </summary>
		[XmlElement("id"), JsonProperty("id")]
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the name
		/// </summary>
		[XmlElement("name"), JsonProperty("name")]
		public String Name { get; set; }

		/// <summary>
		/// Gets or sets the challenge text
		/// </summary>
		[XmlElement("challengeText"), JsonProperty("challengeText")]
		public String ChallengeText { get; set; }
	}
}