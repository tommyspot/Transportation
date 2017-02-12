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
    public class FilterFigureFixture : IntegrationFixture
    {
        Tenant firstTenant;
        Tenant secondTenant;
        Figure firstFigure;
        Figure secondFigure;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            firstFigure = Presto.Persist<Figure>();
            firstTenant = firstFigure.Page.Canvas.Tenant;

            secondFigure = Presto.Persist<Figure>();
            secondTenant = secondFigure.Page.Canvas.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());
        }

        [Test]
        public void FiltersFigure()
        {
            Assert.AreEqual(1, ClarityDB.Instance.Figures.Count());
            Assert.AreEqual(firstFigure.ID, ClarityDB.Instance.Figures.First().ID);
        }

        [Test]
        public void CannotAddFigureOfAnotherTenant()
        {
            var canvas = secondTenant.Canvases.First();
            var page = Presto.Create<Page>(x => {
                x.Canvas = canvas;
                x.CanvasID = canvas.ID;
            });

            var figure = Presto.Create<Figure>(x => { x.Page = page; x.PageID = x.Page.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Figures.Add(figure));
        }

        [Test]
        public void CannotRemoveFigureOfAnotherTenant()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Figures.Remove(secondFigure));
        }

        [Test]
        public void PageIDIsNull_FiltersFigure()
        {
            base.SetUp();

            var firstMenuTile = Presto.Persist<MenuTile>();
            firstFigure = Presto.Persist<Figure>(x => { x.PageID = null; x.Page = null; x.MenuTileID = firstMenuTile.ID; });
            firstTenant = firstMenuTile.Tenant;

            var secondMenuTile = Presto.Persist<MenuTile>();
            secondFigure = Presto.Persist<Figure>(x => { x.PageID = null; x.Page = null; x.MenuTileID = secondMenuTile.ID; });
            secondTenant = secondMenuTile.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());

            Assert.AreEqual(1, ClarityDB.Instance.Figures.Count());
            Assert.AreEqual(firstFigure.ID, ClarityDB.Instance.Figures.First().ID);
        }

        [Test]
        public void KeyFigure_CannotAddFigureOfAnotherTenant()
        {
            base.SetUp();

            var firstMenuTile = Presto.Persist<MenuTile>();
            firstTenant = firstMenuTile.Tenant;

            var secondMenuTile = Presto.Persist<MenuTile>();
            secondTenant = secondMenuTile.Tenant;

            ClarityDB.Instance.SaveChanges();
            ClarityDB.CreateInstance(firstTenant.Users.First());

            var figure = Presto.Create<Figure>(x => { x.PageID = null; x.Page = null; x.MenuTile = secondMenuTile; x.MenuTileID = x.MenuTile.ID; });
            Assert.Throws<ArgumentOutOfRangeException>(() => ClarityDB.Instance.Figures.Add(figure));
        }

    }
}
