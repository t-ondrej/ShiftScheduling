using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftScheduleAlgorithm.Validation
{

    interface I
    {
        
    }

    class ValidationOutput
    {
        private IDictionary<Type, IEnumerable<Type>> Warnings;
    }
}
