using System.Collections.Generic;
using EventsModeling.Models.Transactions;

namespace EventsModeling.Services.Transactions
{
    public static class TransactionHelper
    {
        public static Dictionary<string, TransactionSettings> SettingsByType { get; }
        public static Dictionary<string, double> FreqByType { get; set; }
        public static List<List<int>> RequiredCoresSets { get; }
        public static List<int> CurRequiredCoresSet { get; set; }

        static TransactionHelper()
        {
            SettingsByType = new Dictionary<string, TransactionSettings>();
            RequiredCoresSets = new List<List<int>>();
        }
    }
}
