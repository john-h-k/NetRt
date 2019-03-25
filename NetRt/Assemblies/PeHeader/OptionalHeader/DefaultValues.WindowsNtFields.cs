using System;
using System.Collections.Generic;
using System.Text;

namespace NetRt.Assemblies.PeHeader.OptionalHeader
{
    public partial struct WindowsNtFields
    {
        public enum FileAlignmentValue : uint
        {
            Default = 0x200
        }

        public enum OsMajorValue : ushort
        {
            Default = 5
        }

        public enum OsMinorValue : ushort
        {
            Default = 0x200
        }

        public enum UserMajorValue : ushort
        {
            Default = 0
        }

        public enum UserMinorValue : ushort
        {
            Default = 0
        }

        public enum SubSysMajorValue : ushort
        {
            Default = 5
        }

        public enum SubSysMinorValue : ushort
        {
            Default = 0
        }

        public enum ReservedValue : uint
        {
            Default = 0
        }

        public enum FileChecksumValue : uint
        {
            Default = 0
        }

        public enum StackReserveSizeValue : uint
        {
            Default = 0x100000
        }

        public enum StackCommitSizeValue : uint
        {
            Default = 0x1000
        }

        public enum HeapReserveSizeValue : uint
        {
            Default = 0x100000
        }

        public enum HeapCommitSizeValue : uint
        {
            Default = 0x1000
        }

        public enum LoaderFlagsValue : uint
        {
            Default = 0
        }

        public enum NumberOfDataDirectoriesValue : uint
        {
            Default = 0x10
        }
    }
}
