using Common;
using System.Collections.Generic;

namespace DumpingBufferLib
{
    public class Datasets
    {
        private static List<Set> Sets = new List<Set>()
        {
            new Set(Code.CODE_ANALOG, Code.CODE_DIGITAL),
            new Set(Code.CODE_CUSTOM, Code.CODE_LIMITSET),
            new Set(Code.CODE_SINGLENOE, Code.CODE_MULTIPLENODE),
            new Set(Code.CODE_CONSUMER, Code.CODE_SOURCE),
            new Set(Code.CODE_MOTION, Code.CODE_SENSOR)
        };

        public static Code FindOpositeInSet(Code code)
        {
            foreach (var item in Sets)
            {
                if (item.Code == code)
                {
                    return item.Oposite;
                }
                if (item.Oposite == code)
                {
                    return item.Code;
                }
            }

            return new Code();
        }
    }
}
