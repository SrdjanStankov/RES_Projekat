using System;

namespace Common
{
    public class Value
    {
        public Value(DateTime? timeStamp, string idGeographicArea, int consumption)
        {
            DateOfCreation = timeStamp;
            IdGeographicArea = idGeographicArea;
            Consumption = consumption;
        }

        public DateTime? DateOfCreation { get; set; }
        public string IdGeographicArea { get; set; }
        public int Consumption { get; set; }  // in mW/h

        public override bool Equals(object obj)
        {
            var value = obj as Value;

            if (IdGeographicArea != value.IdGeographicArea)
            {
                return false;
            }

            if (Consumption != value.Consumption)
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            return $"{DateOfCreation.Value.ToLongDateString()} {DateOfCreation.Value.ToLongTimeString()}, {IdGeographicArea}, {Consumption}";
        }
    }
}
