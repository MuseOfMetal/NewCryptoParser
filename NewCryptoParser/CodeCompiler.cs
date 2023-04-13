using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;

namespace NewCryptoParser;

#region V.1
//internal class CodeCompiler
//{
//    private static List<MetadataReference> _references = new List<MetadataReference>();
//    public static T GetObject<T>(MemoryStream stream) where T : class
//    {
//        Assembly assembly = Assembly.Load(stream.ToArray());
//        T? obj = null;
//        foreach (var item in assembly.GetTypes())
//        {
//            try
//            {
//                obj = Activator.CreateInstance(item) as T;
//                if (obj is not null)
//                    break;
//            }
//            catch { } //Trying to create first suitable type

//        }
//        if (obj == null)
//            throw new Exception("Cannot find type");
//        return obj;
//    }
//    public static MemoryStream CompileCode(string path, out EmitResult result)
//    {
//        SyntaxTree syntaxTree;
//        using (StreamReader r = new StreamReader(path))
//        {
//            syntaxTree = CSharpSyntaxTree.ParseText(r.ReadToEnd());
//        }
//        CSharpCompilation compilation = CSharpCompilation.Create(
//            Path.GetRandomFileName(),
//            syntaxTrees: new[] { syntaxTree },
//            references: _references,
//            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

//        using var stream = new MemoryStream();

//        result = compilation.Emit(stream);

//        stream.Seek(0, SeekOrigin.Begin);

//        return stream;
//    }
//    static CodeCompiler()
//    {
//        loadDefaultReferences();
//    }

//    private static void loadDefaultReferences()
//    {
//        var assemblies = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
//        if (assemblies != null)
//        {
//            foreach (var r in ((string)assemblies).Split(Path.PathSeparator))
//            {
//                _references.Add(MetadataReference.CreateFromFile(r));
//            }
//        }
//        else
//        {
//            throw new Exception();
//        }
//    }
//}
#endregion

#region V.2
internal class CodeCompiler
{
    private static List<MetadataReference> _references = new List<MetadataReference>();
    public static T CompileFileAndGetObject<T>(string path) where T : class
    {
        #region Load and compile .cs file (Do not touch)
        SyntaxTree syntaxTree;
        using (StreamReader r = new StreamReader(path))
        {
            syntaxTree = CSharpSyntaxTree.ParseText(r.ReadToEnd());
        }


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
            throw new AggregateException(exceptions.ToArray());
        }

        ms.Seek(0, SeekOrigin.Begin);
        #endregion

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
        {
            foreach (var r in ((string)assemblies).Split(Path.PathSeparator))
            {
                _references.Add(MetadataReference.CreateFromFile(r));
            }
        }
        else
        {
            throw new Exception();
        }
    }
}
#endregion