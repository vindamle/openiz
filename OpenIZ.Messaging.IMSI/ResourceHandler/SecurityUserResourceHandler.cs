﻿/*
 * Copyright 2015-2016 Mohawk College of Applied Arts and Technology
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
 * User: Nityan
 * Date: 2016-9-5
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIZ.Core.Model;
using OpenIZ.Core.Model.Query;
using OpenIZ.Core.Services;
using MARC.HI.EHRS.SVC.Core;
using OpenIZ.Core.Model.Security;
using OpenIZ.Core.Security.Attribute;
using System.Security.Permissions;
using OpenIZ.Core.Security;
using OpenIZ.Core.Model.Collection;

namespace OpenIZ.Messaging.IMSI.ResourceHandler
{
	/// <summary>
	/// Represents a resource handler for security users.
	/// </summary>
	public class SecurityUserResourceHandler : IResourceHandler
	{
		/// <summary>
		/// The internal reference to the <see cref="ISecurityRepositoryService"/> instance.
		/// </summary>
		private ISecurityRepositoryService repository;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecurityUserResourceHandler"/> class.
		/// </summary>
		public SecurityUserResourceHandler()
		{
			ApplicationContext.Current.Started += (o, e) => this.repository = ApplicationContext.Current.GetService<ISecurityRepositoryService>();
		}

		/// <summary>
		/// Gets the name of the resource that this handler.
		/// </summary>
		public string ResourceName
		{
			get
			{
				return "SecurityUser";
			}
		}

		/// <summary>
		/// Gets the .NET type of the resource handler.
		/// </summary>
		public Type Type
		{
			get
			{
				return typeof(SecurityUser);
			}
		}

		/// <summary>
		/// Creates the specified user entity
		/// </summary>
		[PolicyPermission(SecurityAction.Demand, PolicyId = PermissionPolicyIdentifiers.UnrestrictedAdministration)]
		public IdentifiedData Create(IdentifiedData data, bool updateIfExists)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			Bundle bundleData = data as Bundle;

			bundleData?.Reconstitute();

			var processData = bundleData?.Entry ?? data;

			if (processData is Bundle)
			{
				throw new InvalidOperationException("Bundle must have an entry point");
			}
			else if (processData is SecurityUser)
			{
				var securityUser = processData as SecurityUser;

				if (updateIfExists)
				{
					return this.repository.SaveUser(securityUser);
				}
				else
				{
					return this.repository.CreateUser(securityUser, securityUser.PasswordHash);
				}
			}
			else
			{
				throw new ArgumentException(nameof(data), "Invalid data type");
			}
		}

		/// <summary>
		/// Gets the specified user entity
		/// </summary>
		public IdentifiedData Get(Guid id, Guid versionId)
		{
			return this.repository.GetUser(id);
		}

		/// <summary>
		/// Obsolete the entity
		/// </summary>
		[PolicyPermission(SecurityAction.Demand, PolicyId = PermissionPolicyIdentifiers.UnrestrictedAdministration)]
		public IdentifiedData Obsolete(Guid key)
		{
			return this.repository.ObsoleteUser(key);
		}

		/// <summary>
		/// Queries the specified user entity
		/// </summary>
		public IEnumerable<IdentifiedData> Query(NameValueCollection queryParameters)
		{
			return this.repository.FindUsers(QueryExpressionParser.BuildLinqExpression<SecurityUser>(queryParameters));
		}

		/// <summary>
		/// Query the specified user entity with restrictions
		/// </summary>
		public IEnumerable<IdentifiedData> Query(NameValueCollection queryParameters, int offset, int count, out int totalCount)
		{
			return this.repository.FindUsers(QueryExpressionParser.BuildLinqExpression<SecurityUser>(queryParameters), offset, count, out totalCount);
		}

		/// <summary>
		/// Updates the specified user entity
		/// </summary>
		public IdentifiedData Update(IdentifiedData data)
		{
			if (data == null)
				throw new ArgumentNullException(nameof(data));

			var bundleData = data as Bundle;

			bundleData?.Reconstitute();

			var saveData = bundleData?.Entry ?? data;

			if (saveData is Bundle)
			{
				throw new InvalidOperationException("Bundle must have an entry");
			}
			else if (saveData is SecurityUser)
			{
				return this.repository.SaveUser(saveData as SecurityUser);
			}
			else
			{
				throw new ArgumentException(nameof(data), "Invalid storage type");
			}
		}
	}
}