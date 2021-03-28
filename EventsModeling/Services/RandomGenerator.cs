using System;

namespace EventsModeling.Services
{
    public class RandomGenerator
    {
        private readonly Random _randGen = new Random();

        public double Generate(double mu, double sigma)
        {
            var dSumm = 0.0;
            for (var i = 0; i <= 12; i++)
                dSumm += _randGen.NextDouble();

            return Math.Round((mu + sigma * (dSumm - 6)), 3);
        }
    }
}
