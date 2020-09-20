using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace TransformerTool
{
    /// <summary>
    /// Classe che permette di trasformare automaticamente il risultato di una query salvato in una datatable, in oggetti di classi inizializzati in base ai record
    /// della datatable i cui campi sono le proprietà delle classi. L'unico requisito affinchè funzioni è che se, per esempio, una classe A ha come proprietà un riferimento
    /// ad un'altra classe B allora si procede così:
    /// Classe A (Proprietà Nome, Proprietà B)
    /// Classe B (Proprietà Codice)
    /// Allora, se si vuole settare il Codice della classe B, nell'elenco di selezione della query, questo campo si dovrà scrivere "B.Codice"
    /// 
    /// ----------------------------
    /// Credits to @Alessandro Zuccolo
    /// </summary>
    /// <typeparam name="T">T è il tipo del/degli oggetto/i che si vuole/vogliono ottenere</typeparam>
    public class Transformer<T>
    {
        public List<T> Transform(DataTable _dataTable)
        {
            List<T> risultato = new List<T>();
            foreach (DataRow record in _dataTable.Rows)
            {
                T istanza = Activator.CreateInstance<T>();
                foreach (DataColumn campo in _dataTable.Columns)
                {
                    //Se nel nome campo troviamo una cosa come: Proprietario.Nome , vuol dire che 
                    //la proprietà da settare è la prop Nome dentro la classe rappreseentata da Proprietario(cioè Persona)
                    string nomeCampo = campo.ColumnName;
                    bool isProprietàTipoNonPrimitivo = false;
                    if (nomeCampo.Contains("."))
                    {
                        List<string> nomeCampi = nomeCampo.Split('.').ToList<string>();
                        int index = 1;
                        object oggettoInCuiRicercareLaProprietà = istanza;
                        foreach (string nomeProprietà in nomeCampi)
                        {
                            if (index == nomeCampi.Count)
                            {
                                this.SettaProprietàPrimitiva(oggettoInCuiRicercareLaProprietà, nomeProprietà,nomeCampo, record);
                                isProprietàTipoNonPrimitivo = true;
                                break;
                            }
                            oggettoInCuiRicercareLaProprietà = this.GetOggettoSottolivello(oggettoInCuiRicercareLaProprietà, nomeProprietà);
                            index++;
                        }
                    }

                    if(!isProprietàTipoNonPrimitivo)
                    this.SettaProprietàPrimitiva(istanza, campo.ColumnName,campo.ColumnName, record);
                }

                risultato.Add(istanza);
            }

            return risultato;
        }

        /// <summary>
        /// Riga 1: cerca la proprietà da settare di tipo non primitivo nell'oggetto passato come parametro
        /// Riga 2: definisce di che tipo di dato(classe) della proprietà cercata prima
        /// Riga 3-6: se la proprietà è un oggetto già istanziato allora salta, altrimenti lo istanzia
        /// Riga 7:ritorna l'oggetto a cui si riferisce la proprietà
        /// </summary>
        /// <param name="oggettoInCuiRicercareLaProprietà">Oggetto padre di cui bisogna cercare la proprietà non di tipo primitivo</param>
        /// <param name="nomeProprietà">il nome della proprietà da ricercare</param>
        /// <returns>l'oggetto rappresentato dalla proprietà cercata</returns>
        private object GetOggettoSottolivello(object oggettoInCuiRicercareLaProprietà, string nomeProprietà)
        {
            PropertyInfo proprietàDaSettare = oggettoInCuiRicercareLaProprietà.GetType().GetProperty(nomeProprietà);
            Type t = proprietàDaSettare.PropertyType;
            if (proprietàDaSettare.GetValue(oggettoInCuiRicercareLaProprietà) == null)
            {
                proprietàDaSettare.SetValue(oggettoInCuiRicercareLaProprietà, Activator.CreateInstance(t));
            }
            return proprietàDaSettare.GetValue(oggettoInCuiRicercareLaProprietà);
        }

        /// <summary>
        /// Metodo che si occupa di settare il valore della proprietà ricercata
        /// </summary>
        /// <param name="istanza">Oggetto di cui si vuole settare una proprietà</param>
        /// <param name="nomeProprietà">Il nome della proprietà da settare</param>
        /// <param name="nomeCampo">Il nome del campo nella tabella di riferimento(ad esempio, di una datatable) NOTA: può differire con il nomeProprietà</param>
        /// <param name="record">La riga di cui si vuole estrapolare il valore del campo da settare alla proprietà</param>
        private void SettaProprietàPrimitiva(object istanza,string nomeProprietà,string nomeCampo,DataRow record)
        {
            PropertyInfo proprietàDaSettare = istanza.GetType().GetProperty(nomeProprietà);
            string nomeTipoProprietàDaSettare = proprietàDaSettare.PropertyType.Name.ToLower();
            switch (nomeTipoProprietàDaSettare)
            {
                case "string":
                    if(record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, record[nomeCampo].ToString());
                    else
                        proprietàDaSettare.SetValue(istanza, string.Empty);
                    break;
                case "int16":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToInt16(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, 0 as Int16);
                    break;
                case "int32":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToInt32(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, 0 as Int32);
                    break;
                case "int64":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToInt64(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, 0 as Int64);
                    break;
                case "decimal":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToDecimal(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, Decimal.Zero);
                    break;
                case "double":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToDouble(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, 0);
                    break;
                case "float":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToSingle(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, 0);
                    break;
                case "datetime":
                    if (record[nomeCampo] != DBNull.Value)
                        proprietàDaSettare.SetValue(istanza, Convert.ToDateTime(record[nomeCampo]));
                    else
                        proprietàDaSettare.SetValue(istanza, DateTime.MinValue);
                    break;
                default:
                    break;
            }
        }
    }
}
