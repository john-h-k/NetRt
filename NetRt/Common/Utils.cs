using System;

namespace NetRt.Common
{
    //public static class Utils
    //{
    //    public static int ReadVarLenUInt32(ref Span<byte> span)
    //    {
    //        int len;
    //        if ((span[0] & 0b1000_0000) == 0)
    //        {
    //            len = span[0];
    //            span = span.Slice(1);
    //        }
    //        else if ((span[0] & 0b0100_0000) == 0)
    //        {
    //            len = ((span[0] & ~0b1000_0000) << 8) | span[1];
    //            span = span.Slice(2);
    //        }
    //        else
    //        {
    //            len = ((span[0] & ~0b1100_0000) << 24) | (span[1] << 16) | (span[2] << 8) | span[3];
    //            span = span.Slice(4);
    //        }

    //        return len;
    //    }
    //}
}