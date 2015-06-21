using Sharprdf.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Writing;

namespace Sharprdf.Core
{
    public class Engine
    {
        private KnowledgeBase _knowledgeBase;
        private SyntaxTreeBuilder _syntaxTreeBuilder;

        public Engine()
            : this(new Ontology(name: "C# Ontology", prefix: "cscro", fileName: "cscro.v2.owl")) { }

        public Engine(Ontology ontology)
        {
            _knowledgeBase = new KnowledgeBase("Engine KB", ontology);
            _syntaxTreeBuilder = new SyntaxTreeBuilder();
        }

        public byte[] CreateRdf(string fileName, string sourceCode)
        {
            var syntaxTree = _syntaxTreeBuilder.BuildSyntaxTree(sourceCode);
            var sourceCodeRdf = _knowledgeBase.AddDataToGraph(fileName, fileName, syntaxTree);

            var rdfxmlwriter = new RdfXmlWriter();
            var data = VDS.RDF.Writing.StringWriter.Write(sourceCodeRdf, rdfxmlwriter);
            var byteArray = Encoding.Unicode.GetBytes(data);
            return byteArray;
        }

        public IGraph CreateRdfGraph(string fileName, string sourceCode)
        {
            var syntaxTree = _syntaxTreeBuilder.BuildSyntaxTree(sourceCode);
            var sourceCodeRdf = _knowledgeBase.AddDataToGraph(fileName, fileName, syntaxTree);
            return sourceCodeRdf;
        }
    }
}
