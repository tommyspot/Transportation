using Conjurer;
using Transportation;
using System;
using Transportation.Api.Framework;
using System.Collections.Generic;

public class ConjurerDefinitions
{
    private static bool initialized;

    public static void Intialize()
    {
        if (initialized == false)
        {
            Presto.Sequence.Add<User>();

            Presto.Define<User>(x => {
                var passwordHash = new PasswordHash();
                x.ID = Presto.Sequence.Next<User>();
                x.UserName = "SuperUser";
                x.Salt = passwordHash.CreateSalt();
                x.Password = passwordHash.CreatePasswordHash("Password", x.Salt);
                x.FirstName = "First Name";
                x.LastName = "Last Name";
                x.Role = Role.Normal;
                x.CreatedDate = DateTime.Now;
            });

            Presto.PersistAction = entity => {
                var addEntityMethod = typeof(IClarityDB).GetMethod("AddEntity");
                var addEntityOfTypeMethod = addEntityMethod.MakeGenericMethod(new[] { entity.GetType() });

                addEntityOfTypeMethod.Invoke(ClarityDB.Instance, new object[] { entity });
            };

        }

        initialized = true;
    }
}
