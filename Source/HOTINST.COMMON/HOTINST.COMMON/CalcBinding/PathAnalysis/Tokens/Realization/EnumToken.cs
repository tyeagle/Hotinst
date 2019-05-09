﻿using System;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract.Help;

namespace HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Realization
{
    public class EnumToken : PathToken
    {
        public Type Enum { get; private set; }
        public string EnumMember { get; private set; }
        public string Namespace { get; private set; }

        private PathTokenId id;
        public override PathTokenId Id { get { return id; } }

        public EnumToken(int start, int end, string @namespace, Type @enum, string enumMember)
            : base(start, end)
        {
            Enum = @enum;
            EnumMember = enumMember;
            Namespace = @namespace;

            id = new PathTokenId(PathTokenType.Enum, String.Format("{0}:{1}.{2}", Namespace, @enum.Name, EnumMember));
        }
    }
}
