using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManeProject.Domain.Box
{
    /// <summary>
    /// 生成位置
    /// </summary>
    public readonly struct Position
    {
        /// <summary>
        /// X値
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Y値
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Z値
        /// </summary>
        public int Z { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Position(int x, int y, int z) => (X, Y, Z) = (x, y, z);
    }
}
