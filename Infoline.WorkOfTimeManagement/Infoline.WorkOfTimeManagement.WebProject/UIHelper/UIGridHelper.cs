using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.UIHelper
{

    public static class Guid
    {
        public static System.Guid Null { get { return new System.Guid("99999999-1234-5678-0000-999999999999"); } }
    }

    public static class Int32
    {
        public static System.Int32 Null { get { return 1020304050; } }
    }

    public static class boolean
    {
        public static System.Boolean Null { get { return false; } }
    }

    public static class String
    {
        public static System.String Null { get { return System.String.Empty; } }
    }

    public static class Datetime
    {
        public static System.DateTime Null { get { return System.DateTime.MaxValue; } }
    }
    
    public static class Boolean
    {
        public static System.Boolean? Null { get { return null; } }
    }

}