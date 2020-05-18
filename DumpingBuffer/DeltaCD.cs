using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpingBufferLib
{
    public class DeltaCD
    {
        public int TransactionId { set; get; }

        public CollectionDescription AddCollectionDescription { set; get; }

        public CollectionDescription UpdateCollectionDescription { set; get; }

        public CollectionDescription RemoveCollectionDescription { set; get; }

        public DeltaCD(int transactionId, CollectionDescription addCollectionDescription, CollectionDescription updateCollectionDescription, CollectionDescription removeCollectionDescription)
        {
            TransactionId = transactionId;
            AddCollectionDescription = addCollectionDescription;
            UpdateCollectionDescription = updateCollectionDescription;
            RemoveCollectionDescription = removeCollectionDescription;
        }

        public DeltaCD()
        {
            AddCollectionDescription = null;
            UpdateCollectionDescription = null;
            RemoveCollectionDescription = null;
        }
    }
}
