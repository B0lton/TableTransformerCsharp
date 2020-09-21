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
    public class Transformer
    {
        /// <summary>
        /// Metodo che trasforma una datatable in oggetti, tipizzati di una classe indicata, per poter essere elaborati
        /// </summary>
        /// <typeparam name="T">T è la classe di cui vogliamo ottenere la trasformazione dalla datatable</typeparam>
        /// <param name="_dataTable">La datatable da trasformare contenente il risultato di una query</param>
        /// <returns>Una lista di oggetti trasformati nella classe indicata. NOTA: ritorna comunque una lista anche in caso di un solo record</returns>
        public List<T> Transform<T>(DataTable _dataTable)
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
            //Questa ricerca della proprietà rende case insensitive la ricerca; ciò è molto utile perchè basta scrivere le proprietà uguali ai campi senza doversi preoccupare di maiuscole o minuscole
            PropertyInfo proprietàDaSettare = oggettoInCuiRicercareLaProprietà.GetType().GetProperties().SingleOrDefault(x => x.Name.Equals(nomeProprietà, StringComparison.InvariantCultureIgnoreCase));
            this.PropertyNullControl(proprietàDaSettare, nomeProprietà, oggettoInCuiRicercareLaProprietà);
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
            this.PropertyNullControl(proprietàDaSettare, nomeProprietà, istanza);

            if(record[nomeCampo] != DBNull.Value)
                proprietàDaSettare.SetValue(istanza, Convert.ChangeType(record[nomeCampo], record[nomeCampo].GetType()));
            else
                proprietàDaSettare.SetValue(istanza, Activator.CreateInstance(proprietàDaSettare.PropertyType));
        }

        private void PropertyNullControl(PropertyInfo proprietà,string nomeProprietà,object oggettoContenenteLaProprietà)
        {
            if (proprietà == null)
                throw new NullReferenceException($"Nessuna proprietà di nome {nomeProprietà} trovata all'interno della classe {oggettoContenenteLaProprietà.GetType().Name}");
        }
    }
}
