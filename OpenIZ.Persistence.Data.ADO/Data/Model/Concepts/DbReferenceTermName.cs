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
 * Date: 2017-1-14
 */
using OpenIZ.OrmLite.Attributes;
using OpenIZ.Persistence.Data.ADO.Data.Model.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIZ.Persistence.Data.ADO.Data.Model.Security;

namespace OpenIZ.Persistence.Data.ADO.Data.Model.Concepts
{
    /// <summary>
    /// Reference term name
    /// </summary>
    [Table("ref_term_name_tbl")]
    public class DbReferenceTermName : DbAssociation, IDbBaseData
    {
        /// <summary>
        /// Gets or sets the key
        /// </summary>
        [Column("ref_term_name_id"), PrimaryKey, AutoGenerated]
        public override Guid Key { get; set; }

        /// <summary>
        /// Gets or sets the ref term to which the nae applies
        /// </summary>
        [Column("ref_term_id"), ForeignKey(typeof(DbReferenceTerm), nameof(DbReferenceTerm.Key))]
        public override Guid SourceKey { get; set; }

	    /// <summary>
	    /// Created by 
	    /// </summary>
	    [Column("crt_usr_id"), ForeignKey(typeof(DbSecurityUser), nameof(DbSecurityUser.Key))]
	    public Guid CreatedByKey { get; set; }

	    /// <summary>
	    /// Creation time
	    /// </summary>
	    [Column("crt_utc"), AutoGenerated]
	    public DateTimeOffset CreationTime { get; set; }

	    /// <summary>
	    /// Obsoleted by 
	    /// </summary>
	    [Column("obslt_usr_id"), ForeignKey(typeof(DbSecurityUser), nameof(DbSecurityUser.Key))]
	    public Guid? ObsoletedByKey { get; set; }

	    /// <summary>
	    /// Gets or sets the obsoletion time
	    /// </summary>
	    [Column("obslt_utc")]
	    public DateTimeOffset? ObsoletionTime { get; set; }


		/// <summary>
		/// Gets or sets the language code
		/// </summary>
		[Column("lang_cs")]
        public String LanguageCode { get; set; }

        /// <summary>
        /// Gets orsets the value
        /// </summary>
        [Column("term_name")]
        public String Value { get; set; }

        /// <summary>
        /// Gets or sets the phonetic code
        /// </summary>
        [Column("phon_cs")]
        public String PhoneticCode { get; set; }

        /// <summary>
        /// Gets or sets the algorithm id
        /// </summary>
        [Column("phon_alg_id"), ForeignKey(typeof(DbPhoneticAlgorithm), nameof(DbPhoneticAlgorithm.Key))]
        public Guid PhoneticAlgorithm { get; set; }
    }
}
