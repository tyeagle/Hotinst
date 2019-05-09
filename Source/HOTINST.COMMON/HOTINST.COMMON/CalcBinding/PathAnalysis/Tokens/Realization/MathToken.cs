using System;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract.Help;

namespace HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Realization
{
    public class MathToken : PathToken
    {
        public string MathMember { get; private set; }

        private PathTokenId id;
        public override PathTokenId Id { get { return id; } }

        public MathToken(int start, int end, string mathMember)
            : base(start, end)
        {
            MathMember = mathMember;
            id = new PathTokenId(PathTokenType.Math, String.Join(".", "Math", MathMember));
        }
    }
}
