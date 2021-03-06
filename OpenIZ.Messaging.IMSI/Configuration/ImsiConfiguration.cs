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
 * User: justi
 * Date: 2016-6-14
 */
namespace OpenIZ.Messaging.IMSI.Configuration
{
    /// <summary>
    /// Configuration class for IMSI configuration
    /// </summary>
    public class ImsiConfiguration
    {

        /// <summary>
        /// Creates a new IMSI configuration
        /// </summary>
        public ImsiConfiguration(string wcfServiceName)
        {
            this.WcfServiceName = wcfServiceName;
        }

        /// <summary>
        /// Gets the wcf service name
        /// </summary>
        public string WcfServiceName { get; private set; }
    }
}