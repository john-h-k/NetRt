using System;

namespace Common
{
    public static class Utils
    {
        public static uint ReadVarLenUInt32(ref Span<byte> span)
        {
            uint len;
            if ((span[0] & 0b1000_0000) == 0)
            {
                len = span[0];
                span = span.Slice(1);
            }
            else if ((span[0] & 0b1100_0000) == 0b1000_0000)
            {
                len = ((span[0] & ~0b1000_0000U) << 8) | span[1];
                span = span.Slice(2);
            }
            else
            {
                len = ((span[0] & ~0b1110_0000U) << 24) | (uint)(span[1] << 16) | (uint)(span[2] << 8) | span[3];
                span = span.Slice(4);
            }

            return len;
        }
    }
}