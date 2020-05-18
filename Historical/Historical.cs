using Common;
using DumpingBufferLib;
using HistoricalLib.Database;
using LoggerLib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HistoricalLib
{
    public class Historical
    {
        private int id = -1;

        public int Id
        {
            get
            {
                if (id < 0)
                {
                    id = 0;
                }
                else
                {
                    id++;
                }
                return id;
            }
        }

        public List<Description> ListDescriptions { get; set; }

        public void ReceiveValue(DeltaCD deltaCD)
        {
            ListDescriptions = PackDescription(deltaCD);

            foreach (var Item2 in from item in ListDescriptions from Item2 in item.HistoricalProperties select Item2)
            {
                Logger.Instance.Log($"Historical received: {Item2.Code} {Item2.HistoricalValue}");
            }

            if (!ValidateDatasets())
            {
                Logger.Instance.Log("Invalid dataset");
                throw new ArgumentException("DeltaCD does not have valid datasets");
            }

            CheckDeadband(ListDescriptions);

            WriteToDatabase(ListDescriptions);
        }

        public void WriteToDatabase(List<Description> listDescriptions)
        {
            using (var db = new ModelContext())
            {
                foreach (var item in listDescriptions)
                {
                    db.DataModels.AddRange(ConvertDescriptionToModel(item));
                }

                db.SaveChanges();
                foreach (var item2 in from item in listDescriptions from item2 in item.HistoricalProperties select item2)
                {
                    Logger.Instance.Log($"Added to database: {item2.Code} {item2.HistoricalValue}");
                }
            }
        }

        public List<Model> ReadFromDatabase(DateTime d1, DateTime d2)
        {
            using (var db = new ModelContext())
            {

            }
            var lista = new List<Model>();
            var sqlConnection = new SqlConnection();
            var sqlCommand = new SqlCommand();
            string address = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Baza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            sqlConnection.ConnectionString = address;

            sqlCommand.Connection = sqlConnection;

            string komanda = "SELECT * FROM Models ";

            // string komanda = "SELECT sp.name , sp.default_database_name FROM sys.server_principals sp WHERE sp.name = SUSER_SNAME()";
            sqlCommand.CommandText = komanda;
            try
            {
                sqlConnection.Open();
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var model = new Model(int.Parse(reader["ID"].ToString()), new Value(reader["Value_DateOfCreation"] as DateTime?, reader["Value_IdGeographicArea"] as string, int.Parse(reader["Value_Consumption"].ToString())), (Code)Enum.Parse(typeof(Code), reader["Code"].ToString()), reader["TimeStamp"] as DateTime?);
                    lista.Add(model);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            sqlConnection.Close();

            return lista;
        }

        private List<Model> ConvertDescriptionToModel(Description description)
        {
            var model = new List<Model>();
            foreach (var item in description.HistoricalProperties)
            {
                model.Add(new Model(Id, item.HistoricalValue, item.Code, DateTime.Now));
            }
            return model;
        }

        // Deadband iznosi 2% i značiće da ukoliko pristigli podatak, već postoji u bazi, ukoliko nova njegova
        // vrednost je veća od 2% od stare vrednosti, tada će biti upisana nova vrednost. Ukoliko nova vrednost ne
        // izlazi iz okvira od 2% od stare vrednosti tada nova vrednost ne treba da bude upisana u bazu podataka.
        // Jedini izuzetak iz Deadband-a je Code – CODE_DIGITAL, za ovaj Code se uvek upisuje prosleđena
        // vrednost i ne proverava se Deadband.
        private void CheckDeadband(List<Description> descriptions)
        {
            var databaseValues = ReadFromDatabase(DateTime.MinValue, DateTime.MaxValue).Select(i => i.Value).ToList();

            var deletions = new List<HistoricalProperty>();

            foreach (var item in descriptions)
            {
                foreach (var property in item.HistoricalProperties)
                {
                    if (property.Code == Code.CODE_DIGITAL)
                    {
                        continue;
                    }
                    var newVal = property.HistoricalValue;
                    if (databaseValues.Select(f => f.IdGeographicArea == newVal.IdGeographicArea).FirstOrDefault())
                    {
                        var oldVal = databaseValues.FindAll(i => i.IdGeographicArea == newVal.IdGeographicArea).LastOrDefault();
                        var percentDiff = CalculatePercentageDifferance(newVal, oldVal);
                        if (percentDiff <= 2)
                        {
                            deletions.Add(property);
                            Logger.Instance.Log($"Outside of deadband: {property.Code} new val = {newVal}, old val = {oldVal}");
                        }
                    }
                }
            }

            foreach (var item in deletions)
            {
                foreach (var item2 in descriptions)
                {
                    var del = new List<int>();

                    for (int i = 0; i < item2.HistoricalProperties.Count; i++)
                    {
                        if (item.Code == item2.HistoricalProperties[i].Code)
                        {
                            del.Add(i);
                            break;
                        }
                    }

                    foreach (int delItem in del)
                    {
                        item2.HistoricalProperties.RemoveAt(delItem);
                    }
                }
            }
        }

        private static double CalculatePercentageDifferance(Value newVal, Value oldVal)
        {
            var abs = Math.Abs(newVal.Consumption - oldVal.Consumption);
            double avg = (newVal.Consumption + oldVal.Consumption) / 2;
            double diff = abs / avg;
            return diff * 100;
        }

        // Historical komponenta treba da proveri da li su podaci validni – da li su dataset-ovi odgovarajući i u
        // skladu sa Code-ovima koji su prosleđeni u okviru dataset-a.
        private bool ValidateDatasets()
        {
            foreach (var item in ListDescriptions)
            {
                switch (item.Dataset)
                {
                    case 1:
                        if (!CheckProperties(item.HistoricalProperties, Code.CODE_ANALOG, Code.CODE_DIGITAL))
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (!CheckProperties(item.HistoricalProperties, Code.CODE_CUSTOM, Code.CODE_LIMITSET))
                        {
                            return false;
                        }
                        break;
                    case 3:
                        if (!CheckProperties(item.HistoricalProperties, Code.CODE_SINGLENOE, Code.CODE_MULTIPLENODE))
                        {
                            return false;
                        }
                        break;
                    case 4:
                        if (!CheckProperties(item.HistoricalProperties, Code.CODE_CONSUMER, Code.CODE_SOURCE))
                        {
                            return false;
                        }
                        break;
                    case 5:
                        if (!CheckProperties(item.HistoricalProperties, Code.CODE_MOTION, Code.CODE_SENSOR))
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        private bool CheckProperties(List<HistoricalProperty> properties, Code code1, Code code2)
        {
            foreach (var item in properties)
            {
                if (item.Code == code1 || item.Code == code2)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        private List<Description> PackDescription(DeltaCD deltaCD)
        {
            var ListDescriptions = new List<Description>();
            if (deltaCD.AddCollectionDescription != null)
            {
                var addCollectionDescription = deltaCD.AddCollectionDescription;
                ListDescriptions.Add(new Description(addCollectionDescription.Id, addCollectionDescription.DataSet, ConvertDumpingPropertyToHistoricalProperty(addCollectionDescription.DumpingPropertyCollection)));
            }
            else if (deltaCD.UpdateCollectionDescription != null)
            {
                var updateCollectionDescription = deltaCD.UpdateCollectionDescription;
                ListDescriptions.Add(new Description(updateCollectionDescription.Id, updateCollectionDescription.DataSet, ConvertDumpingPropertyToHistoricalProperty(updateCollectionDescription.DumpingPropertyCollection)));
            }
            else if (deltaCD.RemoveCollectionDescription != null)
            {
                var removeCollectionDescription = deltaCD.RemoveCollectionDescription;
                ListDescriptions.Add(new Description(removeCollectionDescription.Id, removeCollectionDescription.DataSet, ConvertDumpingPropertyToHistoricalProperty(removeCollectionDescription.DumpingPropertyCollection)));
            }
            return ListDescriptions;
        }

        private List<HistoricalProperty> ConvertDumpingPropertyToHistoricalProperty(List<DumpingProperty> dumpingPropertyCollection)
        {
            var retval = new List<HistoricalProperty>();
            foreach (var item in dumpingPropertyCollection)
            {
                retval.Add(new HistoricalProperty(item.Code, item.DumpingValue));
            }
            return retval;
        }
    }
}
