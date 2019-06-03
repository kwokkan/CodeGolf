﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace CodeGolf.Service
{
    public class CancellationTokenInjector : CSharpSyntaxRewriter
    {
        private const string TokenName = "cancellationToken";

        private static readonly StatementSyntax ThrowIfCancelled = ParseStatement(
            @"if (" + TokenName + @".IsCancellationRequested)
                {
                    throw new System.Threading.Tasks.TaskCanceledException();
                }").NormalizeWhitespace();

        private IList<string> ModifiedFuncs { get; } = new List<string>();

        public override SyntaxNode VisitLabeledStatement(LabeledStatementSyntax node)
        {
            var statement = (GotoStatementSyntax) node.Statement;
            var updated = Block(ThrowIfCancelled, statement);
            return base.VisitLabeledStatement(node.ReplaceNode(node.Statement, updated));
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var statements = node.Body != null
                ? node.Body.Statements.ToArray()
                : new[] {ReturnStatement(node.ExpressionBody.Expression)};
            var updated = node.AddParameterListParameters(Parameter(Identifier(TokenName))
                    .WithType(ParseTypeName(typeof(CancellationToken).FullName)))
                .WithBody(Block(new[] {ThrowIfCancelled}.Concat(statements)))
                .WithExpressionBody(null)
                .WithoutTrivia().WithSemicolonToken(Token(SyntaxKind.None));

            this.ModifiedFuncs.Add(node.Identifier.ValueText);
            return base.VisitMethodDeclaration(node.ReplaceNode(node, updated));
        }

        public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node)
        {
            var statement = (BlockSyntax) node.Statement;
            var updated = statement.AddStatements(ThrowIfCancelled);
            return base.VisitWhileStatement(node.ReplaceNode(statement, updated));
        }

        public override SyntaxNode VisitForStatement(ForStatementSyntax node)
        {
            var statement = (BlockSyntax) node.Statement;
            var updated = statement.AddStatements(ThrowIfCancelled);
            return base.VisitForStatement(node.ReplaceNode(statement, updated));
        }

        public override SyntaxNode VisitDoStatement(DoStatementSyntax node)
        {
            var statement = (BlockSyntax) node.Statement;
            var updated = statement.AddStatements(ThrowIfCancelled);
            return base.VisitDoStatement(node.ReplaceNode(statement, updated));
        }

        public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node)
        {
            var statement = (BlockSyntax) node.Statement;
            var updated = statement.AddStatements(ThrowIfCancelled);
            return base.VisitForEachStatement(node.ReplaceNode(statement, updated));
        }

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            if (node.Expression is IdentifierNameSyntax ins && this.ModifiedFuncs.Contains(ins.Identifier.ValueText))
            {
                var updated = node.AddArgumentListArguments(Argument(
                    IdentifierName(TokenName)));
                return base.VisitInvocationExpression(node.ReplaceNode(node, updated).NormalizeWhitespace());
            }
            else
            {
                return base.VisitInvocationExpression(node);
            }
        }
    }
}