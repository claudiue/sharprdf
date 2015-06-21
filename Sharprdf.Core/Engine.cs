using Sharprdf.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF.Writing;

namespace Sharprdf.Core
{
    public class Engine
    {
        private KnowledgeBase _knowledgeBase;
        private Ontology _ontology;
        private SyntaxTreeBuilder _syntaxTreeBuilder;

        public Engine()
        {
            _ontology = new Ontology(name: "C# Ontology", prefix: "cscro", fileName: "cscro.v2.owl");
            _knowledgeBase = new KnowledgeBase("Engine KB", _ontology);
            _syntaxTreeBuilder = new SyntaxTreeBuilder();
        }

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

            String data = VDS.RDF.Writing.StringWriter.Write(sourceCodeRdf, rdfxmlwriter);


            //var memoryStream = new System.IO.MemoryStream()
            //var streamWriter = new System.IO.StreamWriter(memoryStream);// TextWriter();
            //rdfxmlwriter.Save(sourceCodeRdf, streamWriter);
            //streamWriter.Flush();
            var byteArray = Encoding.Unicode.GetBytes(data);//memoryStream.ToArray();
            return byteArray;
            
            //return memoryStream;
        }
    }
}
