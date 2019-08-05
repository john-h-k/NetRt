using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using NetRt.Assemblies;
using NetRt.Assemblies.Heaps;
using NetRt.TypeLoad.TypeSystem;
using PAL;

namespace NetRt.TypeLoad
{
    public sealed class TypeLoader
    {
        private readonly CliImage _image;
        private readonly Stream _stream;
        private readonly MetadataReader _reader;
        private readonly uint _typeTableStart;
        private readonly TableHeap.TableInfo _typeTableInfo;
        private readonly Dictionary<TypeDef, TypeInformation> _loadedTypes = new Dictionary<TypeDef, TypeInformation>();

        public TypeLoader(CliImage image, Stream stream)
        {
            _image = image;
            _stream = stream;
            _reader = new MetadataReader(image, stream);

            _typeTableInfo = _image.TableHeap[TableHeap.Table.TypeDef];

            _typeTableStart =
                _image.MetadataSection.PointerToRawData +
                _image.TableHeapOffset +
                _typeTableInfo.Offset;
        }

        public TypeInformation EnsureTypeLoaded(TypeDef typeDef)
        {
            if (_loadedTypes.TryGetValue(typeDef, out TypeInformation type)) return type;
            LoadStaticFields(typeDef);
            //GenerateMethodTable(typeDef);
            //if (!typeDef.HasBeforefieldinit)
            //    typeDef.RunCctor();

            throw new NotImplementedException();
        }

        private void LoadStaticFields(TypeDef typeDef)
        {
            foreach (Field field in _reader.EnumerateFields(typeDef))
            {
                if (!field.Flags.HasFlag(FieldAttributes.Static)) continue;

                TypeInformation fieldType = EnsureTypeLoaded(((dynamic)field.ReadSignature()).FieldType);
                MallocBlock<byte> fieldData = MallocBlock<byte>.Allocate(fieldType.Size);

            }
        }

        public TypeDef ResolveTypeRef(TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        
    }
}