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
            public BoxArray CreateBoxArray()
            {
                var test = new int[]{0,1,2,3,4,5,6,7};
                var test2 = new int[]{0,1,2,3,4,5,6,7};



                return default;
            }
        }
    }
}