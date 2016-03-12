﻿/*
 * Copyright 2016-2016 Mohawk College of Applied Arts and Technology
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
 * User: fyfej
 * Date: 2016-1-24
 */
using OpenIZ.Core.Model.Attributes;
using OpenIZ.Core.Model.DataTypes;
using OpenIZ.Core.Model.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenIZ.Core.Model.Acts
{
    /// <summary>
    /// Associates an entity which participates in an act
    /// </summary>
    
    [XmlType(Namespace = "http://openiz.org/model", TypeName = "ActParticipation")]
    public class ActParticipation : VersionedAssociation<Act>
    {

        private Guid m_playerKey;
        
        private Entity m_player;
        private Guid m_participationRoleKey;
        
        private Concept m_participationRole;

        /// <summary>
        /// Gets or sets the target entity reference
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        
        [XmlElement("player"), JsonProperty("player")]
        public Guid PlayerEntityKey
        {
            get { return this.m_playerKey; }
            set
            {
                this.m_playerKey = value;
                this.m_player = null;
            }
        }

        /// <summary>
        /// Gets or sets the participation role key
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        
        [XmlElement("participationRole"), JsonProperty("participationRole")]
        public Guid ParticipationRoleKey
        {
            get { return this.m_participationRoleKey; }
            set
            {
                this.m_participationRoleKey = value;
                this.m_participationRole = null;
            }
        }

        /// <summary>
        /// Gets or sets the entity which participated in the act
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [DelayLoad(nameof(PlayerEntityKey))]
        public Entity PlayerEntity
        {
            get
            {
                this.m_player = base.DelayLoad(this.m_playerKey, this.m_player);
                return this.m_player;
            }
            set
            {
                this.m_player = value;
                if (value == null)
                    this.m_playerKey = Guid.Empty;
                else
                    this.m_playerKey = value.Key;
            }
        }

        /// <summary>
        /// Gets or sets the role that the entity played in participating in the act
        /// </summary>
        [XmlIgnore, JsonIgnore]
        [DelayLoad(nameof(ParticipationRoleKey))]
        public Concept ParticipationRole
        {
            get
            {
                this.m_participationRole = base.DelayLoad(this.m_participationRoleKey, this.m_participationRole);
                return this.m_participationRole;
            }
            set
            {
                this.m_participationRole = value;
                if (value == null)
                    this.m_participationRoleKey = Guid.Empty;
                else
                    this.m_participationRoleKey = value.Key;
            }
        }

        /// <summary>
        /// Forces a delay load from the underlying model
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
            this.m_participationRole = null;
            this.m_player = null;
        }
    }
}