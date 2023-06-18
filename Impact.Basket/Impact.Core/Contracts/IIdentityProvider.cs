using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Impact.Core.Contracts
{
    public interface IIdentityProvider
    {
        string Email { get; }
        bool IsAuthenticated { get; }
    }
}
