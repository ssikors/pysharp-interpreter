﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PySharpCompiler.Classes
{
    public class Position
    {
        public int Line;
        public int Column;

        public Position(int line, int column)
        {
            Line = line;
            Column = column;
        }

        public override string ToString()
        {
            return $"[{Line}:{Column}]";
        }
    }
}
