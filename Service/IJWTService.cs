using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IJWTService
    {
        Task<Tokens> Authenticate(Authentication users);
    }
}
