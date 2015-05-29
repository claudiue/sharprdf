using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GS.SyntaxTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var tree = CSharpSyntaxTree.ParseText(
@"using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello, World!"");    
        }

        int Main(int x)
        {
            //Nothing
        }  

        int Sum(int a, int b)
        {
            return a + b;
        }
    }
}");
            // Manually traversing the tree
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var firstMember = root.Members[0];
            var helloWorldDeclaration = (NamespaceDeclarationSyntax)firstMember;
            var programDeclaration = (ClassDeclarationSyntax)helloWorldDeclaration.Members[0];
            var mainDeclaration = (MethodDeclarationSyntax)programDeclaration.Members[0];
            var argsParameter = mainDeclaration.ParameterList.Parameters[0];

            // Using query methods
            var firstParameters = from methodDeclaration in root.DescendantNodes()
                                                                    .OfType<MethodDeclarationSyntax>()
                                  where methodDeclaration.Identifier.ValueText == "Main"
                                  select methodDeclaration.ParameterList.Parameters.First();
            var argsParameter2 = firstParameters.FirstOrDefault();

            var collector = new UsingCollector();
            collector.Visit(root);

            foreach (var directive in collector.Usings)
                Console.WriteLine(directive.Name);
        }
    }
}
