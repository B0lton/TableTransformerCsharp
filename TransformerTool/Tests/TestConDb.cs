using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using Tests.ClassiDiTest;
using TransformerTool;

namespace Tests
{
    [TestClass]
    public class TestConDb
    {
        [TestMethod]
        public void ConnessioneDatabase()
        {
            MySqlConnection connection = new MySqlConnection("Server = localhost; Database = prova; Uid = administrator; Pwd = Arcadinoe95; ");

            connection.Open();
            Assert.IsTrue(connection.State == ConnectionState.Open);

            connection.Close();
            Assert.IsTrue(connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public void TransformDataTableFromQuery()
        {
            MySqlConnection connection = new MySqlConnection("Server = localhost; Database = prova; Uid = administrator; Pwd = Arcadinoe95; ");
            connection.Open();

            string query = "select spcod as CodiceSpedizione, datasp as DataAsp, cittadest as CittàDestinatario, cittapart as CittàPartenza, qta_totale as QuantitàTotale  from spedizioni;";
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            connection.Close();

            Transformer<Spedizione> transformer = new Transformer<Spedizione>();
            List<Spedizione> risultato = transformer.Transform(dataTable);

            Assert.IsNotNull(risultato);
            Assert.IsTrue(risultato.Count > 0);

            Assert.IsTrue(dataTable.Rows.Count > 0);




        }
    }
}
