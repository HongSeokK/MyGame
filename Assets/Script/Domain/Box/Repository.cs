using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeProject.Domain.Box
{
    public interface IRepository
    {
        IBoxArray CreateBoxArray();
        Task<List<IBoxArray>>  CreateRandomType(int count);
    }
}