using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS.Symbols
{
    class Program
    {
        static void Main(string[] args)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");    
        }
    }
}");
            var root = (CompilationUnitSyntax)syntaxTree.GetRoot();
            var compilation = CSharpCompilation.Create("HelloWorld")
                    .AddReferences(MetadataReference.CreateFromAssembly(typeof(object).Assembly))
                    .AddSyntaxTrees(syntaxTree);
            
            var model = compilation.GetSemanticModel(syntaxTree);
            var nameInfo = model.GetSymbolInfo(root.Usings[0].Name);
            
            var systemSymbol = (INamespaceSymbol)nameInfo.Symbol;
            foreach (var ns in systemSymbol.GetNamespaceMembers())
            {
                Console.WriteLine(ns.Name);
            }

            var helloWorldString = root.DescendantNodes()
                .OfType<LiteralExpressionSyntax>()
                .FirstOrDefault();

            var literalInfo = model.GetTypeInfo(helloWorldString);
            var stringTypeSymbol = (INamedTypeSymbol)literalInfo.Type;
            Console.Clear();
            foreach(var name in (from method in stringTypeSymbol.GetMembers()
                                     .OfType<IMethodSymbol>()
                                 where method.ReturnType.Equals(stringTypeSymbol)
                                     && method.DeclaredAccessibility == Accessibility.Public
                                 select method.Name).Distinct())
            {
                Console.WriteLine(name);
            }
        }
    }
}
