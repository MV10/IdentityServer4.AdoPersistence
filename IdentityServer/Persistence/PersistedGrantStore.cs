using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

// Global namespace

public class PersistedGrantStore : IPersistedGrantStore
{
    private string connectionString;
    public PersistedGrantStore(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
    {
        var grants = new List<PersistedGrant>();
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using(var cmd = new SqlCommand("SELECT * FROM [Grant] WHERE [SubjectId] = @sub;", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@sub", subjectId));
                var reader = await cmd.ExecuteReaderAsync();
                if(reader.HasRows)
                {
                    var table = new DataTable();
                    table.Load(reader);
                    foreach(DataRow row in table.Rows)
                    {
                        grants.Add(DataToGrant(row));
                    }
                }
                reader.Close();
            }
        }
        return grants;
    }

    public async Task<PersistedGrant> GetAsync(string key)
    {
        PersistedGrant grant = null;
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using(var cmd = new SqlCommand("SELECT * FROM [Grant] WHERE [Key] = @key", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@key", key));
                var reader = await cmd.ExecuteReaderAsync();
                if(reader.HasRows)
                {
                    var table = new DataTable();
                    table.Load(reader);
                    grant = DataToGrant(table.Rows[0]);
                }
                reader.Close();
            }
        }
        return grant;
    }

    public async Task RemoveAllAsync(string subjectId, string clientId)
    {
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using(var cmd = new SqlCommand("DELETE FROM [Grant] WHERE [SubjectId] = @sub AND [ClientId] = @client", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@sub", subjectId));
                cmd.Parameters.Add(new SqlParameter("@client", clientId));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task RemoveAllAsync(string subjectId, string clientId, string type)
    {
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using(var cmd = new SqlCommand("DELETE FROM [Grant] WHERE [SubjectId] = @sub AND [ClientId] = @client AND [Type] = @type", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@sub", subjectId));
                cmd.Parameters.Add(new SqlParameter("@client", clientId));
                cmd.Parameters.Add(new SqlParameter("@type", type));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task RemoveAsync(string key)
    {
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            using(var cmd = new SqlCommand("DELETE FROM [Grant] WHERE [Key] = @key", conn))
            {
                cmd.Parameters.Add(new SqlParameter("@key", key));
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task StoreAsync(PersistedGrant grant)
    {
        using(var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            string upsert =
                $"MERGE [Grant] WITH (ROWLOCK) AS [T] " +
                $"USING (SELECT '{grant.Key}' AS [Key]) AS [S] " +
                $"ON [T].[Key] = [S].[Key] " +
                $"WHEN MATCHED THEN UPDATE SET [ClientId]='{grant.ClientId}', [CreationTime]='{FormatDate(grant.CreationTime)}', [Data]='{grant.Data}', [Expiration]={NullOrDate(grant.Expiration)}, [SubjectId]='{grant.SubjectId}', [Type]='{grant.Type}' " +
                $"WHEN NOT MATCHED THEN INSERT ([Key], [ClientId], [CreationTime], [Data], [Expiration], [SubjectId], [Type]) " +
                $"VALUES ('{grant.Key}','{grant.ClientId}','{FormatDate(grant.CreationTime)}','{grant.Data}',{NullOrDate(grant.Expiration)},'{grant.SubjectId}','{grant.Type}'); ";
            using(var cmd = new SqlCommand(upsert, conn))
            {
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    private string NullOrDate(DateTime? value)
    {
        return (value.HasValue) ? $"'{FormatDate(value.Value)}'" : "null";
    }

    private string FormatDate(DateTime value)
    {
        // UTC ISO 8601 format
        return ((DateTimeOffset)value).ToUniversalTime().ToString("o");
    }

    private PersistedGrant DataToGrant(DataRow row)
    {
        DateTime? expiration = (row["Expiration"] is DBNull) ? null : (DateTime?)row["Expiration"];
        return new PersistedGrant()
        {
            Key = (string)row["Key"],
            ClientId = (string)row["ClientId"],
            CreationTime = (DateTime)row["CreationTime"],
            Data = (string)row["Data"],
            Expiration = expiration,
            SubjectId = (string)row["SubjectId"],
            Type = (string)row["Type"]
        };
    }
}

