using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using Epinion.Clarity.Api;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Data;

namespace Epinion.Clarity.UnitTests.Core.UserFixtures
{
    public class User_FromDataRowFixture: UnitFixture
    {
        private DataRow dataRow;

        [SetUp]
        public void SetUp()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("First name", typeof(string));
            dataTable.Columns.Add("Last name", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Role", typeof(int));

            dataTable.Rows.Add(1, "John", "Hammond", "user1", "Password", "user1@epinion.dk", 0);
            dataRow = dataTable.Rows[0];
        }

        [Test]
        public void ReturnsUserWithID()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual(1, result.ID);
        }

        [Test]
        public void ReturnsUserWithFirstName()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual("John", result.FirstName);
        }

        [Test]
        public void ReturnsUserWithLastName()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual("Hammond", result.LastName);
        }

        [Test]
        public void ReturnsUserWithUserName()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual("user1", result.UserName);
        }

        [Test]
        public void ReturnsUserWithPassword()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual("Password", result.Password);
        }

        [Test]
        public void ReturnsUserWithEmail()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual("user1@epinion.dk", result.Email);
        }

        [Test]
        public void ReturnsUserWithRole()
        {
            var result = User.FromDataRow(dataRow);
            Assert.AreEqual(Role.Normal, result.Role);
        }
    }
}
