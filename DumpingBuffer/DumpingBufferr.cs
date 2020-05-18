using Common;
using LoggerLib;
using System;
using System.Collections.Generic;

namespace DumpingBufferLib
{
    public class DumpingBufferr
    {
        private CollectionDescription Description = new CollectionDescription();
        private Dictionary<Code, DumpingProperty> WaitingValues { get; set; } = new Dictionary<Code, DumpingProperty>();
        private List<Code> SavedCodes { get; set; } = new List<Code>();

        private uint valuesCount = 0;
        private bool IsAddedCode2;
        private bool IsAddedCode1;

        public DeltaCD ReceiveValues(Tuple<Code, Value> incoomming)
        {
            valuesCount++;

            var property = PackToDumpingProperty(incoomming);
            WaitingValues[property.Code] = property;
            Logger.Instance.Log($"DumpingBuffer received: {incoomming.Item1} {incoomming.Item2}");

            if (valuesCount % 10 == 0)
            {
                Description = new CollectionDescription();
                FindDataSet(out var saveForLaterAdding1, out var saveForLaterAdding2);

                AddToCdIfPossible(ref saveForLaterAdding1, ref saveForLaterAdding2);

                return CheckAndSend();
            }
            else
            {
                return null;
            }
        }

        private void AddToCdIfPossible(ref DumpingProperty saveForLaterAdding1, ref DumpingProperty saveForLaterAdding2)
        {
            if (saveForLaterAdding1 != null)
            {
                Description.DumpingPropertyCollection.Clear();

                Description.DumpingPropertyCollection.Add(saveForLaterAdding1);
                Description.DumpingPropertyCollection.Add(saveForLaterAdding2);

                SetDataSet(saveForLaterAdding1);

                WaitingValues.Remove(saveForLaterAdding1.Code);
                WaitingValues.Remove(saveForLaterAdding2.Code);

                CheckSavedCodes(saveForLaterAdding1, saveForLaterAdding2);

                saveForLaterAdding1 = null;
                saveForLaterAdding2 = null;
            }
        }

        private void SetDataSet(DumpingProperty saveForLaterAdding1)
        {
            switch (saveForLaterAdding1.Code)
            {
                case Code.CODE_DIGITAL:
                case Code.CODE_ANALOG:
                    Description.DataSet = 1;
                    break;
                case Code.CODE_CUSTOM:
                case Code.CODE_LIMITSET:
                    Description.DataSet = 2;
                    break;
                case Code.CODE_SINGLENOE:
                case Code.CODE_MULTIPLENODE:
                    Description.DataSet = 3;
                    break;
                case Code.CODE_CONSUMER:
                case Code.CODE_SOURCE:
                    Description.DataSet = 4;
                    break;
                case Code.CODE_MOTION:
                case Code.CODE_SENSOR:
                    Description.DataSet = 5;
                    break;
            }
        }

        private void CheckSavedCodes(DumpingProperty saveForLaterAdding1, DumpingProperty saveForLaterAdding2)
        {
            if (SavedCodes.Contains(saveForLaterAdding1.Code))
            {
                IsAddedCode1 = true;
            }
            else
            {
                SavedCodes.Add(saveForLaterAdding1.Code);
                IsAddedCode1 = false;
            }

            if (SavedCodes.Contains(saveForLaterAdding2.Code))
            {
                IsAddedCode2 = true;
            }
            else
            {
                SavedCodes.Add(saveForLaterAdding2.Code);
                IsAddedCode2 = false;
            }
        }

        private void FindDataSet(out DumpingProperty saveForLaterAdding1, out DumpingProperty saveForLaterAdding2)
        {
            Code oposite;
            saveForLaterAdding1 = null;
            saveForLaterAdding2 = null;
            foreach (var item in WaitingValues)
            {
                oposite = Datasets.FindOpositeInSet(item.Key);
                foreach (var item2 in WaitingValues)
                {
                    if (oposite == item2.Key)
                    {
                        saveForLaterAdding1 = item.Value;
                        saveForLaterAdding2 = item2.Value;
                        break;
                    }
                }
                if (saveForLaterAdding1 != null && saveForLaterAdding2 != null)
                {
                    break;
                }
            }
        }

        private DeltaCD CheckAndSend()
        {
            var deltaCD = WriteToHistory();
            ResetDescription();
            return deltaCD;
        }

        private void ResetDescription()
        {
            Description.DumpingPropertyCollection.Clear();
            Description.DataSet = 0;
        }

        // odlaganje za jos 10 po potrebi
        private DeltaCD WriteToHistory()
        {
            return PackDescriptionToDeltaCD();
        }

        // treba odluciti da li ide u add, update ili delete
        // u zavisnosti od toga da li vrednost vec postoji u Description-u
        private DeltaCD PackDescriptionToDeltaCD()
        {
            var deltaCD = new DeltaCD();
            if (IsAddedCode1 && IsAddedCode2)
            {
                deltaCD.UpdateCollectionDescription = new CollectionDescription(Description.Id, Description.DataSet, Description.DumpingPropertyCollection);
                return new DeltaCD(deltaCD.TransactionId, deltaCD.AddCollectionDescription, deltaCD.UpdateCollectionDescription, deltaCD.RemoveCollectionDescription);
            }
            else
            {
                deltaCD.AddCollectionDescription = new CollectionDescription(Description.Id, Description.DataSet, Description.DumpingPropertyCollection);
                return new DeltaCD(deltaCD.TransactionId, deltaCD.AddCollectionDescription, deltaCD.UpdateCollectionDescription, deltaCD.RemoveCollectionDescription);
            }
        }

        // dodeli jedinstveni id
        // dodeli data set na osnovu code-a
        private DumpingProperty PackToDumpingProperty(Tuple<Code, Value> incoomming)
        {
            return new DumpingProperty(incoomming.Item1, incoomming.Item2);
        }
    }
}
