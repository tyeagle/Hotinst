﻿using System.Collections.Generic;
using HOTINST.COMMON.DynamicExpresso;

namespace HOTINST.COMMON.CalcBinding
{
    public class InterpreterParser : IExpressionParser
    {
        Interpreter interpreter;

        public InterpreterParser()
        {
            interpreter = new Interpreter();
        }

        public Lambda Parse(string expressionText, Parameter[] parameters)
        {
            return interpreter.Parse(expressionText, parameters);
        }

        public void SetReference(IEnumerable<ReferenceType> referencedTypes)
        {
            interpreter.Reference(referencedTypes);
        }
    }
}