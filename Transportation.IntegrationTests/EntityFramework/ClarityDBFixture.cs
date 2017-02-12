using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Diagnostics;
using Conjurer;

namespace Transportation.IntegrationTests.EntityFramework
{
    public class ClarityDBFixture : IntegrationFixture
    {
        [Test]
        public void Add_User()
        {
            var user = Presto.Create<User>();
            ClarityDB.Instance.Users.Add(user);
            ClarityDB.Instance.SaveChanges();
            Assert.That(DatabaseHelper.TableHasRows("Users"));
        }
    }
}
