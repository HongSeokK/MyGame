using SQLite4Unity3d;
using System.Collections.Generic;
using System;

namespace ManeProject.Infrastructure.DB
{
    [Table("array")]
    public sealed class Array
    {
        [PrimaryKey, Column("row")]
        public int Row { get; set; } = 0;

        [PrimaryKey, Column("column")]
        public int Column { get; set; } = 0;

        [Column("position_x")]
        public int PositionX { get; set; } = 0;

        [Column("position_y")]
        public int PositionY { get; set; } = 0;
    }
}