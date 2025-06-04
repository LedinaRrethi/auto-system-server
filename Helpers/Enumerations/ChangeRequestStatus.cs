using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Enumerations
{
    public enum ChangeRequestStatus : byte
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
