using System;
using System.Runtime.InteropServices;
using NetInterface;

public delegate int Dummy_Main_Del();


public class NetJit : Jit
{

    public override unsafe byte* JitMethod(MethodDef method)
    {
        return (byte*)Marshal.GetFunctionPointerForDelegate<Dummy_Main_Del>(Dummy_Main);
    }

    private static int Dummy_Main()
    {
        Console.WriteLine("Hello World!");
        return 0;
    }
}
