using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NUnit.Framework;

namespace Epinion.Clarity.IntegrationTests
{
    public class ExcelAssert
    {
        public static void TableDimensionHasReference(string filePath, string cellRange)
        {
            using (var document = SpreadsheetDocument.Open(filePath, true))
            {
                var worksheetPart = document.WorkbookPart.WorksheetParts.First();
                Assert.AreEqual(cellRange, worksheetPart.TableDefinitionParts.First().Table.Reference.Value);
            }
        }

        public static void CheckCellValue(string filePath, string cellReference, bool isNumeric, string cellValue)
        {
            using (var document = SpreadsheetDocument.Open(filePath, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();

                var cell = sheetData.Descendants<Cell>().Where(x => x.CellReference.Value == cellReference).FirstOrDefault();

                Assert.IsNotNull(cell);

                if (isNumeric)
                {
                    Assert.AreEqual(cellValue, cell.CellValue.InnerText);
                }
                else
                {
                    Assert.AreEqual(CellValues.SharedString, cell.DataType.Value);
                    Assert.AreEqual(cellValue, GetSharedStringItem(sharedStringTable, int.Parse(cell.CellValue.InnerText)));
                }
            }
        }

        public static void CheckCellValue(string filePath, string sheetName, string cellReference, bool isNumeric, string cellValue)
        {
            using (var document = SpreadsheetDocument.Open(filePath, true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                SharedStringTable sharedStringTable = workbookPart.SharedStringTablePart.SharedStringTable;
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var cell = sheetData.Descendants<Cell>().Where(x => x.CellReference.Value == cellReference).FirstOrDefault();

                Assert.IsNotNull(cell);

                if (isNumeric)
                {
                    Assert.AreEqual(cellValue, cell.CellValue.InnerText);
                }
                else
                {
                    Assert.AreEqual(cellValue, GetSharedStringItem(sharedStringTable, int.Parse(cell.CellValue.InnerText)));
                }
            }
        }

        private static string GetSharedStringItem(SharedStringTable sharedStringTable, int index)
        {
            var item = sharedStringTable.Elements<SharedStringItem>().ElementAt(index);
            return item.InnerText;
        }

        public static void HasSheet(string filePath, string sheetName)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheetCount = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).Count();
                Assert.AreEqual(1, sheetCount);
            }
        }

        public static void DoesNotHaveSheet(string filePath, string sheetName)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheetCount = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).Count();
                Assert.AreEqual(0, sheetCount);
            }
        }

        public static void RowsCount(string filePath, string sheetName, int rowsCount)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var count = sheetData.Descendants<Row>().Count();
                Assert.AreEqual(rowsCount, count);
            }
        }

        public static void CellsCount(string filePath, string sheetName, int rowIndex, int cellsCount)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var row = sheetData.Descendants<Row>().ElementAt(rowIndex);
                var count = row.Descendants<Cell>().Count();
                Assert.AreEqual(cellsCount, count);
            }
        }

        public static void CellHasValueType(string filePath, string sheetName, string cellReference, CellValues cellValueType)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var cell = sheetData.Descendants<Cell>().Where(x => x.CellReference.Value == cellReference).FirstOrDefault();

                Assert.AreEqual(cellValueType, cell.DataType.Value);
            }
        }

        public static void CellHasStyleIndex(string filePath, string sheetName, string cellReference, uint cellStyleIndex)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                WorksheetPart worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                var cell = sheetData.Descendants<Cell>().Where(x => x.CellReference.Value == cellReference).FirstOrDefault();

                Assert.AreEqual(cellStyleIndex, cell.StyleIndex.Value);
            }
        }

        public static void HasMergeCells(string filePath, string sheetName, string cellRange)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                var mergeCellCount = worksheetPart.Worksheet.Descendants<MergeCell>().Where(x => x.Reference == cellRange).Count();

                Assert.AreEqual(1, mergeCellCount);
            }
        }

        public static void HasNoMergeCells(string filePath, string sheetName)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                var mergeCellCount = worksheetPart.Worksheet.Descendants<MergeCells>().Count();

                Assert.AreEqual(0, mergeCellCount);
            }
        }

        public static void MergeCellsCount(string filePath, string sheetName, int count)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheet = document.WorkbookPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).First();
                var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);
                var mergeCellCount = worksheetPart.Worksheet.Descendants<MergeCell>().Count();

                Assert.AreEqual(count, mergeCellCount);
            }
        }

        public static void SheetCount(string filePath, int expectedSheetCount)
        {
            using (var document = SpreadsheetDocument.Open(filePath, false))
            {
                var sheetCount = document.WorkbookPart.Workbook.Descendants<Sheet>().Count();
                Assert.AreEqual(expectedSheetCount, sheetCount);
            }
        }
    }
}
