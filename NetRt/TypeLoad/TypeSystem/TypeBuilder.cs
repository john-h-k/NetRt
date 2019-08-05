﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NetInterface;
using NetRt.Assemblies;

namespace NetRt.TypeLoad.TypeSystem
{
    public sealed class TypeBuilder
    {
        public TypeBuilder()
        {

        }

        private readonly List<Field> _fields = new List<Field>();
        private readonly List<MethodDef> _methods = new List<MethodDef>();

        private readonly bool _makeByRef = false;
        private readonly bool _makePointer = false;

        public void Add(Field field) => _fields.Add(field);
        public void Add(MethodDef methodDef) => _methods.Add(methodDef);

        public TypeInformation Finalize()
        {
            throw new NotImplementedException();
        }


        public static ByRefType MakeByRefType(TypeInformation type)
        {
            if (type is ByRefType)
                throw new ArgumentException("Cannot make a byref to a byref");

            return new ByRefType(type);
        }

        public static ByRefType MakePointerType(TypeInformation type)
        {
            if (type is ByRefType)
                throw new ArgumentException("Cannot make a byref to a byref");

            return new ByRefType(type);
        }
    }
}