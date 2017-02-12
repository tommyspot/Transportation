using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conjurer;
using NUnit.Framework;

namespace Epinion.Clarity.IntegrationTests.EntityFramework
{
    public class FilterPageFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Page firstPage;
        Page secondPage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            
            firstPage = Presto.Persist<Page>();
            firstTenant = firstPage.Canvas.Tenant;
            
            secondPage = Presto.Persist<Page>();
            secondTenant = secondPage.Canvas.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersPage()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Pages.Count());
            Assert.AreEqual(firstPage.ID, ClarityDB.Instance.Pages.First().ID);
        }

        [Test]
        public void CannotAddPageOfAnotherTenant()
        {
            var canvas = secondTenant.Canvases.First();
            var page = Presto.Create<Page>(x => { x.Canvas = canvas; x.CanvasID = canvas.ID; });

            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Pages.Add(page));
        }

        [Test]
        public void CannotRemovePageOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Pages.Remove(secondPage));
        }
    }
}
