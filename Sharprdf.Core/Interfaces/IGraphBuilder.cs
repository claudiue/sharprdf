using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace Sharprdf.Core.Interfaces
{
    public interface IGraphBuilder
    {
        IGraph CreateGraph(string graphId);
        IGraph AddDataToGraph(string graphId, string fileName, SyntaxTree syntaxTree);
    }
}
