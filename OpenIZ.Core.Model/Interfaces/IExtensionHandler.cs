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
 * Date: 2016-8-2
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenIZ.Core.Interfaces
{
    /// <summary>
    /// Extension handler contract
    /// </summary>
    public interface IExtensionHandler
    {

        /// <summary>
        /// Gets the name of the handler
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Represents the data as a .net value
        /// </summary>
        object DeSerialize(byte[] extensionData);

        /// <summary>
        /// Serializes the data 
        /// </summary>
        byte[] Serialize(object data);

        /// <summary>
        /// Gets the display value
        /// </summary>
        String GetDisplay(object data);
    }
}
