using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.ClassiDiTest
{
    public class Persona
    {
        public string Nome { get; set; }
        public int Età { get; set; }
        public Indirizzo Indirizzo { get; set; }

        public Persona() { }

        //public Persona(string nome,int età)
        //{
        //    this.Nome = nome;
        //    this.Età = età;
        //}
    }
}
