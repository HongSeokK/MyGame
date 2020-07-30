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

    /// <summary>
    /// Box の配列中の位置
    /// </summary>
    public readonly struct ArrayPosition
    {
        /// <summary>
        /// 行
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// 列
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public ArrayPosition(int row, int column) => (Row, Column) = (row, column);
    }

    /// <summary>
    /// ボックスネーム
    /// </summary>
    public readonly struct BoxName
    {
        /// <summary>
        /// ネーム
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name"></param>
        public BoxName(string name) => Value = name;
    }
}
