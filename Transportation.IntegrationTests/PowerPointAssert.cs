using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using NUnit.Framework;
using Epinion.Clarity.ReportBuilders;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;

namespace Epinion.Clarity.IntegrationTests
{
    public class PowerPointAssert
    {
        public static void SlidesCount(string filePath, int numberOfSlides)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                SlideIdList slideIdList = document.PresentationPart.Presentation.SlideIdList;
                Assert.AreEqual(numberOfSlides, slideIdList.Count());
            }
        }

        public static void ShapeExists(string filePath, string shapeName)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var shape = document.GetShapeByName(shapeName);
                Assert.IsNotNull(shape);
            }
        }

        public static void DoesNotExist(string filePath, string shapeName)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var shape = document.GetShapeByName(shapeName);
                Assert.IsNull(shape);
            }
        }

        public static void PieChartExists(string filePath, string shapeName)
        {
            ChartExists<PieChart>(filePath, shapeName);
        }

        public static void BarChartExists(string filePath, string shapeName)
        {
            ChartExists<BarChart>(filePath, shapeName);
        }

        public static void LineChartExists(string filePath, string shapeName)
        {
            ChartExists<LineChart>(filePath, shapeName);
        }

        private static void ChartExists<T>(string filePath, string graphicFrameName) where T : OpenXmlElement
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var shape = document.GetGraphicFrameByName(graphicFrameName);
                var chartReference = shape.Descendants<DocumentFormat.OpenXml.Drawing.Charts.ChartReference>().FirstOrDefault();
                var chartPart = (ChartPart)shape.Ancestors<Slide>().First().SlidePart.GetPartById(chartReference.Id);
                var chart = chartPart.ChartSpace.Descendants<T>().First();

                Assert.IsNotNull(chart);
            }
        }

        public static void TableExists(string filePath, string graphicFrameName)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var graphic = document.GetGraphicFrameByName(graphicFrameName);
                var table = graphic.Descendants<Table>().FirstOrDefault();
                Assert.IsNotNull(table);
            }
        }

        public static void GraphicFrameExists(string filePath, string graphicFrameName)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var graphic = document.GetGraphicFrameByName(graphicFrameName);
                Assert.IsNotNull(graphic);
            }
        }

        public static void GraphicFrameHasTransform2D(string filePath, string graphicFrameName, Transform2D transform)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var graphic = document.GetGraphicFrameByName(graphicFrameName);
                var gfTransform = graphic.Transform;

                Assert.AreEqual(transform.Offset.X, gfTransform.Offset.X);
                Assert.AreEqual(transform.Offset.Y, gfTransform.Offset.Y);
                Assert.AreEqual(transform.Extents.Cx, gfTransform.Extents.Cx);
                Assert.AreEqual(transform.Extents.Cy, gfTransform.Extents.Cy);
            }
        }

        public static void ContainsText(string filePath, string shapeName, string expectedText)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var textbox = document.GetShapeByName(shapeName);
                var text = "";
                var runs = textbox.TextBody.Descendants<Run>();

                if (runs != null && runs.Count() > 0)
                {
                    foreach (var run in runs)
                    {
                        if (run != null)
                        {
                            text += run.Text.Text;
                        }
                    }
                }
                Assert.AreEqual(expectedText, text);
            }
        }

        public static void ContainsCellText(string filePath, string tableGraphicFrameName, string expectedText)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var table = document.GetGraphicFrameByName(tableGraphicFrameName);
                var text = "";
                var runs = table.Descendants<Run>();

                if (runs != null && runs.Count() > 0)
                {
                    foreach (var run in runs)
                    {
                        if (run != null)
                        {
                            text += run.Text.Text;
                        }
                    }
                }
                Assert.AreEqual(expectedText, text);
            }
        }

        public static void ContainsNoSmokingShapeWithText(string filePath, string expectedText)
        {
            using (PresentationDocument document = PresentationDocument.Open(filePath, false))
            {
                var shape = document.GetShapeByType(ShapeTypeValues.NoSmoking);
                var text = "";
                var runs = shape.TextBody.Descendants<Run>();

                if (runs != null && runs.Count() > 0)
                {
                    foreach (var run in runs)
                    {
                        if (run != null)
                        {
                            text += run.Text.Text;
                        }
                    }
                }
                Assert.AreEqual(expectedText, text);
            }
        }
    }

    public static class PowerPointAssertHelper {

        public static DocumentFormat.OpenXml.Presentation.GraphicFrame GetGraphicFrameByName(this PresentationDocument document, string graphicFrameName)
        {
            var presentationPart = document.PresentationPart;
            var slideIdList = presentationPart.Presentation.SlideIdList;

            foreach (var slideId in slideIdList.Elements<SlideId>())
            {
                SlidePart slidePart = (SlidePart)(presentationPart.GetPartById(slideId.RelationshipId.ToString()));

                if (slidePart != null)
                {
                    Slide slide = slidePart.Slide;
                    if (slide != null)
                    {
                        var chart = slidePart.Slide.Descendants<DocumentFormat.OpenXml.Presentation.GraphicFrame>()
                                                   .Where(g => g.NonVisualGraphicFrameProperties.NonVisualDrawingProperties.Name == graphicFrameName)
                                                   .FirstOrDefault();
                        if (chart != null)
                        {
                            return chart;
                        }
                    }
                }
            }

            return null;
        }

        public static DocumentFormat.OpenXml.Presentation.Shape GetShapeByType(this PresentationDocument document, ShapeTypeValues shapeType)
        {
            var presentationPart = document.PresentationPart;
            var slideIdList = presentationPart.Presentation.SlideIdList;

            foreach (var slideId in slideIdList.Elements<SlideId>())
            {
                SlidePart slidePart = (SlidePart)(presentationPart.GetPartById(slideId.RelationshipId.ToString()));

                if (slidePart != null)
                {
                    Slide slide = slidePart.Slide;
                    if (slide != null)
                    {
                        var shape = slidePart.Slide.Descendants<DocumentFormat.OpenXml.Presentation.Shape>()
                                                   .Where(x => x.ShapeProperties.Elements<PresetGeometry>().Any( p => p.Preset == shapeType))
                                                   .FirstOrDefault();
                        if (shape != null)
                        {
                            return shape;
                        }
                    }
                }
            }

            return null;
        }
}
}
