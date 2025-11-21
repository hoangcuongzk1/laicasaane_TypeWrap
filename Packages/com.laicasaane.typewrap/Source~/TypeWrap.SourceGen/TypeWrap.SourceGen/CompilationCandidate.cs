using System;
using Microsoft.CodeAnalysis;
using SourceGen.Common;

namespace TypeWrap.SourceGen
{
    public struct CompilationCandidate : IEquatable<CompilationCandidate>
    {
        public string assemblyName;
        public References references;
        public bool enableNullable;
        public bool isValid;

        /// <summary>
        /// Do not store <paramref name="compilation"/>.
        /// </summary>
        public static CompilationCandidate GetCompilation(
              Compilation compilation
            , string generatorNamespace
            , string skipAttribute
        )
        {
            var references = new References();

            foreach (var assembly in compilation.ReferencedAssemblyNames)
            {
                if (assembly.Name is "Sirenix.OdinInspector.Attributes")
                {
                    references.odin = true;
                    continue;
                }

                if (assembly.Name.StartsWith("UnityEngine"))
                {
                    references.unity = true;
                    continue;
                }

                if (assembly.Name is "Unity.Collections")
                {
                    references.unityCollections = true;
                    continue;
                }
            }

            return new CompilationCandidate {
                assemblyName = compilation.Assembly.Name,
                references = references,
                enableNullable = compilation.Options.NullableContextOptions != NullableContextOptions.Disable,
                isValid = compilation.IsValidCompilation(generatorNamespace, skipAttribute),
            };
        }

        public readonly override bool Equals(object obj)
            => obj is CompilationCandidate other && Equals(other);

        public readonly bool Equals(CompilationCandidate other)
            => string.Equals(assemblyName, other.assemblyName, StringComparison.Ordinal)
            && references.Equals(other.references)
            && enableNullable == other.enableNullable
            ;

        public readonly override int GetHashCode()
            => HashValue.Combine(assemblyName, references, enableNullable);
    }

    public struct References
    {
        public bool odin;
        public bool unity;
        public bool unityCollections;
    }
}
