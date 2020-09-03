using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Infrastructure.DB;
using UnityEngine;

namespace ManeProject.Domain.Score
{
    public interface IRepository
    {
        /// <summary>
        /// スコアー生成
        /// </summary>
        /// <returns></returns>
        IScore CreateScore(int EndGameScore);

        /// <summary>
        /// スコアー更新
        /// </summary>
        /// <param name="Score"></param>
        /// <returns></returns>
        IScore UpdateScore(int Score);

        /// <summary>
        /// キャッシュから現在スコアーを取ってくる
        /// </summary>
        /// <returns></returns>
        IScore GetScore();
    }

    public interface ICache : IDisposable
    {
        /// <summary>
        /// スコアーが生成されているのか
        /// </summary>
        bool IsCreated { get; }

        /// <summary>
        /// スコアー初期化
        /// </summary>
        /// <returns></returns>
        IScore InitScore(int EndGameScore);

        /// <summary>
        /// スコアー更新
        /// </summary>
        /// <param name="Score"></param>
        /// <returns></returns>
        IScore UpdateScore(int Score);

        /// <summary>
        /// 現在スコアーを取ってくる
        /// </summary>
        /// <returns></returns>
        IScore GetScore();
    }
}