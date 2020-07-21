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

        private static readonly Lazy<IRepository> instance = new Lazy<IRepository>(() => new BoxRepositoryImpl(BlockCache.Instance));

        private sealed class BoxRepositoryImpl : IRepository
        {
            private readonly ICache m_cache;

            public BoxRepositoryImpl(ICache cache) => m_cache = cache;

            public Task<IBoxArray[,]> CreateBoxArray()
            {
                var dbArray = DBConnect.SQLConnect.Table<DBArray>().ToArray();

                return Task.FromResult(m_cache.InitBoxArray(dbArray));
            }
        }
    }
}