using System.IO;
using System.Net.Mime;
using System.Reflection.PortableExecutable;
using NetRt.Common;

namespace NetRt.Assemblies
{
    public sealed class Assembly
    {
        private readonly CliImage _image;

        public Assembly(Stream stream, string name)
        {
            var reader = new CliImage.CliImageReader();
            reader.CreateFromStream(new Disposable<Stream>(stream, true), name);
            _image = reader.Image;
        }
    }
}