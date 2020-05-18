using System;

namespace Common
{
    public class RandomImplement : IRandom
    {
        public int Next(int max)
        {
            return new Random().Next(max);
        }

    }
}
