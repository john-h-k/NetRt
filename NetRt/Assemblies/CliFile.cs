using System.IO;

namespace NetRt.Assemblies
{
    public class CliFile
    {
        private FormatValidator _validator;
        private Stream _stream;

        public CliFile(string fileName)
        {
            _validator = FormatValidator.Create(fileName);
            _stream = new FileStream(fileName, FileMode.Open);
        }

        public CliFile(Stream stream)
        {
            _validator = FormatValidator.Create(stream);
            _stream = stream;
        }
    }
}