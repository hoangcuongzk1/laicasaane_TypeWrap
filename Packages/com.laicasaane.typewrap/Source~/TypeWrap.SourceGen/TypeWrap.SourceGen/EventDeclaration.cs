using Microsoft.CodeAnalysis;
using SourceGen.Common;

namespace TypeWrap.SourceGen
{
    public struct EventDeclaration
    {
        public string name;
        public string typeName;
        public int explicitInterfaceImplementationsLength;
        public bool isPublic;
        public bool isStatic;

        public static EventDeclaration Create(IEventSymbol evt, INamedTypeSymbol fieldTypeSymbol)
        {
            return new EventDeclaration {
                name = evt.ToDisplayString(SymbolExtensions.QualifiedMemberNameWithGlobalPrefix),
                typeName = evt.Type.ToFullName(),
                isPublic = evt.DeclaredAccessibility == Accessibility.Public,
                isStatic = evt.IsStatic,
                explicitInterfaceImplementationsLength = evt.ExplicitInterfaceImplementations.Length,
            };
        }
    }
}
