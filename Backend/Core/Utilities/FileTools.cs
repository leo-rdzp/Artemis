using Artemis.Backend.Core.DTO.Util;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Artemis.Backend.Core.Utilities
{
    public class FileTools
    {
        private readonly ILogger<FileTools> _logger;
        private const int DEFAULT_START_ROW = 1;
        private const int DEFAULT_START_COLUMN = 1;

        public FileTools(ILogger<FileTools> logger)
        {
            _logger = logger;            
        }

        public async Task<ResultNotifier> CreateExcelFileAsync(ArtExcelWorkbook workbook)
        {
            try
            {
                if (!ValidateWorkbook(workbook, out var validationResult))
                {
                    return validationResult;
                }

                using var excelPackage = CreateExcelPackage(workbook.FileTemplate);
                bool addDateFileName = true;

                foreach (var sheet in workbook.Worksheets!.OrderBy(ws => ws.Index))
                {
                    var processResult = await ProcessWorksheetAsync(excelPackage, sheet, addDateFileName);
                    if (!processResult.ResultStatus.IsPassed)
                    {
                        return processResult;
                    }
                }

                var filePath = await SaveExcelFileAsync(excelPackage, workbook, addDateFileName);

                return ResultNotifier.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Excel file");
                return ResultNotifier.Failure(ex.Message, ex.ToString());
            }
        }

        public async Task<ResultNotifier> CreateCsvFileAsync(ArtExcelWorkbook workbook)
        {
            try
            {
                if (workbook.Worksheets?.Count == 0)
                {
                    return ResultNotifier.Failure("No worksheets found in workbook");
                }

                var sheet = workbook.Worksheets![0];
                var (lines, addDateFileName) = ProcessCsvContent(sheet);
                var filePath = await SaveCsvFileAsync(workbook, lines, addDateFileName);

                return ResultNotifier.Success(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating CSV file");
                return ResultNotifier.Failure(ex.Message);
            }
        }

        private bool ValidateWorkbook(ArtExcelWorkbook workbook, out ResultNotifier result)
        {
            var context = new ValidationContext(workbook);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(workbook, context, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                result = ResultNotifier.Failure($"Error Creating Excel File: {errors}", errors);
                return false;
            }

            foreach (var sheet in workbook.Worksheets!)
            {
                context = new ValidationContext(sheet);
                validationResults = new List<ValidationResult>();

                if (!Validator.TryValidateObject(sheet, context, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(vr => vr.ErrorMessage));
                    result = ResultNotifier.Failure($"Error Creating Excel File ({workbook.FileName}): {errors}", errors);
                    return false;
                }
            }

            result = null!;
            return true;
        }

        private ExcelPackage CreateExcelPackage(string? templatePath)
        {
            if (string.IsNullOrEmpty(templatePath))
            {
                return new ExcelPackage();
            }

            var fileInfo = new FileInfo(templatePath);
            return new ExcelPackage(fileInfo);
        }

        private async Task<ResultNotifier> ProcessWorksheetAsync(
            ExcelPackage package,
            ArtExcelWorksheet sheet,
            bool addDateFileName)
        {
            try
            {
                var startRow = sheet.StartRow ?? DEFAULT_START_ROW;
                var startColumn = sheet.StartColumn ?? DEFAULT_START_COLUMN;
                var rowIndex = startRow != 0 ? startRow + 1 : 2;
                var colIndex = startColumn != 0 ? startColumn : 1;

                var worksheet = GetOrCreateWorksheet(package, sheet);
                var lastCol = ProcessHeaders(worksheet, sheet, startRow, ref colIndex, ref addDateFileName);

                if (sheet.HeaderStyle != null)
                {
                    ApplyStyle(worksheet.Cells[startRow, startColumn, startRow, lastCol], sheet.HeaderStyle);
                }

                var sheetData = ProcessSheetData(sheet, addDateFileName);
                worksheet.Cells[rowIndex, startColumn].LoadFromArrays(sheetData);

                if (sheet.ContentStyle != null)
                {
                    ApplyStyle(
                        worksheet.Cells[startRow + 1, startColumn, startRow + sheetData.Count, lastCol],
                        sheet.ContentStyle);
                }

                await ProcessVariableReplacementsAsync(worksheet, sheet, startRow, lastCol, sheetData.Count);

                return ResultNotifier.Success(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing worksheet");
                throw;
            }
        }

        private ExcelWorksheet GetOrCreateWorksheet(ExcelPackage package, ArtExcelWorksheet sheet)
        {
            var sheetName = sheet.Name ?? $"Sheet {sheet.Index}";
            return package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name.Equals(sheetName))
                ?? package.Workbook.Worksheets.Add(sheetName);
        }

        private int ProcessHeaders(
            ExcelWorksheet worksheet,
            ArtExcelWorksheet sheet,
            int startRow,
            ref int colIndex,
            ref bool addDateFileName)
        {
            foreach (var columnName in sheet.Data![0].Keys)
            {
                if (columnName.Equals("ExcelFileName", StringComparison.OrdinalIgnoreCase))
                {
                    addDateFileName = false;
                    continue;
                }

                worksheet.Cells[startRow, colIndex++].Value = columnName;
            }

            return colIndex - 1;
        }

        private List<object[]> ProcessSheetData(ArtExcelWorksheet sheet, bool addDateFileName)
        {
            if (!addDateFileName)
            {
                sheet.Data!.ForEach(i => i.Remove("ExcelFileName"));
            }

            return sheet.Data!.Select(x => x.Values.ToArray()).ToList();
        }

        private async Task ProcessVariableReplacementsAsync(
            ExcelWorksheet worksheet,
            ArtExcelWorksheet sheet,
            int startRow,
            int lastCol,
            int dataCount)
        {
            if (sheet.ReplaceVariables?.Count == 0) return;

            foreach (var (key, value) in sheet.ReplaceVariables!)
            {
                var searchCells = worksheet.Cells[1, 1, startRow + dataCount, lastCol]
                    .Where(cell => cell.Value?.ToString()?.Contains($"%{key}%") == true)
                    .Select(c => c.Address);

                foreach (var cellAddress in searchCells)
                {
                    var cell = worksheet.Cells[cellAddress];
                    var cellValue = cell.Value?.ToString() ?? string.Empty;
                    cell.Value = cellValue.Replace($"%{key}%", value?.ToString() ?? string.Empty);
                }
            }

            await Task.CompletedTask; // To satisfy async signature
        }

        private void ApplyStyle(ExcelRange range, ArtExcelStyles styles)
        {
            if (!string.IsNullOrEmpty(styles.BackgroundColor))
            {
                range.Style.Fill.PatternType = !string.IsNullOrEmpty(styles.PatternType)
                    ? (ExcelFillStyle)Enum.Parse(typeof(ExcelFillStyle), styles.PatternType)
                    : ExcelFillStyle.Solid;

                range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(styles.BackgroundColor));
            }

            if (!string.IsNullOrEmpty(styles.FontSize))
            {
                range.Style.Font.Size = float.Parse(styles.FontSize);
            }

            if (!string.IsNullOrEmpty(styles.FontName))
            {
                range.Style.Font.Name = styles.FontName;
            }

            if (!string.IsNullOrEmpty(styles.FontBold))
            {
                range.Style.Font.Bold = bool.Parse(styles.FontBold);
            }

            if (!string.IsNullOrEmpty(styles.FontColor))
            {
                range.Style.Font.Color.SetColor(ColorTranslator.FromHtml(styles.FontColor));
            }
        }

        private (List<string> Lines, bool AddDateFileName) ProcessCsvContent(ArtExcelWorksheet sheet)
        {
            var lines = new List<string>();
            var addDateFileName = true;

            // Process headers
            var headerLine = string.Join(",", sheet.Data![0].Keys);
            if (headerLine.Contains("ExcelFileName", StringComparison.OrdinalIgnoreCase))
            {
                addDateFileName = false;
                headerLine = headerLine.Replace(",ExcelFileName", string.Empty);
            }
            lines.Add(headerLine);

            // Process data
            foreach (var data in sheet.Data)
            {
                var lineValues = new List<string>();
                foreach (var (key, value) in data)
                {
                    if (key.Equals("ExcelFileName", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    lineValues.Add(value?.ToString() ?? string.Empty);
                }
                lines.Add(string.Join(",", lineValues));
            }

            return (lines, addDateFileName);
        }

        private async Task<string> SaveExcelFileAsync(
            ExcelPackage package,
            ArtExcelWorkbook workbook,
            bool addDateFileName)
        {
            var fileName = GetFileName(workbook, addDateFileName);
            var fullPath = Path.Combine(workbook.PathToSave!, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            await package.SaveAsAsync(new FileInfo(fullPath));

            if (!File.Exists(fullPath))
            {
                throw new IOException("Failed to create file");
            }

            return fullPath;
        }

        private async Task<string> SaveCsvFileAsync(
            ArtExcelWorkbook workbook,
            List<string> lines,
            bool addDateFileName)
        {
            var fileName = GetFileName(workbook, addDateFileName);
            var fullPath = Path.Combine(workbook.PathToSave!, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            await File.WriteAllLinesAsync(fullPath, lines);

            if (!File.Exists(fullPath))
            {
                throw new IOException("Failed to create file");
            }

            return fullPath;
        }

        private string GetFileName(ArtExcelWorkbook workbook, bool addDateFileName)
        {
            var date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var baseFileName = workbook.FileName!
                .Replace("xlsx", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("xls", string.Empty, StringComparison.OrdinalIgnoreCase)
                .Replace("csv", string.Empty, StringComparison.OrdinalIgnoreCase);

            return addDateFileName
                ? $"{baseFileName}-{date}.{workbook.FileExt}"
                : $"{baseFileName}.{workbook.FileExt}";
        }        
    }
}