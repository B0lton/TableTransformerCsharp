using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransformerTool;

namespace Tests
{
    [TestClass]
    public class TestLivelli
    {
        [TestMethod]
        public void TestPrincipaleLivello1()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            dt.Rows.Add(row);

            Transformer<Automobile> t = new Transformer<Automobile>();
            List<Automobile> risultato = t.Transform(dt);

            Assert.AreEqual(risultato[0].Targa, "aaa");
            Assert.AreEqual(risultato[0].NumeroPorte, 3);

        }

        [TestMethod]
        public void TestPrincipaleLivello2()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("Proprietario.Nome", typeof(string));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["Proprietario.Nome"] = "Alessandro Zuccolo";
            dt.Rows.Add(row);

            Transformer<Automobile> t = new Transformer<Automobile>();
            List<Automobile> risultato = t.Transform(dt);

            Assert.AreEqual("aaa",risultato[0].Targa);
            Assert.AreEqual(3, risultato[0].NumeroPorte);
            Assert.IsNotNull(risultato[0].Proprietario);
            Assert.AreEqual("Alessandro Zuccolo", risultato[0].Proprietario.Nome);

        }

        [TestMethod]
        public void TestPrincipaleLivello3()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("Proprietario.Nome", typeof(string)); dt.Columns.Add("Proprietario.Indirizzo.NumeroCivico", typeof(int));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["Proprietario.Nome"] = "Alessandro Zuccolo";
            row["Proprietario.Indirizzo.NumeroCivico"] = 7;
            dt.Rows.Add(row);

            Transformer<Automobile> t = new Transformer<Automobile>();
            List<Automobile> risultato = t.Transform(dt);

            Assert.AreEqual("aaa", risultato[0].Targa);
            Assert.AreEqual(3, risultato[0].NumeroPorte);
            Assert.IsNotNull(risultato[0].Proprietario);
            Assert.AreEqual("Alessandro Zuccolo", risultato[0].Proprietario.Nome);
            Assert.IsNotNull(risultato[0].Proprietario.Indirizzo);
            Assert.AreEqual(7, risultato[0].Proprietario.Indirizzo.NumeroCivico);

        }

        [TestMethod]
        public void TestPrincipaleLivello4()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Targa", typeof(string)); dt.Columns.Add("NumeroPorte", typeof(int)); dt.Columns.Add("DataImmatricolazione", typeof(DateTime));
            dt.Columns.Add("Proprietario.Nome", typeof(string)); dt.Columns.Add("Proprietario.Indirizzo.NumeroCivico", typeof(int));
            dt.Columns.Add("Proprietario.Indirizzo.Paese.CodicePostale", typeof(int));
            var row = dt.NewRow();
            row["Targa"] = "aaa";
            row["NumeroPorte"] = 3;
            row["DataImmatricolazione"] = DateTime.Now;
            row["Proprietario.Nome"] = "Alessandro Zuccolo";
            row["Proprietario.Indirizzo.NumeroCivico"] = 7;
            row["Proprietario.Indirizzo.Paese.CodicePostale"] = 27029;
            dt.Rows.Add(row);

            Transformer<Automobile> t = new Transformer<Automobile>();
            List<Automobile> risultato = t.Transform(dt);

            Assert.AreEqual("aaa", risultato[0].Targa);
            Assert.AreEqual(3, risultato[0].NumeroPorte);
            Assert.IsNotNull(risultato[0].Proprietario);
            Assert.AreEqual("Alessandro Zuccolo", risultato[0].Proprietario.Nome);
            Assert.IsNotNull(risultato[0].Proprietario.Indirizzo);
            Assert.AreEqual(7, risultato[0].Proprietario.Indirizzo.NumeroCivico);
            Assert.IsNotNull(risultato[0].Proprietario.Indirizzo.Paese);
            Assert.AreEqual(27029, risultato[0].Proprietario.Indirizzo.Paese.CodicePostale);

        }

    }
}
