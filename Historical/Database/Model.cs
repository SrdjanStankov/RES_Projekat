using Common;
using System;

namespace HistoricalLib.Database
{
    public class Model
    {
        public Model(int id, Value value, Code code, DateTime? timeStamp)
        {
            Id = id;
            Value = value;
            Code = code;
            TimeStamp = timeStamp;
        }

        public int Id { get; set; }
        public Value Value { get; set; }
        public Code Code { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
