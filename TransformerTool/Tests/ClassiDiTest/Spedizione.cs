using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.ClassiDiTest
{
    public class Spedizione
    {
        public string CodiceSpedizione { get; set; }
        public DateTime DataAsp { get; set; }
        public string CittàDestinatario { get; set; }
        public string CittàPartenza { get; set; }
        public decimal QuantitàTotale { get; set; }
    }
}
