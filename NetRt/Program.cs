using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using NetInterface;
using NetRt.Assemblies;
using NetRt.Common;
using NetRt.PAL;
using NetRt.TypeLoad;

namespace NetRt
{
    internal static unsafe class Program
    {
        public static void Main(string[] args)
        {
            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(DutchNameGenerator(6));
            }
        }

        private static string[] _dutchShit = { "ik", "van", "e", "ee", "or" };
        private static Random _rng = new Random();
        public static string DutchNameGenerator(int len)
        {
            var name = new string[len];
            for (var i = 0; i < name.Length; i++)
            {
                name[i] = _dutchShit[_rng.Next(0, _dutchShit.Length)];
            }
            return string.Join("", name);
        }

        private static int Main2(string[] args)
        {
            if (!BitConverter.IsLittleEndian)  // TODO make work on big endian (stream reads)
                throw new Exception("fuck off and get a proper architecture jeez");

#if DEBUG
            args = new string[1];
            args[0] = Console.ReadLine();
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
                using var file = new FileStream(args[0], FileMode.Open, FileAccess.Read, FileShare.None);
                var reader = new CliImage.CliImageReader();
                reader.CreateFromStream(new Disposable<Stream>(file, owned: true), args[0]);
                var metadata = new MetadataReader(reader.Image, file);
                var loader = new TypeLoad.TypeLoader(reader.Image, file);

                foreach (TypeDef typeDef in metadata.EnumerateTypeDefs())   
                {
                    Console.WriteLine("TypeDef: " + MakeName(typeDef.TypeNamespace, typeDef.TypeName));

                    int n = 0;
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
            catch (IOException e)
            {
                Console.WriteLine(e);
                return ReturnCodes.FileNotFound;
            }

            return ReturnCodes.Success;
        }

        private static string MakeName(string typeNamespace, string typeName)
        {
            if (string.IsNullOrEmpty(typeNamespace))
                return typeName;

            return typeNamespace + "." + typeName;
        }

        public static void PrintHelp()
        {

        }
}
}
