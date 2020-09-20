using System;
using System.Collections.Generic;
using System.Text;
using Tests.ClassiDiTest;

namespace Tests
{
    public class Automobile
    {
        public string Targa { get; set; }
        public int NumeroPorte { get; set; }
        public DateTime DataImmatricolazione { get; set; }
        public Persona Proprietario { get; set; }

        public Automobile() { }

        public Automobile(string targa, int numporte,DateTime dataimmatri,Persona proprietario)
        {
            this.Targa = targa;
            this.NumeroPorte = numporte;
            this.DataImmatricolazione = dataimmatri;
            this.Proprietario = proprietario;
        }
    }
}
