using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharprdf.Core.Interfaces
{
    public interface ISyntaxTreeBuilder
    {
        SyntaxTree BuildSyntaxTree(string sourceCode);
    }
}
