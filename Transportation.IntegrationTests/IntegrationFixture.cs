using NUnit.Framework;
using System.Data.Entity;

namespace Transportation.IntegrationTests
{
    [TestFixture(Category = "Integration Test")]
    public abstract class IntegrationFixture
    {
        static IntegrationFixture()
        {
            ConjurerDefinitions.Intialize();
            Database.SetInitializer(new DropCreateDatabaseAlways<ClarityDB>());
        }

        [SetUp]
        public virtual void SetUp()
        {
            ClarityDB.CreateInstance();
            ClarityDB.Instance.Database.Initialize(false);
            DatabaseHelper.ConfigureClarityDB();
            DatabaseHelper.TruncateTables();
        }

        [TearDown]
        public virtual void TearDown()
        {
            ClarityDB.DestroyInstance();
        }
    }
}
