using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{
    internal class Screen
    {
        public string Result { get; set; }
        public string CurrOprator { get; set; }
        public Screen() 
        { 
            Result = string.Empty;
            CurrOprator = string.Empty;
        }
    }
}
