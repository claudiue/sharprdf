using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharprdf.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;

namespace Sharprdf.Core
{
    public class Ontology
    {
        public string Name { get; private set; }
        public string Prefix { get; private set; }
        public IGraph Graph { get { return _ontologyGraph; } }

        public Ontology(string name, string prefix, string fileName)
        {
            Name = name;
            Prefix = prefix;
            _ontologyGraph = new Graph();
            _ontologyGraph.LoadFromFile(fileName);
        }

        public IGraph CopyNamespacesToGraph(IGraph graph)
        {
            foreach (var prefix in Graph.NamespaceMap.Prefixes)
            {
                if (string.IsNullOrEmpty(prefix))
                    continue;
                graph.NamespaceMap.AddNamespace(prefix, Graph.NamespaceMap.GetNamespaceUri(prefix));
            }
            return graph;
        }
        
        private IGraph _ontologyGraph;
    }
}
