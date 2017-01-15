﻿using OpenIZ.Persistence.Data.ADO.Data;
using OpenIZ.Persistence.Data.ADO.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpenIZ.Persistence.Data.ADO.Providers
{
    /// <summary>
    /// Data provider
    /// </summary>
    public interface IDbProvider
    {
        /// <summary>
        /// Readonly (mirror) connection string
        /// </summary>
        ConnectionStringSettings ReadonlyConnectionString { get; set; }

        /// <summary>
        /// Read/write connection string
        /// </summary>
        ConnectionStringSettings ConnectionString { get; set; }

        /// <summary>
        /// Retrieves a readonly connection
        /// </summary>
        DataContext GetReadonlyConnection();

        /// <summary>
        /// Retrieves a read/writer connection
        /// </summary>
        DataContext GetWriteConnection();
        
        /// <summary>
        /// Creates a command on the specified transaction
        /// </summary>
        IDbCommand CreateCommand(DataContext context, SqlStatement stmt);

        /// <summary>
        /// Creates a stored procedure call command
        /// </summary>
        IDbCommand CreateStoredProcedureCommand(DataContext context, String spName, params object[] parms);

        /// <summary>
        /// Create command with specified text and parameters
        /// </summary>
        IDbCommand CreateCommand(DataContext context, String sql, params object[] parms);

        /// <summary>
        /// Generates a SQL statement for the order by condition
        /// </summary>
        SqlStatement OrderBy(SqlStatement sql, String columnName);

        /// <summary>
        /// Generates a SQL statement for the order by condition
        /// </summary>
        SqlStatement OrderByDescending(SqlStatement sql, String columnName);

        /// <summary>
        /// Generates the offset function
        /// </summary>
        SqlStatement Take(SqlStatement sql, int quantity);

        /// <summary>
        /// Generates the offset function
        /// </summary>
        SqlStatement Skip(SqlStatement sql, int offset);
    }
}