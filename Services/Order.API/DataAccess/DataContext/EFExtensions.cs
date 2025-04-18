using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data;
using System.Reflection;
using Order.API.Helper;

namespace Order.API.DataAcess.DataContext
{
    public static class EFExtensions
    {
        public static DbCommand LoadStoredProc(this DbContext context, string storedProcName, bool prependDefaultSchema = true, short commandTimeout = 300)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = commandTimeout;

            if (prependDefaultSchema)
            {
                var schemaName = context.Model["DefaultSchema"];
                if (schemaName != null)
                {
                    storedProcName = $"{schemaName}.{storedProcName}";
                }
            }

            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;

            return cmd;
        }

        public static DbCommand LoadCustomQuery(this DbContext context, string query, short commandTimeout = 300)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandTimeout = commandTimeout;

            cmd.CommandText = query;
            cmd.CommandType = CommandType.Text;

            return cmd;
        }

        public static void SetCommonPropertiesForCreate<T>(this T entity, long userId)
        {
            var property = typeof(T).GetProperty("CreatedBy");
            if (property != null)
                property.SetValue(entity, userId);

            property = typeof(T).GetProperty("CreatedDate");
            if (property != null)
                property.SetValue(entity, CommonMethods.GetCurrentTime());

            property = typeof(T).GetProperty("ModifiedBy");
            if (property != null)
                property.SetValue(entity, userId);

            property = typeof(T).GetProperty("ModifiedDate");
            if (property != null)
                property.SetValue(entity, CommonMethods.GetCurrentTime());
        }

        public static void SetCommonPropertiesForUpdate<T>(this T entity, long userId)
        {
            var property = typeof(T).GetProperty("ModifiedBy");
            if (property != null)
                property.SetValue(entity, userId);

            property = typeof(T).GetProperty("ModifiedDate");
            if (property != null)
                property.SetValue(entity, CommonMethods.GetCurrentTime());
        }

        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName, object paramValue,
            Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue ?? DBNull.Value;
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand WithSqlParam(this DbCommand cmd, string paramName,
            Action<DbParameter> configureParam = null)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            var param = cmd.CreateParameter();
            param.ParameterName = paramName;
            configureParam?.Invoke(param);
            cmd.Parameters.Add(param);
            return cmd;
        }

        public static DbCommand WithSqlParam(this DbCommand cmd, IDbDataParameter parameter)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            cmd.Parameters.Add(parameter);

            return cmd;
        }

        public static DbCommand WithSqlParams(this DbCommand cmd, IDbDataParameter[] parameters)
        {
            if (string.IsNullOrEmpty(cmd.CommandText) && cmd.CommandType != CommandType.StoredProcedure)
                throw new InvalidOperationException("Call LoadStoredProc before using this method");

            cmd.Parameters.AddRange(parameters);

            return cmd;
        }

        public class SprocResults
        {
            private readonly DbDataReader _reader;

            public SprocResults(DbDataReader reader)
            {
                _reader = reader;
            }

            public IList<T> ReadToList<T>() where T : new()
            {
                return MapToList<T>(_reader);
            }

            public T? ReadToValue<T>() where T : struct
            {
                return MapToValue<T>(_reader);
            }

            public Task<bool> NextResultAsync()
            {
                return _reader.NextResultAsync();
            }

            public Task<bool> NextResultAsync(CancellationToken ct)
            {
                return _reader.NextResultAsync(ct);
            }

            public bool NextResult()
            {
                return _reader.NextResult();
            }

            private static IList<T> MapToList<T>(DbDataReader dr) where T : new()
            {
                var objList = new List<T>();
                var props = typeof(T).GetRuntimeProperties().ToList();

                var colMapping = dr.GetColumnSchema()
                    .Where(x => props.Any(y =>
                        string.Equals(y.GetCustomAttribute<ColumnAttribute>(true)?.Name ?? y.Name, x.ColumnName, StringComparison.CurrentCultureIgnoreCase)))
                    .ToDictionary(key => key.ColumnName.ToUpper());

                if (!dr.HasRows)
                    return objList;

                while (dr.Read())
                {
                    var obj = new T();
                    foreach (var prop in props)
                    {
                        var upperName = (prop.GetCustomAttribute<ColumnAttribute>(true)?.Name ?? prop.Name).ToUpper();

                        if (!colMapping.ContainsKey(upperName))
                            continue;

                        var column = colMapping[upperName];

                        if (column?.ColumnOrdinal == null)
                            continue;

                        var val = dr.GetValue(column.ColumnOrdinal.Value);
                        prop.SetValue(obj, val == DBNull.Value ? null : val);
                    }

                    objList.Add(obj);
                }

                return objList;
            }

            private static T? MapToValue<T>(DbDataReader dr) where T : struct
            {
                if (!dr.HasRows)
                    return new T?();

                if (dr.Read())
                {
                    return dr.IsDBNull(0) ? new T?() : dr.GetFieldValue<T>(0);
                }

                return new T?();
            }
        }

        public static void ExecuteStoredProc(this DbCommand command, Action<SprocResults> handleResults,
            CommandBehavior commandBehaviour = CommandBehavior.Default,
            bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == ConnectionState.Closed)
                    command.Connection.Open();
                try
                {
                    using (var reader = command.ExecuteReader(commandBehaviour))
                    {
                        var sprocResults = new SprocResults(reader);
                        handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        public static async Task ExecuteStoredProcAsync(this DbCommand command, Action<SprocResults> handleResults,
            CommandBehavior commandBehaviour = CommandBehavior.Default,
            CancellationToken ct = default, bool manageConnection = true)
        {
            if (handleResults == null)
            {
                throw new ArgumentNullException(nameof(handleResults));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync(ct).ConfigureAwait(false);
                try
                {
                    using (var reader = await command.ExecuteReaderAsync(commandBehaviour, ct)
                        .ConfigureAwait(false))
                    {
                        var sprocResults = new SprocResults(reader);
                        handleResults(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        public static async Task ExecuteStoredProcAsync(this DbCommand command,
            CommandBehavior commandBehaviour = CommandBehavior.Default,
            CancellationToken ct = default, bool manageConnection = true, params Action<SprocResults>[] resultActions)
        {
            if (resultActions == null)
            {
                throw new ArgumentNullException(nameof(resultActions));
            }

            using (command)
            {
                if (manageConnection && command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync(ct).ConfigureAwait(false);
                try
                {
                    using (var reader = await command.ExecuteReaderAsync(commandBehaviour, ct)
                        .ConfigureAwait(false))
                    {
                        var sprocResults = new SprocResults(reader);

                        foreach (var t in resultActions)
                            t(sprocResults);
                    }
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }
        }

        public static int ExecuteStoredNonQuery(this DbCommand command, bool manageConnection = true)
        {
            var numberOfRecordsAffected = -1;

            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    command.Connection.Open();
                }

                try
                {
                    numberOfRecordsAffected = command.ExecuteNonQuery();
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }

            return numberOfRecordsAffected;
        }

        public static async Task<int> ExecuteStoredNonQueryAsync(this DbCommand command, CancellationToken ct = default,
            bool manageConnection = true)
        {
            var numberOfRecordsAffected = -1;

            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                {
                    await command.Connection.OpenAsync(ct).ConfigureAwait(false);
                }

                try
                {
                    numberOfRecordsAffected = await command.ExecuteNonQueryAsync(ct).ConfigureAwait(false);
                }
                finally
                {
                    if (manageConnection)
                    {
                        command.Connection.Close();
                    }
                }
            }

            return numberOfRecordsAffected;
        }
    }
}
