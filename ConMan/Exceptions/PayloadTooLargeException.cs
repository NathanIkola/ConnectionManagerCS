﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionManager.Exceptions
{
    public class PayloadTooLargeException : Exception
    {
        public PayloadTooLargeException(string msg) : base(msg) { }
    }
}