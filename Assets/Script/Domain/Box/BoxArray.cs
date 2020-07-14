using UnityEngine;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// Box 配列
    /// </summary>
    public interface BoxArray
    {
        /// <summary>
        /// Box の位置
        /// </summary>
        Position BoxPosition { get; }

        /// <summary>
        /// Box のタイプ
        /// </summary>
        BoxType BoxType { get; }
    }
}