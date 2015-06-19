using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Sharprdf.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharprdf.Core
{
    public class SyntaxTreeBuilder : ISyntaxTreeBuilder
    {
        public SyntaxTree BuildSyntaxTree(string sourceCode)
        {
            return CSharpSyntaxTree.ParseText(sourceCode);
        }
    }
}
