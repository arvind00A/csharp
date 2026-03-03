using System;
using System.Collections.Generic;
using System.Text;

namespace MethodsParams
{
    internal class ParamenterPassing
    {
        public void Increment(int x)
        {
            x++;
        }

        public void IncrementRef(ref int x)
        {
            x++;
        }

        public void Calculate(int a, int b, out int sum, out int diff)
        {
            sum = a + b;        // Must assign value
            diff = a - b;       // Must assign value
        }
    }
}
