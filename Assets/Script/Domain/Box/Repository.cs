using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Infrastructure.DB;
using UnityEngine;

namespace ManeProject.Domain.Box
{
    public interface IRepository
    {
        Task<IBoxArray[,]> CreateBoxArray();

        DeleteResult TryDelete(int row, int column);
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

        IBoxArray[,] InitBoxArray(List<DBArray> DBinfo);

        List<IBoxArray> GetBoxArrayFromList(int row, int column);

        DeleteResult TryDelete(int row, int column);
    }

    public sealed class DeleteResult
    {
        public IBoxArray[,] BoxList { get; set; }

        public bool IsDeleteable { get; set; }
    }
}