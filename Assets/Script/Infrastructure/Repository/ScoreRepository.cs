using System;
using ManeProject.Domain.Score;
using ManeProject.Infrastructure.Repository.Cache;

namespace ManeProject.Infrastructure.Repository
{
    public static class ScoreRepository
    {
        public static IRepository Instance => instance.Value;

        private static readonly Lazy<IRepository> instance = new Lazy<IRepository>(() => new ScoreRepositoryImpl(ScoreCache.Instance));


        private sealed class ScoreRepositoryImpl : IRepository
        {
            private readonly ICache m_cache;

            public ScoreRepositoryImpl(ICache cache) => m_cache = cache;

            /// <summary>
            /// スコアー生成
            /// </summary>
            /// <returns></returns>
            public IScore CreateScore(int EndGameScore) => m_cache.InitScore(EndGameScore);

            /// <summary>
            /// スコアー更新
            /// </summary>
            /// <param name="Score"></param>
            /// <returns></returns>
            public IScore UpdateScore(int Score) => m_cache.UpdateScore(Score);

            /// <summary>
            /// キャッシュから現在スコアーを取ってくる
            /// </summary>
            /// <returns></returns>
            public IScore GetScore() => m_cache.GetScore();
        }
    }
}