using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite4Unity3d;
using UniRx.Async;
using UnityEngine;

namespace ManeProject.Infrastructure.DB
{
    /// <summary>
    /// DB とのコネクション
    /// </summary>
    public static class DBManager
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
            /// マスタ名
            /// </summary>
            private readonly string m_masterName;

            /// <summary>
            /// read write 可能保存領域
            /// </summary>
            private readonly string m_persistentDatabasePath;

            /// <summary>
            /// readonly 保存領域
            /// </summary>
            private readonly string m_streamingAssetsDatabasePath;

            /// <summary>
            /// 初期化
            /// </summary>
            /// <returns></returns>
            public async Task Init()
            {
                if (Instance != null) Instance.Dispose();
                Instance = null;

                // 以下の条件に当てはまったら streamingAssets から persistentDataPath にデータベースファイルをコピーする
                // 1. persistentDataPath にデータベースファイルが存在しない
                // 2. streamingAssetsDatabasePath のデータベースファイルの方が persistentDatabasePath より新しい
                if (!File.Exists(m_persistentDatabasePath) || (File.GetLastWriteTimeUtc(m_streamingAssetsDatabasePath) > File.GetLastWriteTimeUtc(m_persistentDatabasePath)))
                {
                    // android
                    if (m_streamingAssetsDatabasePath.Contains("://"))
                    {
                        // AndroidではUnityWebRequestで正しく読み込めなかったのでWWWを使用
                        using (var www = new WWW(m_streamingAssetsDatabasePath))
                        {
                            await www;
                            if (!string.IsNullOrEmpty(www.error))
                            {
                                Debug.LogError(www.error);
                                throw new FileNotFoundException();
                            }
                            File.WriteAllBytes(m_persistentDatabasePath, www.bytes);
                        }
                    }
                    // iOS
                    else
                    {
                        if (!File.Exists(m_streamingAssetsDatabasePath))
                        {
                            Debug.LogError($"ERROR: the file DB named {m_masterName} doesn't exist in the StreamingAssets Folder, please copy it there.");
                            throw new Exception();
                        }

                        File.Copy(m_streamingAssetsDatabasePath, m_persistentDatabasePath, true);

                    }
                }

                Instance = new SQLiteConnection(m_persistentDatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// データベースの保存位置は StreamingAssets の方に固定 ( 臨時的に設定 )
            public DBConnection(string dbname) =>
            (
                m_masterName,
                m_persistentDatabasePath,
                m_streamingAssetsDatabasePath
            ) = (
                dbname,
                Path.Combine(SavedataPath.GetSecureDataPath(), $"{dbname}.db"),
                Path.Combine(Application.streamingAssetsPath, $"{dbname}.db")
            );
        }

        /// <summary>
        /// データベースコネクションの遅延評価値
        /// </summary>
        private static readonly Lazy<IDBConnection> dbConnection = new Lazy<IDBConnection>(() => new DBConnection("db"));
    }
}