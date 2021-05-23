using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity_Deep_Dive.Models
{
    public class PractiseUsersStore : IUserStore<PractiseUser>,IUserPasswordStore<PractiseUser>
    {

        //===>Db Connection using Dapper

        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Data Source= DESKTOP-RMKA9B7\\AYAAN;"+
                                                     "database = IdentitytDeepDive;" +
                                                     "trusted_connection = yes;" );;
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async  Task<IdentityResult> CreateAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                     "insert into  PractiseUser([Id]," +
                     "[UserName]," +
                     "[NormalizedUserName]," +
                     "[PasswordHash]) " +
                     "Values(@id,@userName,@normalizedUserName,@passwordHash)",
                     new
                     {
                         id = user.Id,
                         userName = user.UserName,
                         normalizedUserName = user.NormalizedUserName,
                         passwordHash = user.PasswordHash
                     }
                 );

            }
                return IdentityResult.Success;
            


        }

        public async Task<IdentityResult> UpdateAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync(
                    "update PractiseUser " +
                    "set [Id] = @id," +
                    "[UserName] = @userName," +
                    "[NormalizedUserName] = @normalizedUserName," +
                    "[PasswordHash] = @passwordHash " +
                    "where [Id] = @id",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    }
                );
            }

            return IdentityResult.Success;
        }


        public Task<IdentityResult> DeleteAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
           

        public async Task<PractiseUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PractiseUser>(
                    "select * From PractiseUser where Id = @id",
                    new { id = userId });
            }
        }

        public async Task<PractiseUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<PractiseUser>(
                    "select * From PractiseUser where NormalizedUserName = @name",
                    new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(PractiseUser user, string normalizedUserName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedUserName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(PractiseUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;

        }

        public Task SetPasswordHashAsync(PractiseUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(PractiseUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
    }
}
