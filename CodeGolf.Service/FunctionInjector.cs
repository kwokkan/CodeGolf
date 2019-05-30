using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGolf.Service
{
    public class FunctionInjector : CSharpSyntaxRewriter
    {
        private readonly MemberDeclarationSyntax[] methods;

        public FunctionInjector(MemberDeclarationSyntax[] methods)
        {
            this.methods = methods;
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var updated = node.AddMembers(this.methods);
            return base.VisitClassDeclaration(node.ReplaceNode(node, updated));
        }
    }
}