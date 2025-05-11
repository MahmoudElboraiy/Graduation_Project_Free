using DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityManager.Services.Interfaces
{
    public interface ILanguageDbContextAccessor
    {
        ApplicationDbContext GetDbContext();
    }
}
