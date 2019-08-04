using System;
using System.IO;
using NetRt.Assemblies;
using NetRt.Assemblies.Heaps;
using NetRt.TypeLoad.TypeSystem;

namespace NetRt.TypeLoad
{
    public sealed class TypeLoader
    {
        private readonly CliImage _image;
        private readonly Stream _stream;
        private readonly MetadataReader _reader;
        private readonly uint _typeTableStart;
        private readonly TableHeap.TableInfo _typeTableInfo;

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

        public TypeDefinition LoadType(TypeDef typeDef)
        {
            throw new NotImplementedException();

            //LoadStaticFields(typeDef);
            //GenerateMethodTable(typeDef);
            //if (!typeDef.HasBeforefieldinit)
            //    typeDef.RunCctor();
        }

        public TypeDef ResolveTypeRef(TypeRef typeRef)
        {
            throw new NotImplementedException();
        }

        
    }
}