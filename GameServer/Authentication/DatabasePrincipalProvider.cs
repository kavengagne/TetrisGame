using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using GameData.Contexts;
using GameData.Models;

namespace GameServer.Authentication
{
    public class DatabasePrincipalProvider : IPrincipalProvider
    {
        public IPrincipal CreatePrincipal(string username, string password)
        {
            var loginModel = new UserLoginModel(username, password);
            var context = new ValidationContext(loginModel);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(loginModel, context, results))
            {
                return null;
            }
            using (var db = new StatisticsDbContext())
            {
                var existingUsers = db.Users.Where(user => String.Equals(user.Username, loginModel.Username));
                if (!existingUsers.Any())
                {
                    return null;
                }
                var identity = new GenericIdentity(loginModel.Username);
                IPrincipal principal = new GenericPrincipal(identity, new[] { "User" });
                return principal;
            }
        }
    }
}