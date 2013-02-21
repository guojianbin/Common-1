﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;


namespace LCW.Framework.Common.DataAccess.Schema
{
    public abstract class SchemaProviderBase
    {
        private DbConnectionStringBuilder connectionstringbuilder;
        public DbConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                return connectionstringbuilder;
            }
            protected set
            {
                connectionstringbuilder = value;
            }
        }

        public abstract IList<DataBaseSchema> GetDataBases(DbConnectionStringBuilder connectionstr);
        public abstract IList<TableSchema> GetTables(DbConnectionStringBuilder connectionstr);
        public abstract IList<ColumnSchema> GetColumns(DbConnectionStringBuilder connectionstr, string tablename);
        public abstract IList<ViewSchema> GetViews(DbConnectionStringBuilder connectionstr);
        public abstract IList<ProceduresSchema> GetProcedures(DbConnectionStringBuilder connectionstr);
        public abstract IList<TriggersSchema> GetTriggers(DbConnectionStringBuilder connectionstr);


    }

    public static class DbConnectionStringBuilderExtend
    {
        public static DbConnectionStringBuilder NewInstance(this DbConnectionStringBuilder builder, string database)
        {
            if (builder is MySql.Data.MySqlClient.MySqlConnectionStringBuilder)
            {
                MySql.Data.MySqlClient.MySqlConnectionStringBuilder temp = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(builder.ConnectionString);
                temp.Database = database;
                return temp;
            }
            SqlConnectionStringBuilder sqltemp = new SqlConnectionStringBuilder(builder.ConnectionString);
            sqltemp.InitialCatalog = database;
            return sqltemp;
        }
        public static string[] DataBase(this DbConnectionStringBuilder builder)
        {
            if (builder is MySql.Data.MySqlClient.MySqlConnectionStringBuilder)
            {
                MySql.Data.MySqlClient.MySqlConnectionStringBuilder temp = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(builder.ConnectionString);
                if (!string.IsNullOrEmpty(temp.Database))
                {
                    return new string[] { temp.Database };
                }
            }
            SqlConnectionStringBuilder sqltemp = new SqlConnectionStringBuilder(builder.ConnectionString);
            if (!string.IsNullOrEmpty(sqltemp.InitialCatalog))
            {
                return new string[] { sqltemp.InitialCatalog };
            }
            return new string[] { null };
        }
    }

}