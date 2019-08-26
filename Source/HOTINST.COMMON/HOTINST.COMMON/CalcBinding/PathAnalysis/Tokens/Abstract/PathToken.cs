﻿using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract.Help;

namespace HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract
{
    public abstract class PathToken
    {
        public int Start { get; private set; }

        public int End { get; private set; }

        public abstract PathTokenId Id { get; }

        protected PathToken(int start, int end)
        {
            Start = start;
            End = end;
        }
    }
}
