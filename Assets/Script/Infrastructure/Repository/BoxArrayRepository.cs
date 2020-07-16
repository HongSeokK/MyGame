using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManeProject.Domain.Box;
using ManeProject.Infrastructure.DB;

namespace ManeProject.Infrastructure.Repository
{
    public static class BoxRepository
    {
        public static IRepository Instance => instance.Value;

        private static readonly Lazy<IRepository> instance = new Lazy<IRepository>(() => new BoxRepositoryImpl());

        private sealed class BoxRepositoryImpl : IRepository
        {
            public Task<List<IBoxArray>> CreateRandomType(int count)
            {
                var dbArray = DBConnect.SQLConnect.Table<DBArray>().ToArray();

                var dbArrayCount = dbArray.Count();

                var random = new Random();

                var randomTypeArray = new List<BoxType.IType>();
                var testArray = new List<IBoxArray>();

                var redCount = 0;
                var blueCount = 0;
                var yellowCount = 0;

                var createdCount = 0;

                var limitColorCount = dbArrayCount / 3 + 1;

                while(createdCount < dbArrayCount)
                {
                    var type = BoxType.CreateBy((BoxType.BoxColorNum)random.Next(1, BoxType.BoxTypeCount + 1));

                    switch(type.Num)
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
                    testArray.Add(BoxArrayFactory.Create(position, type));

                    randomTypeArray.Add(type);
                    createdCount++;
                }

                var test = random.Next(1, 4);

                return Task.FromResult(testArray);
            }

            public IBoxArray CreateBoxArray()
            {
                var dbArray = DBConnect.SQLConnect.Table<DBArray>().AsEnumerable();

                var dbArrayCount = dbArray.Count();

                var color = CreateRandomType(dbArrayCount);

                return default;
            }
        }
    }
}