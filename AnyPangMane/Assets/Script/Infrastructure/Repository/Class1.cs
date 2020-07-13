using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Domain.Box;

namespace ManeProject.Infrastructure.Repository
{
    public static class BoxRepository
    {
        private sealed class BoxRepositoryImpl : IRepository
        {
            public Box CreateBox()
            {
                return default;
            }
        }
    }
}