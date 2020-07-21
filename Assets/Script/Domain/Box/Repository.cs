using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Infrastructure.DB;

namespace ManeProject.Domain.Box
{
    public interface IRepository
    {
        Task<IBoxArray[,]> CreateBoxArray();
    }

    /// <summary>
    /// キャッシュレポジトリ
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// キャッシュに保存済みか
        /// </summary>
        bool IsStored { get; }

        IBoxArray[,] GetBlockArray();

        IBoxArray[,] InitBoxArray(DBArray[] DBinfo);
    }
}