using System;
using System.Collections.Generic;
using HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Abstract.Help;

namespace HOTINST.COMMON.CalcBinding.PathAnalysis.Tokens.Realization
{
    public class StaticPropertyPathToken : PropertyPathToken
    {
        public string Class { get; private set; }
        public string Namespace { get; private set; }

        private PathTokenId id;
        public override PathTokenId Id { get { return id; } }

        public StaticPropertyPathToken(int start, int end, string @namespace, string @class, IEnumerable<string> properties)
            : base(start, end, properties)
        {
            Class = @class;
            Namespace = @namespace;
            id = new PathTokenId(PathTokenType.StaticProperty, String.Format("{0}:{1}.{2}", Namespace, Class, String.Join(".", Properties)));
        }
    }
}
