using System;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// 球の属性
    /// </summary>
    public static class BoxType
    {
        /// <summary>
        /// 青
        /// </summary>
        public static IType Blue = blue.Value;

        /// <summary>
        /// 赤
        /// </summary>
        public static IType Red = red.Value;

        /// <summary>
        /// 黄色
        /// </summary>
        public static IType Yellow = yellow.Value;

        /// <summary>
        /// タイプインタフェース
        /// </summary>
        public interface IType
        {
            /// <summary>
            /// ナンバー
            /// </summary>
            int Num { get; }

            /// <summary>
            /// 名前
            /// </summary>
            string Value { get; }
        }

        /// <summary>
        /// タイプの具象実装
        /// </summary>
        private readonly struct Type : IType
        {
            /// <summary>
            /// ナンバー
            /// </summary>
            public int Num { get; }

            /// <summary>
            /// 名前
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="num"></param>
            /// <param name="value"></param>
            public Type(int num, string value) => (Num, Value) = (num, value);
        }

        /// <summary>
        /// 赤の遅延評価値
        /// </summary>
        private static readonly Lazy<IType> red = new Lazy<IType>(() => new Type(1, "Red"));

        /// <summary>
        /// 青の遅延業価値
        /// </summary>
        private static readonly Lazy<IType> blue = new Lazy<IType>(() => new Type(2, "Blue"));

        /// <summary>
        /// 黄色の遅延評価値
        /// </summary>
        private static readonly Lazy<IType> yellow = new Lazy<IType>(() => new Type(3, "Yellow"));
    }
}