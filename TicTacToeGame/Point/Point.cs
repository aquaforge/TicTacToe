﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    [Serializable]
    public class Point : ICloneable, IEquatable<Point>, IComparable<Point>
    {
        public int Row, Col;

        public Point(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public override string ToString() => $"({Row:00},{Col:00})";
        public Point Copy() => new Point(Row, Col);

        object ICloneable.Clone() => MemberwiseClone();

        public bool Equals(Point? p)
        {
            if (p == null) return false;
            return (Row == p.Row) && (Col == p.Col);
        }

        public override bool Equals(Object? other)
        {
            if (other is not Point p) return false;
            return Equals(p);
        }

        static public Point Zero => new(0, 0);
        static public Point MaxPoint => new(byte.MaxValue, byte.MaxValue);
        public override int GetHashCode() => (Row << 2) ^ Col;

        public int CompareTo(Point? other)
        {
            if (other == null) return -1;
            return 10000 * (Row - other.Row) + (Col - other.Col);
        }
    }


}
