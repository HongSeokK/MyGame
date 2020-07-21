using System;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Domain.Box;
using DBArray = ManeProject.Infrastructure.DB.DBArray;

namespace ManeProject.Infrastructure.Repository.Cache
{
    public static class BlockCache
    {
        public static ICache Instance => instance.Value;

        private sealed class BlockCacheImpl : ICache
        {
            private IBoxArray[,] BoxArrays { get; set; }

            public bool IsStored { get; private set; }

            /// <summary>
            /// 配列を探索するための周縁位置配列
            /// </summary>
            private static readonly int[,] dir = new int[,] { { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } };

            /// <summary>
            /// 配列中でのグループの情報
            /// </summary>
            private readonly struct GroupPosition
            {
                /// <summary>
                /// 配列での X 置
                /// </summary>
                public int X { get; }

                /// <summary>
                /// 配列での Y 値
                /// </summary>
                public int Y { get; }

                /// <summary>
                /// コンストラクター
                /// </summary>
                /// <param name="x"></param>
                /// <param name="y"></param>
                public GroupPosition(int x, int y) => (X, Y) = (x, y);
            }

            /// <summary>
            /// グループリストの配列
            /// グループになったブロックを包含してる List の配列
            /// </summary>
            private static List<GroupPosition>[] GroupListArray;

            /// <summary>
            /// タイルを初期化する
            /// </summary>
            /// <param name="row">最大行</param>
            /// <param name="column">最大列</param>
            /// <param name="dbArray">DB から取ってくる情報</param>
            /// <returns></returns>
            private IBoxArray[,] InitTile(int row, int column, DBArray[] dbArray)
            {

                var random = new Random();

                var testArray = new IBoxArray[row, column];
                var testPositions = new Position[row, column];
                var testTypes = new BoxType.IType[row, column];

                var redCount = 0;
                var blueCount = 0;
                var yellowCount = 0;

                var createdCount = 0;

                var limitColorCount = (row * column) / 3 + 1;


                for (int r = 0; r < row;)
                {
                    for (int c = 0; c < column;)
                    {
                        var type = BoxType.CreateBy((BoxType.BoxColorNum)random.Next(1, BoxType.BoxTypeCount + 1));

                        switch (type.Num)
                        {
                            case BoxType.BoxColorNum.Red:
                                if (redCount > limitColorCount) continue;
                                redCount++;
                                break;
                            case BoxType.BoxColorNum.Blue:
                                if (blueCount > limitColorCount) continue;
                                blueCount++;
                                break;
                            case BoxType.BoxColorNum.Yellow:
                                if (yellowCount > limitColorCount) continue;
                                yellowCount++;
                                break;
                        }
                        var position = new Position(dbArray[createdCount].PositionX, dbArray[createdCount].PositionY, 0);
                        testPositions[r, c] = new Position(dbArray[createdCount].PositionX, dbArray[createdCount].PositionY, 0);
                        testTypes[r, c] = type;
                        c++;
                        createdCount++;
                    }
                    r++;
                }

                GroupListArray = MatchBoxes(row, column, testTypes);

                for (int i = 0; i < GroupListArray.GetLength(0); i++)
                {
                    if (GroupListArray[i] != null)
                    {
                        foreach (var temp in GroupListArray[i])
                        {
                            testArray[temp.X, temp.Y] = BoxArrayFactory.Create(testPositions[temp.X, temp.Y], testTypes[temp.X, temp.Y], i);
                            UnityEngine.Debug.Log(i + "번째 " + testArray[temp.X, temp.Y].BoxType.Value + " 다 ");
                        }
                    }
                }

                return testArray;
            }

            /// <summary>
            /// キャッシュメモリ削除
            /// </summary>
            public void Dispose()
            {
                BoxArrays.Initialize();
                IsStored = false;
            }

            public IBoxArray[,] InitBoxArray(DBArray[] DBInfo)
            {
                IsStored = false;
                var rowColumnCount = (int)Math.Sqrt(DBInfo.Count());

                BoxArrays = InitTile(rowColumnCount, rowColumnCount, DBInfo);
                IsStored = true;

                return BoxArrays;
            }

            public IBoxArray[,] GetBlockArray() => IsStored ? BoxArrays : throw new NullReferenceException();


            private List<GroupPosition>[] MatchBoxes(int maxrow, int maxcolumn, BoxType.IType[,] boxTypes)
            {
                // Bool は宣言時に false になってるため、初期化は不要
                var visited = new bool[maxrow, maxcolumn];
                // 最大は row * column となるため、 row * column 配列の List を生成 ( マッチされてるタイルをセーブするため ) 
                var tList = new List<GroupPosition>[maxrow * maxcolumn];

                var tempCount = 0;

                for (int row = 0; row < maxrow; row++)
                {
                    for (int column = 0; column < maxcolumn; column++)
                    {
                        if (visited[row, column] != true)
                        {
                            tList[tempCount] = new List<GroupPosition>();
                            Dfs(row, column, maxrow, maxcolumn, boxTypes[row, column], boxTypes, tList[tempCount], visited);
                            tempCount++;
                        }
                    }
                }

                return tList;
            }

            /// <summary>
            /// DFS アルゴリズムで配列の中身を探索、グループリストに追加
            /// </summary>
            /// <param name="row">行</param>
            /// <param name="column">列</param>
            /// <param name="maxRow">最大行</param>
            /// <param name="maxColumn">最大列</param>
            /// <param name="type">選択されたタイプ</param>
            /// <param name="types">生成されている配列のタイプたち</param>
            /// <param name="temp"></param>
            /// <param name="visited">訪問情報の配列</param>
            private static void Dfs(
                int row,
                int column,
                int maxRow,
                int maxColumn,
                BoxType.IType type,
                BoxType.IType[,] types,
                List<GroupPosition> temp,
                bool[,] visited)
            {
                visited[row, column] = true;
                temp.Add(new GroupPosition(row, column));
                for (int r = 0; r < 4; r++)
                {
                    var dr = row + dir[r, 0];
                    var dc = column + dir[r, 1];
                    if (canCheck(dr, dc, maxRow, maxColumn) && types[dr, dc] == type && visited[dr, dc] != true)
                    {
                        Dfs(dr, dc, maxRow, maxColumn, type, types, temp, visited);
                    }
                }

            }

            private static bool canCheck(int dr, int dc, int maxRow, int maxColumn)
                => (dr >= 0 && dr < maxRow && dc >= 0 && dc < maxColumn);
        }

        private static readonly Lazy<BlockCacheImpl> instance = new Lazy<BlockCacheImpl>(() => new BlockCacheImpl());
    }
}