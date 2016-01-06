﻿using OpenIZ.Core.Model.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MARC.HI.EHRS.SVC.Core.Authorization;
using MARC.HI.EHRS.SVC.Core.Data;
using OpenIZ.Persistence.Data.MSSQL.Data;
using System.Linq.Expressions;
using OpenIZ.Persistence.Data.MSSQL.Exceptions;
using OpenIZ.Persistence.Data.MSSQL.Linq;
using System.Reflection;
using OpenIZ.Core.Model;

namespace OpenIZ.Persistence.Data.MSSQL.Services.Security
{
    /// <summary>
    /// A persistence service which can persist and query security user objects
    /// </summary>
    public class SecurityUserPersistenceService : BaseDataPersistenceService<Core.Model.Security.SecurityUser>, IModelConverter
    {

        /// <summary>
        /// Gets the domain type which this can convert
        /// </summary>
        public Type DomainType
        {
            get
            {
                return typeof(Data.SecurityUser);
            }
        }

        /// <summary>
        /// Gets the model type of the converter
        /// </summary>
        public Type ModelType
        {
            get
            {
                return typeof(Core.Model.Security.SecurityUser);
            }
        }

        /// <summary>
        /// Convert the 
        /// </summary>
        /// <param name="domainModel"></param>
        /// <returns></returns>
        public BaseData MapInstance(object domainModel)
        {
            if (domainModel is Data.SecurityUser)
                return this.Convert(domainModel as Data.SecurityUser);
            return null;
        }

        /// <summary>
        /// Convert model instance to this instance
        /// </summary>
        public object MapInstance(BaseData modelInstance)
        {
            if (modelInstance is Core.Model.Security.SecurityUser)
                return this.Convert(modelInstance as Core.Model.Security.SecurityUser);
            return null;
        }

        /// <summary>
        /// Map members
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public MemberInfo MapMember(MemberInfo member)
        {
            switch(member.Name)
            {
                case "Key":
                    return typeof(Data.SecurityUser).GetProperty("UserId");
                default:
                    return member;
            }
        }

        /// <summary>
        /// Perform a get operation
        /// </summary>
        /// <param name="containerId">The identifier of the container to retrieve</param>
        /// <param name="authContext">The authorization context</param>
        /// <param name="loadFast">True if the history and historical data should be loaded</param>
        /// <param name="dataContext">The data context</param>
        /// <returns>The security user as part of the get</returns>
        protected override Core.Model.Security.SecurityUser DoGet(Identifier<Guid> containerId, AuthorizationContext authContext, bool loadFast, ModelDataContext dataContext)
        {
            var dataUser = dataContext.SecurityUsers.FirstOrDefault(o => o.UserId == containerId.Id);

            if (dataUser != null)
                return this.Convert(dataUser);
            else
                return null;
        }

        /// <summary>
        /// Perform an insert
        /// </summary>
        /// <param name="storageData">The model class to be stored</param>
        /// <param name="authContext">The authorization context</param>
        /// <param name="dataContext">The data context</param>
        /// <returns>The security user which was inserted</returns>
        protected override Core.Model.Security.SecurityUser DoInsert(Core.Model.Security.SecurityUser storageData, AuthorizationContext authContext, ModelDataContext dataContext)
        {
            if (storageData.Id != null) // Trying to insert an already inserted user?
                throw new SqlFormalConstraintException("Insert must be for an unidentified object");

            var dataUser = this.Convert<Data.SecurityUser>(storageData);
            dataUser.CreatedBy = base.GetUserFromAuthContext(authContext, dataContext);
            dataUser.UserId = Guid.NewGuid();
            dataContext.SecurityUsers.InsertOnSubmit(dataUser);

            return this.Convert(dataUser);
        }

        /// <summary>
        /// Perform an obsolete
        /// </summary>
        /// <param name="storageData">The data object to be obsoleted</param>
        /// <param name="authContext">The authorization context under which the security user is obsolete</param>
        /// <param name="dataContext">The current data context</param>
        /// <returns>The obsoleted user</returns>
        protected override Core.Model.Security.SecurityUser DoObsolete(Core.Model.Security.SecurityUser storageData, AuthorizationContext authContext, ModelDataContext dataContext)
        {
            if (storageData.Id == null)
                throw new SqlFormalConstraintException("Obsolete must be for an identified object");

            var dataUser = this.Convert<Data.SecurityUser>(storageData);
            dataUser.ObsoletedBy = base.GetUserFromAuthContext(authContext, dataContext);
            dataUser.ObsoletionTime = DateTimeOffset.Now;

            return this.Convert(dataUser);
        }

        /// <summary>
        /// Perform a query 
        /// </summary>
        protected override IQueryable<Core.Model.Security.SecurityUser> DoQuery(Expression<Func<Core.Model.Security.SecurityUser, bool>> query, AuthorizationContext authContext, ModelDataContext dataContext)
        {
            var converter = new ModelConversionVisitor();
            Expression queryExpression = converter.Convert<Core.Model.Security.SecurityUser, Data.SecurityUser>(query);
            return null;
        }

        protected override Core.Model.Security.SecurityUser DoUpdate(Core.Model.Security.SecurityUser storageData, AuthorizationContext authContext, ModelDataContext dataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert from model to domain
        /// </summary>
        internal override TData Convert<TData>(Core.Model.Security.SecurityUser model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Convert from domain to model
        /// </summary>
        internal override Core.Model.Security.SecurityUser Convert<TData>(TData data)
        {
            throw new NotImplementedException();
        }
    }
}
