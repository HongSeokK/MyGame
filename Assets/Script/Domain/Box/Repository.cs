using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeProject.Domain.Box
{
    public interface IRepository
    {
        Task<IBoxArray[,]> CreateBoxArray();
    }

    public interface ICache
    {
        bool IsStored { get; }

        IBoxArray[,] BlockArray { get; }

        Task<IBoxArray[,]> Store();

        Task<IBoxArray[,]> GetBlockArray();
    }
}