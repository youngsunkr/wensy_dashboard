using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicePoint.Lib
{
    public struct MemberGrade
    {
        public enum Type : int
        {
            Operator = 1,
            Manager = 8,
            Owner = 9
        }
    }
}
