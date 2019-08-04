using System;
using System.IO;
using System.Text;

namespace NetRt
{
    internal unsafe class FormatValidator
    {
        private readonly FormatValidatorOptions _options;
        private readonly BinaryReader _file;

        // Don't marshal this string to unmanaged code or weird results will
        // occur. \0\0 null chars will confuse C or C++
        private static readonly char[] PeHeaderMarker = { 'P', 'E', '\0', '\0' };

        private FormatValidator(Stream stream, FormatValidatorOptions options)
        {
            _file = new BinaryReader(stream, new UTF8Encoding(), leaveOpen: true);
            _options = options;

            _file.BaseStream.Position = 0x3c;
            _lfanew = _file.ReadUInt32();

            _file.BaseStream.Position = 0;
        }

        public static FormatValidator Create(string fileName,
            FormatValidatorOptions options = FormatValidatorOptions.None)
        {
            if (!File.Exists(fileName))
                throw new ArgumentException($"File \"{fileName}\" doesn't exist", nameof(fileName));

            return Create(
                new FileStream(fileName, FileMode.Open),
                options
            );
        }

        public static FormatValidator Create(Stream stream,
            FormatValidatorOptions options = FormatValidatorOptions.None)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanRead || !stream.CanSeek)
                throw new ArgumentException($"{nameof(stream)} must be seekable and readable",
                    nameof(stream));

            var validator = new FormatValidator
            (
                stream,
                options
            );

            return validator;
        }

        private readonly byte[] _msDosHeader =
        {
            0x4d, 0x5a, 0x90, 0x00, 0x03, 0x00, 0x00, 0x00,
            0x04, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00,
            0xb8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, /* begin lfanew */ 0xDE, 0xAD, 0xBE, 0xEF, /* end lfanew */
            0x0e, 0x1f, 0xba, 0x0e, 0x00, 0xb4, 0x09, 0xcd,
            0x21, 0xb8, 0x01, 0x4c, 0xcd, 0x21, 0x54, 0x68,
            0x69, 0x73, 0x20, 0x70, 0x72, 0x6f, 0x67, 0x72,
            0x61, 0x6d, 0x20, 0x63, 0x61, 0x6e, 0x6e, 0x6f,
            0x74, 0x20, 0x62, 0x65, 0x20, 0x72, 0x75, 0x6e,
            0x20, 0x69, 0x6e, 0x20, 0x44, 0x4f, 0x53, 0x20,
            0x6d, 0x6f, 0x64, 0x65, 0x2e, 0x0d, 0x0d, 0x0a,
            0x24, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        }; // We mutate this. Don't make it static

        private readonly uint _lfanew;

        public bool ValidateMsDosHeader()
        {
            _file.BaseStream.Position = 0;

            Span<byte> buffer = stackalloc byte[_msDosHeader.Length];
            _file.Read(buffer);

            // Copy over lfanew
            _msDosHeader[0x3c + 0] = buffer[0x3c + 0];
            _msDosHeader[0x3c + 1] = buffer[0x3c + 1];
            _msDosHeader[0x3c + 2] = buffer[0x3c + 2];
            _msDosHeader[0x3c + 3] = buffer[0x3c + 3];

            return buffer.SequenceEqual(_msDosHeader);
        }

        public bool ValidatePeFileHeader()
        {
            _file.BaseStream.Position = _lfanew;

            ReadOnlySpan<char> chars = _file.ReadChars(4);
            if (!chars.SequenceEqual(PeHeaderMarker))
                return false;

            ushort machine = _file.ReadUInt16();

            if (machine != 0x14c)
                return false;

            _file.BaseStream.Seek(8, SeekOrigin.Current);

            // Pointer to Symbol Table AND Number of Symbols
            if (_file.ReadUInt64() != 0)
                return false;

            throw new NotImplementedException();
        }
    }
}