using UnityEngine;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// Box 配列要素
    /// </summary>
    public interface IBoxArray
    {
        /// <summary>
        /// Box の位置
        /// </summary>
        Position BoxPosition { get; }

        /// <summary>
        /// Box のタイプ
        /// </summary>
        BoxType.IType BoxType { get; }
    }

    /// <summary>
    /// Box 配列要素ファクトリー
    /// </summary>
    public static class BoxArrayFactory
    {
        /// <summary>
        /// Box 配列の要素を生成する
        /// </summary>
        /// <param name="boxPosition"></param>
        /// <param name="boxType"></param>
        /// <returns></returns>
        public static IBoxArray Create(
                Position boxPosition,
                BoxType.IType boxType
            ) => new BoxArrayImpl(
                boxPosition,
                boxType
            );

        private sealed class BoxArrayImpl : IBoxArray
        {
            /// <summary>
            /// Box の位置
            /// </summary>
            public Position BoxPosition { get; }

            /// <summary>
            /// Box のタイプ
            /// </summary>
            public BoxType.IType BoxType { get; }

            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="boxPosition"></param>
            /// <param name="boxType"></param>
            public BoxArrayImpl(
                    Position boxPosition,
                    BoxType.IType boxType
                ) => (
                    BoxPosition,
                    BoxType
                ) = (
                    boxPosition,
                    boxType
                );
        }
    }
}