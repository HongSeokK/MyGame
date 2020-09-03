using ManeProject.Domain.Score;
using System;


namespace ManeProject.Infrastructure.Repository.Cache
{
    public static class ScoreCache
    {
        public static ICache Instance => instance.Value;

        private sealed class ScoreCacheImpl : ICache
        {
            private IScore CacheScore;

            /// <summary>
            /// スコアーが生成されているのか
            /// </summary>
            public bool IsCreated { get; private set; }

            public void Dispose()
            {
                CacheScore = null;
                IsCreated = false;
            }

            /// <summary>
            /// スコアー初期化
            /// </summary>
            /// <returns></returns>
            public IScore InitScore(int EndGameScore)
            {
                CacheScore = ScoreFactory.Create(0, EndGameScore);

                IsCreated = true;

                return CacheScore;
            }

            /// <summary>
            /// スコアー更新
            /// </summary>
            /// <param name="Score"></param>
            /// <returns></returns>
            public IScore UpdateScore(int Score)
            {
                if (IsCreated)
                {
                    CacheScore = CacheScore.UpdateScore(Score);

                    return CacheScore;
                }

                throw new NullReferenceException("キャッシュが生成されてないです");
            }

            /// <summary>
            /// 現在スコアーを取ってくる
            /// </summary>
            /// <returns></returns>
            public IScore GetScore() => IsCreated ? CacheScore : throw new NullReferenceException("キャッシュが生成されてないです");
        }
        private static readonly Lazy<ScoreCacheImpl> instance = new Lazy<ScoreCacheImpl>(() => new ScoreCacheImpl());
    }
}