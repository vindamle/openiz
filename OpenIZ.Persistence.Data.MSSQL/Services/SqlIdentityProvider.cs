﻿using MARC.HI.EHRS.SVC.Core.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Security;
using MARC.HI.EHRS.SVC.Core;
using MARC.HI.EHRS.SVC.Core.Services;
using OpenIZ.Core.Model.Security;
using MARC.HI.EHRS.SVC.Core.Services.Policy;
using OpenIZ.Persistence.Data.MSSQL.Configuration;
using System.Configuration;
using OpenIZ.Persistence.Data.MSSQL.Data;
using OpenIZ.Core.Security;
using OpenIZ.Persistence.Data.MSSQL.Security;
using MARC.HI.EHRS.SVC.Core.Exceptions;
using MARC.HI.EHRS.SVC.Core.Event;
using System.Diagnostics;

namespace OpenIZ.Persistence.Data.MSSQL.Services
{
    /// <summary>
    /// Identity provider service
    /// </summary>
    public sealed class SqlIdentityProvider : IIdentityProviderService
    {

        // Trace source
        private TraceSource m_traceSource = new TraceSource("OpenIZ.Persistence.Data.MSSQL.Services.Identity");

        // Configuration
        private SqlConfiguration m_configuration = ConfigurationManager.GetSection("openiz.persistence.data.mssql") as SqlConfiguration;

        /// <summary>
        /// Fired prior to an authentication request being made
        /// </summary>
        public event EventHandler<AuthenticatingEventArgs> Authenticating;

        /// <summary>
        /// Fired after an authentication request has been made
        /// </summary>
        public event EventHandler<AuthenticatedEventArgs> Authenticated;

        /// <summary>
        /// Gets or sets the two factor secret generator
        /// </summary>
        public ITwoFactorSecretGenerator TwoFactorSecretGenerator { get; set; }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        public IPrincipal Authenticate(string userName, string password)
        {
            var evt = new AuthenticatingEventArgs(userName);
            this.Authenticating?.Invoke(this, evt);
            if (evt.Cancel)
                throw new SecurityException("Authentication cancelled");

            this.m_traceSource.TraceInformation("Authenticating {0}/{1}", userName, password);

            try
            {
                var principal = SqlClaimsIdentity.Create(userName, password).CreateClaimsPrincipal();
                this.Authenticated?.Invoke(this, new AuthenticatedEventArgs(userName, principal, true));
                return principal;
            }
            catch(SecurityException e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                this.Authenticated?.Invoke(this, new AuthenticatedEventArgs(userName, null, false));
                throw;
            }
        }

        /// <summary>
        /// Gets an un-authenticated identity
        /// </summary>
        public IIdentity GetIdentity(string userName)
        {
            return SqlClaimsIdentity.Create(userName);
        }

        /// <summary>
        /// Authenticate the user using a TwoFactorAuthentication secret
        /// </summary>
        public IPrincipal Authenticate(string userName, string password, string tfaSecret)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Change the user's password
        /// </summary>
        public void ChangePassword(string userName, string newPassword, IPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));
            else if (!principal.Identity.IsAuthenticated)
                throw new SecurityException("Authorization context must be authenticated");

            this.m_traceSource.TraceInformation("Change userpassword for {0} to {1} ({2})", userName, newPassword, principal);

            try
            {
                // Create the hasher and load the user
                using (var dataContext = new ModelDataContext(this.m_configuration.ReadWriteConnectionString))
                {
                    var user = dataContext.SecurityUsers.Where(u => u.UserName == userName && !u.ObsoletionTime.HasValue).SingleOrDefault();
                    if (user == null)
                        throw new InvalidOperationException(String.Format("Cannot locate user {0}", userName));

                    // Security check
                    var policyDecisionService = ApplicationContext.Current.GetService<IPolicyDecisionService>();
                    var passwordHashingService = ApplicationContext.Current.GetService<IPasswordHashingService>();

                    var pdpOutcome = policyDecisionService?.GetPolicyOutcome(principal, PolicyIdentifiers.OpenIzChangePasswordPolicy);
                    if (userName != principal.Identity.Name &&
                        pdpOutcome.HasValue &&
                        pdpOutcome != PolicyDecisionOutcomeType.Grant)
                        throw new PolicyViolationException(PolicyIdentifiers.OpenIzChangePasswordPolicy, pdpOutcome.Value);

                    user.UserPassword = passwordHashingService.EncodePassword(newPassword);
                    dataContext.SubmitChanges();
                }
            }
            catch(Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                throw;
            }
        }

        /// <summary>
        /// Generate and store the TFA secret
        /// </summary>
        public string GenerateTfaSecret(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a basic user
        /// </summary>
        public IIdentity CreateIdentity(string userName, string password, IPrincipal authContext)
        {

            if (String.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            else if (String.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));

            this.m_traceSource.TraceInformation("Creating identity {0} ({1})", userName, authContext);

            try
            {
                using (var dataContext = new ModelDataContext(this.m_configuration.ReadWriteConnectionString))
                {
                    var hashingService = ApplicationContext.Current.GetService<IPasswordHashingService>();
                    var pdpService = ApplicationContext.Current.GetService<IPolicyDecisionService>();

                    var policyOutcome = pdpService?.GetPolicyOutcome(authContext, PolicyIdentifiers.OpenIzCreateIdentityPolicy);
                    if (policyOutcome.HasValue &&
                        policyOutcome != PolicyDecisionOutcomeType.Grant)
                        throw new PolicyViolationException(PolicyIdentifiers.OpenIzCreateIdentityPolicy, policyOutcome.Value);

                    // Does this principal have the ability to 
                    Data.SecurityUser newIdentityUser = new Data.SecurityUser()
                    {
                        UserName = userName,
                        UserPassword = hashingService.EncodePassword(password),
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    if (authContext != null)
                        newIdentityUser.CreatedByEntity = dataContext.SecurityUsers.Single(u => u.UserName == authContext.Identity.Name);

                    dataContext.SecurityUsers.InsertOnSubmit(newIdentityUser);
                    dataContext.SubmitChanges();

                    return SqlClaimsIdentity.Create(newIdentityUser);

                }
            }
            catch(Exception e)
            {
                this.m_traceSource.TraceEvent(TraceEventType.Error, e.HResult, e.ToString());
                throw;
            }

        }
    }
}
