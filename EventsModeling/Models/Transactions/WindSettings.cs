namespace EventsModeling.Models.Transactions
{
    public class WindSettings
    {
        public double Start { get; set; }
        public double End { get; set; }
        public double Step { get; set; }

        public double GetCount() => ((End - Start) / Step) + 1;
    }
}
