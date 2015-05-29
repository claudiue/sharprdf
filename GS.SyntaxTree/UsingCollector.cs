using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.SyntaxTree
{
    class UsingCollector : CSharpSyntaxWalker
    {
        public readonly IList<UsingDirectiveSyntax> Usings = new List<UsingDirectiveSyntax>();

        public override void VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (node.Name.ToString() != "System" && !node.Name.ToString().StartsWith("System."))
                Usings.Add(node);
        }
    }
}
