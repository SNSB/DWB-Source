using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DWBServices.WebServices
{
    public class DWBServiceException : Exception
    {
        public DWBServiceException(string message) : base(message) { }
        public DWBServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
    public class DataMappingException : Exception
    {
        public DataMappingException(string message) : base(message) { }
        public DataMappingException(string message, Exception innerException) : base(message, innerException) { }
    }
}
