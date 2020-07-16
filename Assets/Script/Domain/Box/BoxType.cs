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
        public static IType Blue => blue.Value;

        /// <summary>
        /// 赤
        /// </summary>
        public static IType Red => red.Value;

        /// <summary>
        /// 黄色
        /// </summary>
        public static IType Yellow => yellow.Value;

        /// <summary>
        /// タイプ数
        /// 3 で固定
        /// </summary>
        public const int BoxTypeCount = 3;

        /// <summary>
        /// 番号で Box の色を判別できるように enum で宣言
        /// </summary>
        public enum BoxColorNum
        {
            Red = 1,
            Blue = 2,
            Yellow = 3
        }

        /// <summary>
        /// タイプインタフェース
        /// </summary>
        public interface IType
        {
            /// <summary>
            /// ナンバー
            /// </summary>
            BoxColorNum Num { get; }

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
            public BoxColorNum Num { get; }

            /// <summary>
            /// 名前
            /// </summary>
            public string Value { get; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="num"></param>
            /// <param name="value"></param>
            public Type(BoxColorNum num, string value) => (Num, Value) = (num, value);
        }

        /// <summary>
        /// 数値からタイプの作成
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns></returns>
        public static IType CreateBy(BoxColorNum value)
        {
            switch (value)
            {
                case BoxColorNum _ when red.Value.Num == value:
                    return Red;
                case BoxColorNum _ when blue.Value.Num == value:
                    return Blue;
                case BoxColorNum _ when yellow.Value.Num == value:
                    return Yellow;
            }

            throw new Exception();
        }

        /// <summary>
        /// 赤の遅延評価値
        /// </summary>
        private static readonly Lazy<IType> red = new Lazy<IType>(() => new Type(BoxColorNum.Red, "Red"));

        /// <summary>
        /// 青の遅延業価値
        /// </summary>
        private static readonly Lazy<IType> blue = new Lazy<IType>(() => new Type(BoxColorNum.Blue, "Blue"));

        /// <summary>
        /// 黄色の遅延評価値
        /// </summary>
        private static readonly Lazy<IType> yellow = new Lazy<IType>(() => new Type(BoxColorNum.Yellow, "Yellow"));
    }
}