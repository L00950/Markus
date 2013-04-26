using System;

namespace MarkusModel
{
    [Serializable]
    public class ClaesStatistik
    {
        public ÅrsPremier[] Premier { get; set; }
        public Värde Värde { get; set; }
        public MånadsPremier[] MånadsPremier { get; set; }
    }

    [Serializable]
    public class MånadsPremier
    {
        public bool ÅretsVärde { get; set; }
        public int Månad { get; set; }
        public double Premier { get; set; }
    }

    [Serializable]
    public class ÅrsPremier
    {
        public bool ÅretsVärde { get; set; }
        public double DepåBank { get; set; }
        public double DepåMäklare { get; set; }
        public double FondBank { get; set; }
        public double FondMäklare { get; set; }
        public double Kryss { get; set; }
        public double EstimatHelår { get; set; }
        public double BankPremier
        {
            get { return DepåBank + FondBank; }
        }
        public double MäklarPremier
        {
            get { return DepåMäklare + FondMäklare; }
        }
        public double TotalaPremier
        {
            get { return BankPremier + MäklarPremier + Kryss; }
        }
        public double DepåPremier
        {
            get { return DepåBank + DepåMäklare; }
        }
        public double FondPremier
        {
            get { return FondBank + FondMäklare; }
        }
    }

    [Serializable]
    public class Värde
    {
        public double FondBank { get; set; }
        public double FondMäklare { get; set; }
        public double DepåMäklare { get; set; }
        public double DepåBank { get; set; }
        public double Kryss { get; set; }

        public double Bank
        {
            get { return FondBank + DepåBank; }
        }
        public double Mäklare
        {
            get { return FondMäklare + DepåMäklare; }
        }
        public double Totalt
        {
            get { return Bank + Mäklare + Kryss; }
        }
    }
}
