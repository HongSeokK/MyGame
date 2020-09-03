using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Domain.Box;
using ManeProject.Infrastructure.DB;
using ManeProject.Infrastructure.Repository.Cache;

namespace ManeProject.Infrastructure.Repository
{
    public static class BoxRepository
    {
        public static IRepository Instance => instance.Value;

        private static readonly Lazy<IRepository> instance = new Lazy<IRepository>(() => new BoxRepositoryImpl(BoxArrayCache.Instance));

        /// <summary>
        /// ボックスレポジトリ
        /// </summary>
        private sealed class BoxRepositoryImpl : IRepository
        {

            private readonly ICache m_cache;

            public BoxRepositoryImpl(ICache cache) => m_cache = cache;

            /// <summary>
            /// ボックス配列生成
            /// </summary>
            /// <returns></returns>
            public Task<IBoxArray[,]> CreateBoxArray()
            {
                // DB からボックス配列の配置情報を取る
                var dbArray = DBManager.SQLConnect.Table<DBArray>().ToList();

                // キャッシュで初期化された物を返す
                return Task.FromResult(m_cache.InitBoxArray(dbArray));
            }

            /// <summary>
            /// 削除実行
            /// </summary>
            /// <param name="row">行指定</param>
            /// <param name="column">列指定</param>
            /// <returns></returns>
            public DeleteResult TryDelete(int row,int column) => m_cache.TryDelete(row, column);
        }
    }
}