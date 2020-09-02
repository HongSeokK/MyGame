using System;
using System.Collections.Generic;
using System.Linq;
using ManeProject.Domain.Box;
using DBArray = ManeProject.Infrastructure.DB.DBArray;
using Random = System.Random;
using System.Threading.Tasks;
using UnityEngine;

namespace ManeProject.Infrastructure.Repository.Cache
{
    public static class BlockCache
    {
        public static ICache Instance => instance.Value;

        private sealed class BlockCacheImpl : MonoBehaviour, ICache
        {
            private static IBoxArray[,] BoxArrays { get; set; }

            private static List<DBArray> DBArray { get; set; }

            public bool IsStored { get; private set; }

            private int MaxRow { get; set; }

            private int MaxColumn { get; set; }

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
            private IBoxArray[,] InitTile(int row, int column, List<DBArray> dbArray)
            {
                var random = new Random();

                var returnArray = new IBoxArray[row, column];

                var redCount = 0;
                var blueCount = 0;
                var yellowCount = 0;

                var createdCount = 0;

                var limitColorCount = (row * column) / 3 + 1;

                DBArray = dbArray;

                MaxRow = row;
                MaxColumn = column;

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
                        var position = new Vector3(DBArray[createdCount].PositionX, DBArray[createdCount].PositionY, 0);
                        var boxName = new BoxName(r.ToString() + c.ToString());
                        returnArray[r, c] = BoxArrayFactory.Create(position, type, 0, false, false, null, boxName);
                        c++;
                        createdCount++;
                    }
                    r++;
                }

                GroupListArray = MatchBoxes(row, column, returnArray);

                for (int i = 0; i < GroupListArray.GetLength(0); i++)
                {
                    if (GroupListArray[i] != null)
                    {
                        foreach (var temp in GroupListArray[i])
                        {
                            var boxName = new BoxName(temp.X.ToString() + temp.Y.ToString());
                            returnArray[temp.X, temp.Y] = returnArray[temp.X, temp.Y].SetGroupNum(i);
                        }
                    }
                }

                return returnArray;
            }

            public void DeleteBoxes(int ListNum)
            {
                if(IsStored)
                {
                    foreach(var box in GroupListArray[ListNum])
                    {
                        BoxArrays[box.X, box.Y] = null;
                    }
                    GroupListArray[ListNum] = null;
                }
            }

            public List<IBoxArray> GetBoxArrayFromList(int row, int column)
            {
                var returnList = new List<IBoxArray>();

                var taget = BoxArrays[row, column];

                if (IsStored)
                {
                    foreach (var box in GroupListArray[taget.GroupListNum])
                    {
                        returnList.Add(BoxArrays[box.X, box.Y]);
                    }
                    return returnList;
                }

                throw new NullReferenceException();
            }

            public DeleteResult TryDelete(int row, int column)
            {
                var target = BoxArrays[row, column];

                if (IsStored)
                {
                    var targetList = GroupListArray[target.GroupListNum];

                    var targetListCount = targetList.Count();

                    var isDeleteable = targetListCount >= 3;
                    if (isDeleteable)
                    {
                        foreach (var box in targetList)
                        {
                            Destroy(BoxArrays[box.X, box.Y].GameObj);
                            BoxArrays[box.X, box.Y] = BoxArrays[box.X, box.Y].UnSetGameObj();
                        }

                        RefreshAfterTileClean();
                    }
                    return new DeleteResult { BoxList = BoxArrays };
                }

                throw new NullReferenceException();
            }

            /// <summary>
            /// 
            /// </summary>
            private void RefreshAfterTileClean()
            {
                var random = new Random();

                for(int r = 0; r<MaxRow;r++)
                {
                    for(int c = 0; c<MaxColumn; c++)
                    {
                        if(BoxArrays[r,c].GameObj == null)
                        {
                            for(int nC = c+1; nC < MaxColumn; nC++)
                            {
                                if (BoxArrays[r, nC].GameObj != null)
                                {
                                    var temp = BoxArrays[r, nC];
                                    BoxArrays[r, c] = BoxArrays[r, c].SetGameObj(temp.GameObj);
                                    BoxArrays[r, c] = BoxArrays[r, c].SetType(temp.BoxType);
                                    BoxArrays[r, c].GameObj.name = r.ToString() + c.ToString();

                                    BoxArrays[r, nC] = BoxArrays[r, nC].UnSetGameObj();
                                    BoxArrays[r, nC] = BoxArrays[r, nC].UnSetType();
                                    break;
                                }
                            }
                        }
                    }
                }


                for (int c = 0; c < MaxColumn; c++)
                {
                    for (int r = 0; r < MaxRow; r++)
                    {
                        if (BoxArrays[r, c].GameObj == null)
                        {
                            var type = BoxType.CreateBy((BoxType.BoxColorNum)random.Next(1, BoxType.BoxTypeCount + 1));

                            BoxArrays[r, c] = BoxArrays[r, c].SetTypeWithRegenerate(type);
                        }
                    }
                }

                GroupListArray = MatchBoxes(MaxRow, MaxColumn, BoxArrays);

                for (int i = 0; i < GroupListArray.GetLength(0); i++)
                {
                    if (GroupListArray[i] != null)
                    {
                        foreach (var temp in GroupListArray[i])
                        {
                            var boxName = new BoxName(temp.X.ToString() + temp.Y.ToString());
                            BoxArrays[temp.X, temp.Y] = BoxArrays[temp.X, temp.Y].SetGroupNum(i);
                        }
                    }
                }
            }


            /// <summary>
            /// キャッシュメモリ削除
            /// </summary>
            public void Dispose()
            {
                BoxArrays.Initialize();
                IsStored = false;
            }

            public IBoxArray[,] InitBoxArray(List<DBArray> DBInfo)
            {
                IsStored = false;
                var rowColumnCount = (int)Math.Sqrt(DBInfo.Count());

                BoxArrays = InitTile(rowColumnCount, rowColumnCount, DBInfo);
                IsStored = true;

                return BoxArrays;
            }

            public IBoxArray[,] GetBlockArray() => IsStored ? BoxArrays : throw new NullReferenceException();

            private List<GroupPosition>[] MatchBoxes(int maxrow, int maxcolumn, IBoxArray[,] boxes)
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
                            MatchDfs(row, column, maxrow, maxcolumn, boxes[row, column].BoxType, boxes, tList[tempCount], visited);
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
            private static void MatchDfs(
                int row,
                int column,
                int maxRow,
                int maxColumn,
                BoxType.IType type,
                IBoxArray[,] boxes,
                List<GroupPosition> temp,
                bool[,] visited)
            {
                visited[row, column] = true;
                temp.Add(new GroupPosition(row, column));
                for (int r = 0; r < 4; r++)
                {
                    var dr = row + dir[r, 0];
                    var dc = column + dir[r, 1];
                    if (canCheck(dr, dc, maxRow, maxColumn) && boxes[dr, dc].BoxType == type && visited[dr, dc] != true)
                    {
                        MatchDfs(dr, dc, maxRow, maxColumn, type, boxes, temp, visited);
                    }
                }

            }

            private static bool canCheck(int dr, int dc, int maxRow, int maxColumn)
                => (dr >= 0 && dr < maxRow && dc >= 0 && dc < maxColumn);
        }

        private static readonly Lazy<BlockCacheImpl> instance = new Lazy<BlockCacheImpl>(() => new BlockCacheImpl());
    }
}