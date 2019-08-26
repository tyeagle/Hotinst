using System.Collections.Generic;
using HOTINST.COMMON.DynamicExpresso;

namespace HOTINST.COMMON.CalcBinding
{
    public interface IExpressionParser
    {
        Lambda Parse(string expressionText, params Parameter[] parameters);

        void SetReference(IEnumerable<ReferenceType> referencedTypes);
    }
}
