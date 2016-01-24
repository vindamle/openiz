﻿using OpenIZ.Core.Model;
using OpenIZ.Core.Model.Acts;
using OpenIZ.Core.Model.Collection;
using OpenIZ.Core.Model.DataTypes;
using OpenIZ.Core.Model.Entities;
using OpenIZ.Core.Model.Roles;
using OpenIZ.Messaging.IMSI.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace OpenIZ.Messaging.IMSI.Wcf
{
    /// <summary>
    /// The IMSI service interface
    /// </summary>
    [ServiceContract(Namespace = "http://openiz.org/imsi/1.0", Name = "IMSI", ConfigurationName = "IMSI_1.0")]
    [XmlSerializerFormat]
    [ServiceKnownType(typeof(Concept))]
    [ServiceKnownType(typeof(ReferenceTerm))]
    [ServiceKnownType(typeof(Act))]
    [ServiceKnownType(typeof(TextObservation))]
    [ServiceKnownType(typeof(CodedObservation))]
    [ServiceKnownType(typeof(QuantityObservation))]
    [ServiceKnownType(typeof(PatientEncounter))]
    [ServiceKnownType(typeof(SubstanceAdministration))]
    [ServiceKnownType(typeof(Entity))]
    [ServiceKnownType(typeof(Patient))]
    [ServiceKnownType(typeof(Provider))]
    [ServiceKnownType(typeof(Organization))]
    [ServiceKnownType(typeof(Place))]
    [ServiceKnownType(typeof(Material))]
    [ServiceKnownType(typeof(ManufacturedMaterial))]
    [ServiceKnownType(typeof(DeviceEntity))]
    [ServiceKnownType(typeof(ApplicationEntity))]
    [ServiceKnownType(typeof(DeviceEntity))]
    [ServiceKnownType(typeof(Bundle))]
    [ServiceKnownType(typeof(ErrorResult))]
    public interface IImsiServiceContract 
    {

        /// <summary>
        /// Search for the specified resource type
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData Search(string resourceType);

        /// <summary>
        /// Get the specified resource
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData Get(string resourceType, string id);

        /// <summary>
        /// Get history of an object
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/history", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData History(string resourceType, string id);

        /// <summary>
        /// Get a specific version
        /// </summary>
        [WebGet(UriTemplate = "/{resourceType}/{id}/history/{versionId}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData GetVersion(string resourceType, string id, string versionId);

        /// <summary>
        /// Create the resource
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/{resourceType}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData Create(string resourceType, IdentifiedData body);

        /// <summary>
        /// Update the specified resource
        /// </summary>
        [WebInvoke(Method = "PUT", UriTemplate = "/{resourceType}/{id}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData Update(string resourceType, string id, IdentifiedData body);

        /// <summary>
        /// Creates a resource or updates one
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/{resourceType}/{id}", BodyStyle = WebMessageBodyStyle.Bare)]
        IdentifiedData CreateUpdate(string resourceType, string id, IdentifiedData body);

        /// <summary>
        /// Get the current time
        /// </summary>
        /// <returns></returns>
        [WebGet(UriTemplate = "/time")]
        DateTime Time();

        /// <summary>
        /// Get the schema
        /// </summary>
        [WebGet(UriTemplate = "/?xsd={schemaId}")]
        XmlSchema GetSchema(int schemaId);
    }
}
