using System;
using System.Collections.Generic;
using System.Text;

namespace Insolvency.CalculationsEngine.Redundancy.Common.Exceptions
{
    public class MissingConfigurationException : Exception
    {
        public MissingConfigurationException(string message) : base(message)
        { }
    }
}
