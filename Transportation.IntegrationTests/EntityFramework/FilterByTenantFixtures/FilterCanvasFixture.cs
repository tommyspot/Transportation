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
    public class FilterCanvasFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Canvas firstCanvas;
        Canvas secondCanvas;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstCanvas = Presto.Persist<Canvas>();
            firstTenant = firstCanvas.Tenant;

            secondCanvas = Presto.Persist<Canvas>();
            secondTenant = secondCanvas.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersCanvas()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Canvases.Count());
            Assert.AreEqual(firstCanvas.ID, ClarityDB.Instance.Canvases.First().ID);
        }

        [Test]
        public void CannotAddCanvasOfAnotherTenant()
        {
            var canvas = Presto.Create<Canvas>(x => { x.Tenant = secondTenant; x.TenantID = x.Tenant.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Canvases.Add(canvas));
        }

        [Test]
        public void CannotRemoveCanvasOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Canvases.Remove(secondCanvas));
        }
    }
}
