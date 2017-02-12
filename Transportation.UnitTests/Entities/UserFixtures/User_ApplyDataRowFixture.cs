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
    public class User_ApplyDataRowFixture: UnitFixture
    {
        private DataRow dataRow;
        private User user;

        [SetUp]
        public void SetUp()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(string));
            dataTable.Columns.Add("First name", typeof(string));
            dataTable.Columns.Add("Last name", typeof(string));
            dataTable.Columns.Add("Username", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Role", typeof(string));

            dataTable.Rows.Add(1, "John", "Hammond", "user1", "Password", "user1@epinion.dk", 0);
            dataRow = dataTable.Rows[0];

            user = new User();
        }

        [Test]
        public void SetsID()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual(1, user.ID);
        }

        [Test]
        public void SetsID_Is0_WrongFormat()
        {
            dataRow["Id"] = "Id_1";
            user.ApplyDataRow(dataRow);
            Assert.AreEqual(0, user.ID);
        }

        [Test]
        public void SetsFirstName()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual("John", user.FirstName);
        }

        [Test]
        public void SetsLastName()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual("Hammond", user.LastName);
        }

        [Test]
        public void SetsEmail()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual("user1@epinion.dk", user.Email);
        }

        [Test]
        public void SetsUserName()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual("user1", user.UserName);
        }

        [Test]
        public void SetsPassword()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual("Password", user.Password);
        }

        [Test]
        public void SetsRole()
        {
            user.ApplyDataRow(dataRow);
            Assert.AreEqual(Role.Normal, user.Role);
        }
    }
}
