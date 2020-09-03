using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Infrastructure.DB;
using UnityEngine;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// レポジトリ
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// ボックス配列生成
        /// </summary>
        /// <returns></returns>
        Task<IBoxArray[,]> CreateBoxArray();

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="row">行指定</param>
        /// <param name="column">列指定</param>
        /// <returns></returns>
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

        /// <summary>
        /// 現在ボックス配列を呼び出す
        /// </summary>
        /// <returns></returns>
        IBoxArray[,] GetBlockArray();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBinfo"></param>
        /// <returns></returns>
        IBoxArray[,] InitBoxArray(List<DBArray> DBinfo);

        /// <summary>
        /// 指定した行、列でのボックスグループを呼び出す
        /// </summary>
        /// <param name="row">行指定</param>
        /// <param name="column">列指定</param>
        /// <returns></returns>
        List<IBoxArray> GetBoxArrayFromList(int row, int column);

        /// <summary>
        /// 削除実行
        /// </summary>
        /// <param name="row">行指定</param>
        /// <param name="column">列指定</param>
        /// <returns></returns>
        DeleteResult TryDelete(int row, int column);
    }

    /// <summary>
    /// 削除結果
    /// </summary>
    public sealed class DeleteResult
    {
        /// <summary>
        /// ボックス配列
        /// </summary>
        public IBoxArray[,] BoxList { get; set; }

        /// <summary>
        /// 削除されたボックスの数
        /// </summary>
        public int DeletedCount { get; set; }

        /// <summary>
        /// 削除されたのか
        /// </summary>
        public bool isDeleted { get; set; }
    }
}