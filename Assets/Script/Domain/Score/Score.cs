using System;

namespace ManeProject.Domain.Score
{
    public interface IScore
    {
        int m_NowScore { get; }

        int m_EndGameScore { get; }

        /// <summary>
        /// スコアーアップデート
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        IScore UpdateScore(int score);
    }

    public static class ScoreFactory
    {
        public static IScore Create(int nowScore, int endGameScore)
            => new ScoreImpl(nowScore, endGameScore);

        private sealed class ScoreImpl : IScore
        {
            public int m_NowScore { get; }

            public int m_EndGameScore { get; }

            public ScoreImpl(int nowScore, int endGameScore)
                => (m_NowScore, m_EndGameScore) = (nowScore, endGameScore);


            public IScore UpdateScore(int score) => new ScoreImpl(m_NowScore + score, m_EndGameScore);
        }
    }
}