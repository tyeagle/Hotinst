using System;
using System.Collections.Generic;
using System.Linq;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract.Help;

namespace HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Realization
{
    public class PropertyPathToken : PathToken
    {
        public IEnumerable<string> Properties { get; private set; }

        private PathTokenId id;
        public override PathTokenId Id { get { return id; } }

        public PropertyPathToken(int start, int end, IEnumerable<string> properties)
            : base(start, end)
        {
            Properties = properties.ToList();
            id = new PathTokenId(PathTokenType.Property, String.Join(".", Properties));
        }
    }
}
