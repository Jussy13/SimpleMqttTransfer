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
            var raw = 1;

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
                    PrintStatistics(worksheet, ref raw);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                Console.WriteLine($"Finished at {endOfExecution}");

                ResultsCollector.Clear();
                EventsCollector.Clear();
                TransactionsQueue.Clear();
                raw++;
            }
            worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            worksheet.Style.Font.FontSize = 10;
            worksheet.Style.Font.FontName = "Arial";
            workbook.SaveAs("Results.xlsx");
            workbook.Dispose();
        }

        private static void PrintStatistics(IXLWorksheet worksheet, ref int raw)
        {
            worksheet.Cell($"A{raw}").Value = "Name";
            worksheet.Cell($"B{raw}").Value = "Cores";
            worksheet.Cell($"C{raw}").Value = "Ram";
            worksheet.Cell($"D{raw}").Value = "Created";
            worksheet.Cell($"E{raw}").Value = "Handled";
            worksheet.Cell($"F{raw}").Value = "Avg-time";
            worksheet.Cell($"G{raw}").Value = "TimeToCalc";
            worksheet.Cell($"H{raw}").Value = "Percent";
            worksheet.Cell($"I{raw}").Value = "Interval";

            raw++;
            foreach (var result in ResultsCollector.GetResults())
            {
                worksheet.Cell($"A{raw}").Value = result.Key;
                worksheet.Cell($"B{raw}").Value = result.Value.CoresCount;
                worksheet.Cell($"C{raw}").Value = result.Value.RamCount;
                worksheet.Cell($"D{raw}").Value = result.Value.CreatedTransactionsCount;
                worksheet.Cell($"E{raw}").Value = result.Value.HandledTransactionCount;
                worksheet.Cell($"F{raw}").Value = result.Value.AvgTransactionCalcTime;
                worksheet.Cell($"G{raw}").Value = result.Value.CalculationTime;
                worksheet.Cell($"H{raw}").Value = TransactionHelper.FreqByType[result.Key] * 100.0;
                worksheet.Cell($"I{raw}").Value = $"M: {AppSettingsProvider.TransactionDelayMean} b: {AppSettingsProvider.TransactionDelaySigma}";
                raw++;
            }
        }
    }
}
