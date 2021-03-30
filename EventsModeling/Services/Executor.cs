using System;
using ClosedXML.Excel;
using EventsModeling.Resources;
using EventsModeling.Services.Handlers;
using EventsModeling.Services.Transactions;
using EventsModeling.Settings;

namespace EventsModeling.Services
{
    public class Executor
    {
        public static DateTime ExecutionTime;
        public static readonly ResultsCollector ResultsCollector;
        private readonly IEventHandler _eventHandler;

        static Executor() => ResultsCollector = new ResultsCollector();
        public Executor(IEventHandler eventHandler) => _eventHandler = eventHandler;

        public void Execute()
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sample");
            var curRaw = 1;

            foreach (var set in TransactionHelper.RequiredCoresSets)
            {
                ServerResources.InitServerResources(AppSettingsProvider.CoresCount, AppSettingsProvider.RamCount);
                ExecutionTime = DateTime.Now;
                var endOfExecution = ExecutionTime + TimeSpan.FromMinutes(AppSettingsProvider.ModelTime);
                TransactionHelper.CurRequiredCoresSet = set;

                Console.WriteLine($"Started at {ExecutionTime}");
                Console.WriteLine("Cores " + string.Join('-', set));
                try
                {
                    while (ExecutionTime < endOfExecution)
                        _eventHandler.HandleEvent(EventsCollector.GetEvent());
                    PrintStatistics(worksheet, curRaw);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine($"Finished at {endOfExecution}");

                ResultsCollector.Clear();
                EventsCollector.Clear();
                TransactionsQueue.Clear();
                ++curRaw;
            }
            worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            worksheet.Style.Font.FontSize = 10;
            worksheet.Style.Font.FontName = "Arial";
            workbook.SaveAs("Results.xlsx");
            workbook.Dispose();
        }

        private static void PrintStatistics(IXLWorksheet worksheet, int curRaw)
        {
            var raw = curRaw;
            var col = 0;
            var lastCol = 0;

            foreach (var result in ResultsCollector.GetResults())
            {
                if (raw == 1)
                {
                    worksheet.Cell(raw, ++col).Value = "Name";
                    worksheet.Cell(raw, ++col).Value = "Cores";
                    worksheet.Cell(raw, ++col).Value = "Ram";
                    worksheet.Cell(raw, ++col).Value = "Created";
                    worksheet.Cell(raw, ++col).Value = "Handled";
                    worksheet.Cell(raw, ++col).Value = "Avg-time";
                    worksheet.Cell(raw, ++col).Value = "TimeToCalc";
                    worksheet.Cell(raw, ++col).Value = "Percent";
                    worksheet.Cell(raw, ++col).Value = "Interval";
                    worksheet.Cell(raw, ++col).Value = "Effective";
                }
                raw++;
                col = lastCol;
                worksheet.Cell(raw, ++col).Value = result.Key;
                worksheet.Cell(raw, ++col).Value = result.Value.CoresCount;
                worksheet.Cell(raw, ++col).Value = result.Value.RamCount;
                worksheet.Cell(raw, ++col).Value = result.Value.CreatedTransactionsCount;
                worksheet.Cell(raw, ++col).Value = result.Value.HandledTransactionCount;
                worksheet.Cell(raw, ++col).Value = result.Value.AvgTransactionCalcTime;
                worksheet.Cell(raw, ++col).Value = result.Value.CalculationTime;
                worksheet.Cell(raw, ++col).Value = TransactionHelper.FreqByType[result.Key] * 100.0;
                worksheet.Cell(raw, ++col).Value = $"M: {AppSettingsProvider.TransactionDelayMean} b: {AppSettingsProvider.TransactionDelaySigma}";
                worksheet.Cell(raw, ++col).Value = (double) result.Value.HandledTransactionCount / result.Value.CreatedTransactionsCount;
                raw = curRaw;
                ++col;
                lastCol = col;
            }
        }
    }
}
