using System;

namespace ManeProject.Domain.Score
{
    /// <summary>
    /// スコアーインタフェース
    /// </summary>
    public interface IScore
    {
        /// <summary>
        /// 現在スコアー
        /// </summary>
        int m_NowScore { get; }

        /// <summary>
        /// ゲーム終了スコアー
        /// </summary>
        int m_EndGameScore { get; }

        /// <summary>
        /// スコアーアップデート
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        IScore UpdateScore(int score);
    }

    /// <summary>
    /// スコアーファクトリ
    /// </summary>
    public static class ScoreFactory
    {
        /// <summary>
        /// スコアーを生成する
        /// </summary>
        /// <param name="nowScore"></param>
        /// <param name="endGameScore"></param>
        /// <returns></returns>
        public static IScore Create(int nowScore, int endGameScore)
            => new ScoreImpl(nowScore, endGameScore);

        private sealed class ScoreImpl : IScore
        {
            /// <summary>
            /// 現在スコアー
            /// </summary>
            public int m_NowScore { get; }

            /// <summary>
            /// ゲーム終了スコアー
            /// </summary>
            public int m_EndGameScore { get; }

            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="nowScore"></param>
            /// <param name="endGameScore"></param>
            public ScoreImpl(int nowScore, int endGameScore)
                => (m_NowScore, m_EndGameScore) = (nowScore, endGameScore);


            /// <summary>
            /// スコアーアップデート
            /// </summary>
            /// <param name="score"></param>
            /// <returns></returns>
            public IScore UpdateScore(int score) => new ScoreImpl(m_NowScore + score, m_EndGameScore);
        }
    }
}