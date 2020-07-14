using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite4Unity3d;
using UnityEngine;


namespace ManeProject.Infrastructure.DB
{
    /// <summary>
    /// DB とのコネクション
    /// </summary>
    public static class DBConnect
    {
        /// <summary>
        /// SQL データコネクション
        /// </summary>
        public static SQLiteConnection SQLConnect => dbConnection.Value.Instance;

        public static Task Initialize()
        {
            return Task.WhenAll(dbConnection.Value.Init());
        }

        /// <summary>
        /// DBConnection インタフェース
        /// </summary>
        public interface IDBConnection
        {
            /// <summary>
            /// SQLiteConnection インスタンス
            /// </summary>
            SQLiteConnection Instance { get; }

            /// <summary>
            /// 初期化
            /// </summary>
            /// <returns></returns>
            Task Init();
        }

        /// <summary>
        /// DBConnection 具象
        /// </summary>
        private sealed class DBConnection : IDBConnection
        {
            /// <summary>
            /// SQLiteConnection インスタンス
            /// </summary>
            public SQLiteConnection Instance { get; private set; }

            /// <summary>
            /// データベースの保存領域
            /// データベース名まで含めて保存
            /// </summary>
            public readonly string m_dataBasePath;

            /// <summary>
            /// 初期化
            /// </summary>
            /// <returns></returns>
            public async Task Init()
            {
                if (Instance != null) Instance.Dispose();
                Instance = null;

                Instance = new SQLiteConnection(m_dataBasePath);
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// データベースの保存位置は StreamingAssets の方に固定 ( 臨時的に設定 )
            public DBConnection(string dbname) => m_dataBasePath = Path.Combine(Application.streamingAssetsPath,$"{dbname}.db");
        }

        /// <summary>
        /// データベースコネクションの遅延評価値
        /// </summary>
        private static readonly Lazy<IDBConnection> dbConnection = new Lazy<IDBConnection>(() => new DBConnection("db"));
    }
}