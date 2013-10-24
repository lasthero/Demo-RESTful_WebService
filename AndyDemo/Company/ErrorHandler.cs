using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company
{    
    public class ErrorHandler
    {
        static StringBuilder errMessage = new StringBuilder();

        static ErrorHandler()
        {

        }
        public string ErrorMessage
        {
            get { return errMessage.ToString(); }
            set
            {
                errMessage.AppendLine(value);
            }
        }
    }    
}
