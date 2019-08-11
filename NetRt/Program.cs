using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using NetRt.Assemblies;
using NetRt.Common;
using NetRt.Interfaces;
using NetRt.Metadata;
using NetRt.TypeLoad;
using ReflectionAssembly = System.Reflection.Assembly;

namespace NetRt
{
    internal static unsafe class Program
    {

        private static int Main(string[] args)
        {
            if (!BitConverter.IsLittleEndian)  // TODO make work on big endian (stream reads)
                throw new Exception("fuck off and get a proper architecture jeez");

#if DEBUG
            args = new[] { @"C:\Users\johnk\source\repos\HelloWorld\HelloWorld\bin\Debug\netcoreapp3.0\HelloWorld.dll" };
#else
            if (args is null || args.Length == 0)
            {
                Console.WriteLine(CliResources.CmdArgs_NullOrEmpty);
                return ReturnCodes.NoCmdLineArgs;
            }
#endif

            Debug.WriteLine($"Executing exe \"{args[0]}\"");

            try
            {
                using var file = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.Read);
                var mem = new MemoryStream(checked((int)file.Length));
                file.CopyTo(mem);
                var reader = new CliImage.CliImageReader(new Disposable<Stream>(mem, owned: true), args[0]);
                var metadata = new MetadataReader(reader.Image, mem);
                MethodDef method = metadata.ReadMethodDef(reader.Image.EntryPointToken);
                MethodInformation methodInfo = metadata.ReadMethod(method);
                Console.WriteLine(methodInfo);

                Jit.Initialize(methodInfo);
                Console.WriteLine(Jit.CreateJitDump());
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                return ReturnCodes.FileNotFound;
            }

            return ReturnCodes.Success;
        }

        private static readonly Jit Jit;

        static Program()
        {
            //ReadOnlySpan<char> location = ReflectionAssembly.GetExecutingAssembly().Location.AsSpan();
            //location = location.Slice(0, location.LastIndexOf('\\'));

            //ReflectionAssembly jit = ReflectionAssembly.LoadFile(Path.Join(location,"NetJit.dll"));
            ReflectionAssembly jit = ReflectionAssembly.LoadFile(Path.Join(@"C:\Users\johnk\source\repos\NetRt\NetJit\bin\Debug\netcoreapp3.0", "NetJit.dll"));
            Type type = jit.GetType("NetJit.Compiler");

            if (!type.IsSubclassOf(typeof(Jit)))
                ThrowHelper.ThrowNotSupportedException(
            "Provided JIT is not derived from NetRt.Jit");

            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            Jit = Jit.Instance;

            if (Jit is null)
                ThrowHelper.ThrowInvalidOperationException(
                    "Provided JIT did not properly initialize NetRt.Jit.Instance");
        }


        private static string MakeName(string typeNamespace, string typeName)
        {
            if (string.IsNullOrEmpty(typeNamespace))
                return typeName;

            return typeNamespace + "." + typeName;
        }

        public static void PrintTypes(string name)
        {
            using var file = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.None);
            var reader = new CliImage.CliImageReader(new Disposable<Stream>(file, owned: true), name);
            var metadata = new MetadataReader(reader.Image, file);
            var loader = new TypeLoader(reader.Image, file);

            foreach (TypeDef typeDef in metadata.EnumerateTypeDefs())
            {
                Console.WriteLine("TypeDef: " + MakeName(typeDef.TypeNamespace, typeDef.TypeName));

                var n = 0;
                foreach (Field field in metadata.EnumerateFields(typeDef))
                {
                    Console.WriteLine($"\tField {n++}: {(field.Flags.HasFlag(FieldAttributes.Static) ? "static" : "")} {field.Name}");
                }

                n = 0;
                foreach (MethodDef method in metadata.EnumerateMethods(typeDef))
                {
                    Console.WriteLine($"\tMethodDef {n++}: {(method.Flags.HasFlag(MethodAttributes.Static) ? "static" : "")} {method.Name}");
                }
            }
        }
    }
}
