using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NewCryptoParser.Exceptions;
using System.Reflection;

namespace NewCryptoParser;

internal class CodeCompiler
{
    private static List<MetadataReference> _references = new List<MetadataReference>();

    public static T CompileCodeAndGetObject<T>(string code) where T : class
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

        CSharpCompilation compilation = CSharpCompilation.Create(
            Path.GetRandomFileName(),
            syntaxTrees: new[] { syntaxTree },
            references: _references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();

        EmitResult result = compilation.Emit(ms);
        if (!result.Success)
        {
            IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

            var exceptions = new List<Exception>();
            foreach (Diagnostic diagnostic in failures)
            {
                exceptions.Add(new Exception(diagnostic.GetMessage()));
            }
            throw new CompilerException(exceptions.ToArray());
        }

        ms.Seek(0, SeekOrigin.Begin);

        Assembly assembly = Assembly.Load(ms.ToArray());
        T? obj = null;
        foreach (var item in assembly.GetTypes())
        {
            try
            {
                obj = Activator.CreateInstance(item) as T;
                if (obj is not null)
                    break;
            }
            catch { } //Trying to create first suitable type
        }
        if (obj == null)
            throw new Exception("Cannot find class");
        return obj;
    }
    static CodeCompiler()
    {
        loadDefaultReferences();
    }
    private static void loadDefaultReferences()
    {
        var assemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
        if (assemblies != null)
            foreach (var r in ((string)assemblies).Split(Path.PathSeparator))
                _references.Add(MetadataReference.CreateFromFile(r));
        else
            throw new Exception();
    }
}