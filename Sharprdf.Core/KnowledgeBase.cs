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
    public class KnowledgeBase : IGraphBuilder
    {
        public const string GlobalUri = "http://www.sharprdf.net/";

        public string Name { get; private set; }

        public KnowledgeBase(string name, Ontology ontology)
        {
            Name = name;
            _ontology = ontology;
            _tripleStore = new TripleStore();
        }

        public KnowledgeBase(Ontology ontology)
            : this("KB", ontology)
        {
        }

        public IGraph CreateGraph(string graphId)
        {
            graphId = graphId.Replace(" ", "_");
            var baseUri = UriFactory.Create(string.Format("{0}{1}#", GlobalUri, graphId));
            if (_tripleStore.HasGraph(baseUri))
                return _tripleStore.Graphs[baseUri];

            IGraph graph = new Graph();
            graph.BaseUri = baseUri;
            graph.NamespaceMap.AddNamespace("", graph.BaseUri);
            graph.NamespaceMap.AddNamespace(graphId, graph.BaseUri);
            graph = _ontology.CopyNamespacesToGraph(graph);
            _tripleStore.Add(graph, true);

            return graph;
        }

        public IGraph AddDataToGraph(string graphId, string fileName, SyntaxTree syntaxTree)
        {
            var graph = CreateGraph(graphId);
            var root = syntaxTree.GetRoot();
            //WriteNode(root, 0);
            //graph = ExtractDataFromNode(graph, fileName, root);

            var rootUriNode = GetRootUriNode(graph, root, fileName);
            graph = AddChildrenInGraph(graph, root, rootUriNode);
            return graph;
        }

        private IUriNode GetRootUriNode(IGraph g, SyntaxNode root, string fileName)
        {
            var rdfType = g.CreateUriNode("rdf:type");
            var rdfsLabel = g.CreateUriNode("rdfs:label");
            var cscroCompilationUnit = g.CreateUriNode("cscro:CompilationUnit");
            var cscroHasName = g.CreateUriNode("cscro:hasName");

            IUriNode rootUriNode = null;
            switch (root.RawKind)
            {
                case (int)SyntaxKind.CompilationUnit:
                    var compilationUnit = (CompilationUnitSyntax)root;
                    rootUriNode = g.CreateUriNode(string.Format(":cu_{0}", compilationUnit.GetHashCode()));
                    g.Assert(new Triple(rootUriNode, rdfType, cscroCompilationUnit));
                    g.Assert(new Triple(rootUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("compilation unit {0}", fileName), "en")));
                    g.Assert(new Triple(rootUriNode, cscroHasName, g.CreateLiteralNode(fileName, "en")));
                    break;
                // The following will probably not be used:
                //case (int)SyntaxKind.UsingDirective:
                //    break;
                //case (int)SyntaxKind.NamespaceDeclaration:
                //    break;
                //case (int)SyntaxKind.DelegateDeclaration:
                //    break;
                //case (int)SyntaxKind.EnumDeclaration:
                //    break;
                //case (int)SyntaxKind.InterfaceDeclaration:
                //    break;
                //case (int)SyntaxKind.StructDeclaration:
                //    break;
                //case (int)SyntaxKind.ClassDeclaration:
                //    break;
                //case (int)SyntaxKind.FieldDeclaration:
                //    break;
                //case (int)SyntaxKind.PropertyDeclaration:
                //    break;
                //case (int)SyntaxKind.MethodDeclaration:
                //    break;
            }
            return rootUriNode;
        }

        private IGraph AddChildrenInGraph(IGraph g, SyntaxNode node, IUriNode uriNode)
        {
            var rdfType = g.CreateUriNode("rdf:type");
            var rdfsLabel = g.CreateUriNode("rdfs:label");

            var cscroCompilationUnit = g.CreateUriNode("cscro:CompilationUnit");
            var cscroNamespace = g.CreateUriNode("cscro:Namespace");
            var cscroDelegate = g.CreateUriNode("cscro:Delegate");
            var cscroEnum = g.CreateUriNode("cscro:Enum");
            var cscroInterface = g.CreateUriNode("cscro:Interface");
            var cscroStruct = g.CreateUriNode("cscro:Struct");
            var cscroClass = g.CreateUriNode("cscro:Class");
            var cscroField = g.CreateUriNode("cscro:Field");
            var cscroMethod = g.CreateUriNode("cscro:Method");
            var cscroProperty = g.CreateUriNode("cscro:Property");
            var cscroConstructor = g.CreateUriNode("cscro:Constructor");
            var cscroAttribute = g.CreateUriNode("cscro:Attribute");

            var cscroUses = g.CreateUriNode("cscro:uses");
            var cscroIsUsedBy = g.CreateUriNode("cscro:isUsedBy");
            var cscroHasNamespace = g.CreateUriNode("cscro:hasNamespace");
            var cscroIsNamespaceOf = g.CreateUriNode("cscro:isNamespaceOf");
            var cscroHasDataType = g.CreateUriNode("cscro:hasDataType");
            var cscroIsDataTypeOf = g.CreateUriNode("cscro:isDataTypeOf");
            var cscroHasMember = g.CreateUriNode("cscro:hasMember");
            var cscroIsMemberOf = g.CreateUriNode("cscro:isMemberOf");
            var cscroHasField = g.CreateUriNode("cscro:hasField");
            var cscroIsFieldOf = g.CreateUriNode("cscro:isFieldOf");
            var cscroHasMethod = g.CreateUriNode("cscro:hasMethod");
            var cscroIsMethodOf = g.CreateUriNode("cscro:isMethodOf");
            var cscroHasProperty = g.CreateUriNode("cscro:hasProperty");
            var cscroIsPropertyOf = g.CreateUriNode("cscro:isPropertyOf");
            var cscroHasConstructor = g.CreateUriNode("cscro:hasConstructor");
            var cscroIsConstructorOf = g.CreateUriNode("cscro:isConstructorOf");

            var cscroHasAttribute = g.CreateUriNode("cscro:hasAttribute");
            var cscroIsAttributeOf = g.CreateUriNode("cscro:isAttributeOf");

            var cscroHasName = g.CreateUriNode("cscro:hasName");
            var cscroHasIdentifier = g.CreateUriNode("cscro:hasIdentifier");
            var cscroHasType = g.CreateUriNode("cscro:hasType");
            var cscroHasReturnType = g.CreateUriNode("cscro:hasReturnType");
            var cscroHasComment = g.CreateUriNode("cscro:hasComment");


            try
            {
                var comments = node.ChildTokens().FirstOrDefault().GetAllTrivia()
                    .Where(t => t.IsKind(SyntaxKind.SingleLineCommentTrivia)
                                || t.IsKind(SyntaxKind.MultiLineCommentTrivia));

                foreach (var comment in comments)
                {
                    g.Assert(new Triple(uriNode, cscroHasComment, g.CreateLiteralNode(comment.ToString(), "en")));
                }
            }
            catch 
            {
            }

            foreach (var child in node.ChildNodes())
            {
                IUriNode childUriNode = null;
                switch (child.RawKind)
                {
                    //Probably never used
                    //case (int)SyntaxKind.CompilationUnit:
                    //    break;
                    case (int)SyntaxKind.UsingDirective:
                        var usingDir = (UsingDirectiveSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":ns_{0}", usingDir.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroNamespace));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("namespace {0}", usingDir.Name.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsUsedBy, uriNode));
                        g.Assert(new Triple(uriNode, cscroUses, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasName, g.CreateLiteralNode(usingDir.Name.ToString(), "en")));
                        break;

                    case (int)SyntaxKind.NamespaceDeclaration:
                        var namspaceDecl = (NamespaceDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":ns_{0}", namspaceDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroNamespace));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("namespace {0}", namspaceDecl.Name.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsNamespaceOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasNamespace, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasName, g.CreateLiteralNode(namspaceDecl.Name.ToString(), "en")));
                        break;

                    case (int)SyntaxKind.DelegateDeclaration:
                        var delegateDecl = (DelegateDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":delegate_{0}", delegateDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroDelegate));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("delegate {0}", delegateDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsDataTypeOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasDataType, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(delegateDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, delegateDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.EnumDeclaration:
                        var enumDecl = (EnumDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":enum_{0}", enumDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroEnum));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("enum {0}", enumDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsDataTypeOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasDataType, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(enumDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, enumDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.InterfaceDeclaration:
                        var interfaceDecl = (InterfaceDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":interface_{0}", interfaceDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroInterface));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("interface {0}", interfaceDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsDataTypeOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasDataType, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(interfaceDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, interfaceDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.StructDeclaration:
                        var structDecl = (StructDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":struct_{0}", structDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroStruct));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("struct {0}", structDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsDataTypeOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasDataType, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(structDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, structDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.ClassDeclaration:
                        var classDecl = (ClassDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":class_{0}", classDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroClass));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("class {0}", classDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroIsDataTypeOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasDataType, childUriNode));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(classDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, classDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.FieldDeclaration:
                        var fieldDecl = (FieldDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":field_{0}", fieldDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroField));
                        g.Assert(new Triple(childUriNode, cscroIsFieldOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasField, childUriNode));
                        string fType;
                        string fIdentifier;
                        GetFieldData(fieldDecl, out fType, out fIdentifier);
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("field {0} {1}", fType, fIdentifier), "en")));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(fIdentifier, "en")));
                        g.Assert(new Triple(childUriNode, cscroHasType, g.CreateLiteralNode(fType, "en")));
                        g = AssertModifiers(g, childUriNode, fieldDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.PropertyDeclaration:
                        var propDecl = (PropertyDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":property_{0}", propDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroProperty));
                        g.Assert(new Triple(childUriNode, cscroIsPropertyOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasProperty, childUriNode));
                        string pType;
                        string pIdentifier;
                        GetPropertyData(propDecl, out pType, out pIdentifier);
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("property {0} {1}", pType, pIdentifier), "en")));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(pIdentifier, "en")));
                        g.Assert(new Triple(childUriNode, cscroHasType, g.CreateLiteralNode(pType, "en")));
                        g = AssertModifiers(g, childUriNode, propDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.MethodDeclaration:
                        var methDecl = (MethodDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":method_{0}", methDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroMethod));
                        g.Assert(new Triple(childUriNode, cscroIsMethodOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasMethod, childUriNode));
                        string mReturnType;
                        string mIdentifier;
                        GetMethodData(methDecl, out mReturnType, out mIdentifier);
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("method {0} {1}", mReturnType, mIdentifier), "en")));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(mIdentifier, "en")));
                        g.Assert(new Triple(childUriNode, cscroHasReturnType, g.CreateLiteralNode(mReturnType, "en")));
                        g = AssertModifiers(g, childUriNode, methDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.ConstructorDeclaration:
                        var ctorDecl = (ConstructorDeclarationSyntax)child;
                        childUriNode = g.CreateUriNode(string.Format(":constructor_{0}", ctorDecl.GetHashCode()));
                        g.Assert(new Triple(childUriNode, rdfType, cscroConstructor));
                        g.Assert(new Triple(childUriNode, cscroIsConstructorOf, uriNode));
                        g.Assert(new Triple(uriNode, cscroHasConstructor, childUriNode));
                        g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("constructor {0}", ctorDecl.Identifier.ToString()), "en")));
                        g.Assert(new Triple(childUriNode, cscroHasIdentifier, g.CreateLiteralNode(ctorDecl.Identifier.ToString(), "en")));
                        g = AssertModifiers(g, childUriNode, ctorDecl.Modifiers);
                        break;

                    case (int)SyntaxKind.AttributeList:
                        var attrList = (AttributeListSyntax)child;
                        foreach (var attribute in attrList.Attributes)
                        {
                            childUriNode = g.CreateUriNode(string.Format(":attribute_{0}", attribute.GetHashCode()));
                            g.Assert(new Triple(childUriNode, rdfType, cscroAttribute));
                            g.Assert(new Triple(childUriNode, rdfsLabel, g.CreateLiteralNode(string.Format("attribute {0}", attribute.Name.ToString()), "en")));
                            //g.Assert(new Triple(childUriNode, cscroIsAttributeOf, uriNode));
                            g.Assert(new Triple(uriNode, cscroHasAttribute, childUriNode));
                            g.Assert(new Triple(childUriNode, cscroHasName, g.CreateLiteralNode(attribute.Name.ToString(), "en")));
                        }
                        break;
                }
                g = AddChildrenInGraph(g, child, childUriNode);
            }

            return g;
        }

        private IGraph AssertModifiers(IGraph g, IUriNode uriNode, SyntaxTokenList modifiers)
        {
            var accessModifiers = new string[] { "public", "private", "protected", "internal" };
            var cscroHasModifier = g.CreateUriNode("cscro:hasModifier");
            var cscroHasAccessModifier = g.CreateUriNode("cscro:hasAccessModifier");

            foreach (var mod in modifiers)
            {
                var modIndividual = mod.ToString();
                var cscroModifier = g.CreateUriNode(string.Format("cscro:{0}", modIndividual));
                if(accessModifiers.Contains(modIndividual))
                    g.Assert(new Triple(uriNode, cscroHasAccessModifier, cscroModifier));
                else 
                    g.Assert(new Triple(uriNode, cscroHasModifier, cscroModifier));
            }
            return g;
        }

        private void GetFieldData(FieldDeclarationSyntax fieldDecl, out string sType, out string sIdentifier)
        {
            sType = string.Empty;
            sIdentifier = string.Empty;
            var varDeclChild = fieldDecl.ChildNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.VariableDeclaration));
            var varDecl = (VariableDeclarationSyntax)varDeclChild;
            var vrb = varDecl.Variables.FirstOrDefault();
            sIdentifier = vrb.Identifier.ToString();
            sType = varDecl.Type.ToString();
        }

        private void GetPropertyData(PropertyDeclarationSyntax propDecl, out string pType, out string pIdentifier)
        {
            pIdentifier = propDecl.Identifier.ToString();
            pType = propDecl.Type.ToString();
        }

        private void GetMethodData(MethodDeclarationSyntax methDecl, out string mReturnType, out string mIdentifier)
        {
            mIdentifier = methDecl.Identifier.ToString();
            mReturnType = methDecl.ReturnType.ToString();
        }

        private IGraph ExtractDataFromNode(IGraph g, string fileName, SyntaxNode node)
        {
            var rdfType = g.CreateUriNode("rdf:type");
            var rdfsLabel = g.CreateUriNode("rdfs:label");
            var cscroCompilationUnit = g.CreateUriNode("cscro:CompilationUnit");
            var cscroNamespace = g.CreateUriNode("cscro:Namespace");
            var cscroUses = g.CreateUriNode("cscro:uses");
            var cscroIsUsedBy = g.CreateUriNode("cscro:isUsedBy");
            var cscroHasNamespaceMember = g.CreateUriNode("cscro:hasNamespaceMember");
            var cscroIsNamespaceMemberOf = g.CreateUriNode("cscro:isNamespaceMemberOf");


            switch (node.RawKind)
            {
                case (int)SyntaxKind.CompilationUnit:
                    var compilationUnit = (CompilationUnitSyntax)node;
                    var cu = g.CreateUriNode(string.Format(":cu_{0}", compilationUnit.GetHashCode()));
                    g.Assert(new Triple(cu, rdfType, cscroCompilationUnit));
                    g.Assert(new Triple(cu, rdfsLabel, g.CreateLiteralNode(fileName, "en")));
                    break;
                case (int)SyntaxKind.UsingDirective:
                    var usingDir = (UsingDirectiveSyntax)node;
                    var ns = g.CreateUriNode(string.Format(":ns_{0}", usingDir.GetHashCode()));
                    g.Assert(new Triple(ns, rdfType, cscroNamespace));
                    g.Assert(new Triple(ns, rdfsLabel, g.CreateLiteralNode(usingDir.Name.ToString(), "en")));
                    if (usingDir.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)usingDir.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(ns, cscroIsUsedBy, parentCu));
                        g.Assert(new Triple(parentCu, cscroUses, ns));
                    }
                    else if (usingDir.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)usingDir.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(ns, cscroIsUsedBy, parentNs));
                        g.Assert(new Triple(parentNs, cscroUses, ns));
                    }
                    break;
                case (int)SyntaxKind.NamespaceDeclaration:
                    var namspaceDecl = (NamespaceDeclarationSyntax)node;
                    var nsDec = g.CreateUriNode(string.Format(":ns_{0}", namspaceDecl.GetHashCode()));
                    g.Assert(new Triple(nsDec, rdfType, cscroNamespace));
                    g.Assert(new Triple(nsDec, rdfsLabel, g.CreateLiteralNode(namspaceDecl.Name.ToString(), "en")));
                    if (namspaceDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)namspaceDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(nsDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, nsDec));
                    }
                    else if (namspaceDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)namspaceDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(nsDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, nsDec));
                    }
                    break;
                case (int)SyntaxKind.ClassDeclaration:
                    var classDecl = (ClassDeclarationSyntax)node;
                    var classDec = g.CreateUriNode(string.Format(":class_{0}", classDecl.GetHashCode()));
                    var isAbstract = classDecl.ChildTokens().Count(e => e.IsKind(SyntaxKind.AbstractKeyword)) > 0;
                    if (!isAbstract)
                    {
                        var isStatic = classDecl.ChildTokens().Count(e => e.IsKind(SyntaxKind.StaticKeyword)) > 0;
                        if (!isStatic)
                        {
                            var isSealed = classDecl.ChildTokens().Count(e => e.IsKind(SyntaxKind.SealedKeyword)) > 0;
                            if (!isSealed)
                            {
                                var cscroClass = g.CreateUriNode("cscro:Class");
                                g.Assert(new Triple(classDec, rdfType, cscroClass));
                            }
                            else
                            {
                                var cscroSealedClass = g.CreateUriNode("cscro:SealedClass");
                                g.Assert(new Triple(classDec, rdfType, cscroSealedClass));
                            }
                        }
                        else
                        {
                            var cscroStaticClass = g.CreateUriNode("cscro:StaticClass");
                            g.Assert(new Triple(classDec, rdfType, cscroStaticClass));
                        }
                    }
                    else
                    {
                        var cscroAbstractClass = g.CreateUriNode("cscro:AbstractClass");
                        g.Assert(new Triple(classDec, rdfType, cscroAbstractClass));
                    }
                    g.Assert(new Triple(classDec, rdfsLabel, g.CreateLiteralNode(classDecl.Identifier.ToString(), "en")));
                    if (classDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)classDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(classDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, classDec));
                    }
                    else if (classDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)classDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(classDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, classDec));
                    }
                    break;
                case (int)SyntaxKind.StructDeclaration:
                    var structDecl = (StructDeclarationSyntax)node;
                    var structDec = g.CreateUriNode(string.Format(":struct_{0}", structDecl.GetHashCode()));
                    var cscroStruct = g.CreateUriNode("cscro:Struct");
                    g.Assert(new Triple(structDec, rdfType, cscroStruct));
                    g.Assert(new Triple(structDec, rdfsLabel, g.CreateLiteralNode(structDecl.Identifier.ToString(), "en")));
                    if (structDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)structDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(structDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, structDec));
                    }
                    else if (structDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)structDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(structDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, structDec));
                    }
                    break;
                case (int)SyntaxKind.EnumDeclaration:
                    var enumDecl = (EnumDeclarationSyntax)node;
                    var enumDec = g.CreateUriNode(string.Format(":enum_{0}", enumDecl.GetHashCode()));
                    var cscroEnum = g.CreateUriNode("cscro:Enum");
                    g.Assert(new Triple(enumDec, rdfType, cscroEnum));
                    g.Assert(new Triple(enumDec, rdfsLabel, g.CreateLiteralNode(enumDecl.Identifier.ToString(), "en")));
                    if (enumDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)enumDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(enumDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, enumDec));
                    }
                    else if (enumDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)enumDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(enumDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, enumDec));
                    }
                    break;
                case (int)SyntaxKind.DelegateDeclaration:
                    var delegateDecl = (DelegateDeclarationSyntax)node;
                    var delegateDec = g.CreateUriNode(string.Format(":delegate_{0}", delegateDecl.GetHashCode()));
                    var cscroDelegate = g.CreateUriNode("cscro:Delegate");
                    g.Assert(new Triple(delegateDec, rdfType, cscroDelegate));
                    g.Assert(new Triple(delegateDec, rdfsLabel, g.CreateLiteralNode(delegateDecl.Identifier.ToString(), "en")));
                    if (delegateDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)delegateDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(delegateDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, delegateDec));
                    }
                    else if (delegateDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)delegateDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(delegateDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, delegateDec));
                    }
                    break;
                case (int)SyntaxKind.InterfaceDeclaration:
                    var interfaceDecl = (InterfaceDeclarationSyntax)node;
                    var interfaceDec = g.CreateUriNode(string.Format(":interface_{0}", interfaceDecl.GetHashCode()));
                    var cscroInterface = g.CreateUriNode("cscro:Interface");
                    g.Assert(new Triple(interfaceDec, rdfType, cscroInterface));
                    g.Assert(new Triple(interfaceDec, rdfsLabel, g.CreateLiteralNode(interfaceDecl.Identifier.ToString(), "en")));
                    if (interfaceDecl.Parent.IsKind(SyntaxKind.CompilationUnit))
                    {
                        var parent = (CompilationUnitSyntax)interfaceDecl.Parent;
                        var parentCu = g.CreateUriNode(string.Format(":cu_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(interfaceDec, cscroIsNamespaceMemberOf, parentCu));
                        g.Assert(new Triple(parentCu, cscroHasNamespaceMember, interfaceDec));
                    }
                    else if (interfaceDecl.Parent.IsKind(SyntaxKind.NamespaceDeclaration))
                    {
                        var parent = (NamespaceDeclarationSyntax)interfaceDecl.Parent;
                        var parentNs = g.CreateUriNode(string.Format(":ns_{0}", parent.GetHashCode()));
                        g.Assert(new Triple(interfaceDec, cscroIsNamespaceMemberOf, parentNs));
                        g.Assert(new Triple(parentNs, cscroHasNamespaceMember, interfaceDec));
                    }
                    break;
                //case (int)SyntaxKind.MethodDeclaration:
                //    break;
            }
            //ExtractDataFromNode(child);


            foreach (var child in node.ChildNodes())
            {
                g = ExtractDataFromNode(g, fileName, child);
            }
            //{
            //    switch (child.RawKind)
            //    {
            //        case (int)SyntaxKind.UsingDirective:
            //            var usingIdentifier = child.ChildTokens().FirstOrDefault(e => e.IsKind(SyntaxKind.IdentifierToken | SyntaxKind.QualifiedName));
            //            var usingName = usingIdentifier.Text;
            //            var ns = g.CreateUriNode(string.Format(":ns_{0}", child.GetHashCode()));
            //            var triple = new Triple(ns, rdfType, cscroNamespace);
            //            g.Assert(triple);
            //            break;
            //case (int)SyntaxKind.NamespaceDeclaration:
            //    var namespaceIdentifier = child.ChildTokens().FirstOrDefault(e => e.IsKind(SyntaxKind.IdentifierToken));
            //    var namespaceName = namespaceIdentifier.Text;
            //    g.NamespaceMap.AddNamespace(namespaceName, UriFactory.Create(string.Format("{0}{1}/", GlobalUri, namespaceName)));
            //    g.CreateUriNode(UriFactory.Create(string.Format("{0}{1}/", GlobalUri, namespaceName)));
            //    break;
            //case (int)SyntaxKind.ClassDeclaration:
            //    var classIdentifier = child.ChildTokens().FirstOrDefault(e => e.IsKind(SyntaxKind.IdentifierToken));
            //    var className = classIdentifier.Text;
            //    var parentNamespaceId = child.Parent.ChildTokens().FirstOrDefault(e => e.IsKind(SyntaxKind.IdentifierToken));
            //    var parentNamespaceName = parentNamespaceId.Text;
            //    var classNode = g.CreateUriNode(string.Format("{0}:{1}/", parentNamespaceName, className));
            //    break;
            //case (int)SyntaxKind.MethodDeclaration:
            //    break;
            //}
            //ExtractDataFromNode(child);
            //}
            return g;
        }


        private Ontology _ontology;
        private TripleStore _tripleStore;


        private void WriteNode(SyntaxNode node, int tabs)
        {
            for (var i = 0; i < tabs; i++)
            {
                Console.Write("\t");
            }

            var kind = ((SyntaxKind)node.RawKind).ToString();
            Console.WriteLine(kind);

            foreach (var d in node.ChildNodes())
            {
                WriteNode(d, tabs + 1);
            }
        }
    }
}
