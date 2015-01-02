using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersWarRuneBuilder
{

    public class WindowEventArgs : EventArgs
    {
        private string _message;
        public string Message { 
            get 
            { 
                return _message; 
            }
        }

        public WindowEventArgs(string message)
        {
            _message = message;
        }
    }
}
