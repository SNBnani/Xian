using System;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data.Sql;

using ClearCanvas.ImageServer.Database;
using ClearCanvas.Enterprise.Core;
using ClearCanvas.Enterprise.Common;

namespace ClearCanvas.ImageServer.Database.SqlServer2005
{
    public abstract class Broker : IPersistenceBroker
    {
        private PersistenceContext _context;

        /// <summary>
        /// Returns the persistence context associated with this broker instance.
        /// </summary>
        protected PersistenceContext Context
        {
            get { return _context; }
        }

        public void SetContext(IPersistenceContext context)
        {
            this._context = (PersistenceContext)context;
        }

        protected void SetParameters(SqlCommand command, ProcedureParameters parms)
        {
            foreach (SearchCriteria parm in parms.SubCriteria.Values)
            {
                String sqlParmName = "@" + parm.GetKey();

                if (parm is ProcedureParameter<DateTime>)
                {
                    ProcedureParameter<DateTime> parm2 = (ProcedureParameter<DateTime>)parm;

                    command.Parameters.AddWithValue(sqlParmName, parm2.Value);
                }
                else if (parm is ProcedureParameter<int>)
                {
                    ProcedureParameter<int> parm2 = (ProcedureParameter<int>)parm;

                    command.Parameters.AddWithValue(sqlParmName, parm2.Value);
                }
                else if (parm is ProcedureParameter<ServerEntityKey>)
                {
                    sqlParmName = sqlParmName.Replace("Ref", "GUID");
                    ProcedureParameter<ServerEntityKey> parm2 = (ProcedureParameter<ServerEntityKey>)parm;

                    command.Parameters.AddWithValue(sqlParmName, parm2.Value.Key);
                }
                else if (parm is ProcedureParameter<bool>)
                {
                    ProcedureParameter<bool> parm2 = (ProcedureParameter<bool>)parm;
                    command.Parameters.AddWithValue(sqlParmName, parm2.Value);
                }
                else if (parm is ProcedureParameter<string>)
                {
                    ProcedureParameter<string> parm2 = (ProcedureParameter<string>)parm;

                    command.Parameters.AddWithValue(sqlParmName, parm2.Value);
                }
                else
                    throw new PersistenceException("Unknown procedure parameter type: " + parm.GetType().ToString(), null);

            }

        }
        protected void PopulateEntity(SqlDataReader reader, ServerEntity entity, Type entityType)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(entity);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                String columnName = reader.GetName(i);
                if (columnName.Equals("GUID"))
                {
                    Guid uid = reader.GetGuid(i);
                    entity.SetKey( new ServerEntityKey(entity.Name,uid) );
                    continue;
                }

                if (columnName.Contains("GUID"))
                    columnName = columnName.Replace("GUID", "Ref");

                PropertyDescriptor prop = props[columnName];
                if (prop == null)
                    throw new EntityNotFoundException("Unable to match column to property: " + columnName, null);

                if (prop.PropertyType == typeof(String))
                    prop.SetValue(entity, reader.GetString(i));
                else if (prop.PropertyType == typeof(Int32))
                    prop.SetValue(entity, reader.GetInt32(i));
                else if (prop.PropertyType == typeof(Int16))
                    prop.SetValue(entity, reader.GetInt16(i));
                else if (prop.PropertyType == typeof(double))
                    prop.SetValue(entity, reader.GetDouble(i));
                else if (prop.PropertyType == typeof(Decimal))
                    prop.SetValue(entity, reader.GetDecimal(i));
                else if (prop.PropertyType == typeof(float))
                    prop.SetValue(entity, reader.GetFloat(i));
                else if (prop.PropertyType == typeof(DateTime))
                    prop.SetValue(entity, reader.GetDateTime(i));
                else if (prop.PropertyType == typeof(ServerEntityKey))
                {
                    Guid uid = reader.GetGuid(i);
                    prop.SetValue(entity, new ServerEntityKey(columnName.Replace("Ref",""), uid));
                }
                else if (prop.PropertyType == typeof(bool))
                    prop.SetValue(entity, reader.GetBoolean(i));
                else
                    throw new EntityNotFoundException("Unsupported property type: " + prop.PropertyType, null);
            }
        }

    }
}
