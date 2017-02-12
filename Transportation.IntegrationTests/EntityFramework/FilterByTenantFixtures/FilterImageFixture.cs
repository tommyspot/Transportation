using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using NUnit.Framework;

namespace Epinion.Clarity.IntegrationTests.EntityFramework
{
    public class FilterImageFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Image firstImage;
        Image secondImage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = Presto.Persist<User>();
            firstTenant = user.Tenant;
            firstImage = Presto.Create<Image>();
            firstTenant.Images.Add(firstImage);

            secondTenant = Presto.Persist<Tenant>();
            secondImage = Presto.Create<Image>();
            secondTenant.Images.Add(secondImage);

            ClarityDB.Instance.SaveChanges();

            ClarityDB.CreateInstance(user);
        }

        [Test]
        public void FiltersImage()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Images.Count());
            Assert.AreEqual(firstImage.ID, ClarityDB.Instance.Images.First().ID);
        }

        [Test]
        public void CannotAddImageOfAnotherTenant()
        {
            var image = Presto.Create<Image>(x => { x.TenantID = secondTenant.ID; x.Tenant = secondTenant; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Images.Add(image));
        }

        [Test]
        public void CannotRemoveImageOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Images.Remove(secondImage));
        }
    }
}
