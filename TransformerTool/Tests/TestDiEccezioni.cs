using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransformerTool;

namespace Tests
{
    [TestClass]
    public class TestDiEccezioni
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "ColonnaNonTrovata non appartiene alla classe da trasformare")]
        public void ColonnaNonTrovataInNessunaProprietà_Livello1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("ColonnaNonAttesa", typeof(string));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["ColonnaNonAttesa"] = "Questa colonna non è mappata con nessuna proprietà nella classe";
            dt.Rows.Add(row);

            Transformer t = new Transformer();
            List<Automobile> risultato = t.Transform<Automobile>(dt);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "ColonnaNonTrovata non appartiene alla classe innestata in secondo livello da trasformare")]
        public void ColonnaNonTrovataInNessunaProprietà_Livello2()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("Proprietario.Nome", typeof(string)); dt.Columns.Add("Proprietario.ColonnaNonAttesa", typeof(string));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["Proprietario.Nome"] = "Alessandro Zuccolo";
            row["Proprietario.ColonnaNonAttesa"] = "Questa colonna non è una proprietà di Proprietario";
            dt.Rows.Add(row);

            Transformer t = new Transformer();
            List<Automobile> risultato = t.Transform<Automobile>(dt);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "ColonnaNonTrovata non appartiene alla classe innestata in secondo livello da trasformare")]
        public void ColonnaNonTrovataInNessunaProprietà_Livello3()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("Proprietario.Nome", typeof(string)); dt.Columns.Add("Proprietario.IndirizzoScrittoMale.NumeroCivico", typeof(int));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["Proprietario.Nome"] = "Alessandro Zuccolo";
            row["Proprietario.IndirizzoScrittoMale.NumeroCivico"] = 7;
            dt.Rows.Add(row);

            Transformer t = new Transformer();
            List<Automobile> risultato = t.Transform<Automobile>(dt);

        }

    }
}
