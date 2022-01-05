using System;
using System.Linq;

namespace Bank.Application.Tests.Factory
{
    public static class OutOfRangeEnumFactory
    {
        public static int Generate<TEnum>() => Enum.GetValues(typeof(TEnum)).Cast<int>().Max() + 1;
    }
}
