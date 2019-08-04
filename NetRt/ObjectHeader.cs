using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


/*
 * bit 31 - HasFinalizerRun - has finalizer run
 * bit 30 - IsGcMarked - cannot be collected if true
 * bit 29 - IsGcPinned - cannot be moved if true
 * bit 28 - HasSyncBlk - if this type has a sync blk or not
 * bit 27 - IsLocked - if this object is currently locked
 * bit 26 - RESERVED
 * bit 25 - RESERVED
 * bit 24 - RESERVED
 * 
 * if HasSyncBlk == 1:
 *      bit 23 - 0 (24 bits) - sync block index
 * else
 *      bit 23 - 15 (8 bits) - lock recursion level (anymore recursion results in a managed LockRecursionException)
 *      bit 15 - 0 (16 bits) - thread ID of owning thread
 */

#if BIT64
    using nuint = System.UInt64;
#else
    using nuint = System.UInt32;
#endif

namespace NetRt
{
    public unsafe struct ObjectHeader
    {
        // IntPtr size for alignment
        private nuint _value;

        #region Bitmasks
        // Use unsigned so we can use sign (these are zero extended for 64 bit)
        private static readonly nuint HasFinalizerRunMask = 0b_1000_0000__0000_0000__0000_0000__0000_0000;
        private static readonly nuint IsGcMarkedMask = 0b_0100_0000__0000_0000__0000_0000__0000_0000;
        private static readonly nuint IsGcPinnedMask = 0b_0010_0000__0000_0000__0000_0000__0000_0000;
        private static readonly nuint HasSyncBlkMask = 0b_0001_0000__0000_0000__0000_0000__0000_0000;
        private static readonly nuint IsLockedMask = 0b_0000_1000__0000_0000__0000_0000__0000_0000;

        private static readonly nuint SyncBlkIndexMask = 0b_0000_0000__1111_1111__1111_1111__1111_1111;

        private static readonly nuint LockRecursionLevelMask = 0b_0000_0000__1111_1111__0000_0000__0000_0000;
        private static readonly nuint ThreadIdMask = 0b_0000_0000__0000_0000__1111_1111__1111_1111;
        #endregion

        #region Bit Properties
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool GetBit(nuint mask) => (_value & mask) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetBit(nuint mask, bool value)
        {
            if (value)
            {
                _value |= mask;
            }
            else
            {
                _value &= ~mask;
            }
        }


        public bool HasFinalizerRun
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBit(HasFinalizerRunMask);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetBit(HasFinalizerRunMask, value);
        }

        public bool IsGcMarked
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBit(IsGcMarkedMask);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetBit(IsGcMarkedMask, value);
        }

        public bool IsGcPinned
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBit(IsGcPinnedMask);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetBit(IsGcPinnedMask, value);
        }

        public bool HasSyncBlk
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBit(HasSyncBlkMask);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetBit(HasSyncBlkMask, value);
        }

        public bool IsLockedRun
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GetBit(IsLockedMask);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => SetBit(IsLockedMask, value);
        }
        #endregion

        #region Region Properties

        public int GetRegion(nuint mask)
        {
            return (int)(_value & mask);
        }

        public void SetRegion(nuint mask, int value)
        {
            Debug.Assert((value & mask) == value);
            _value |= (uint)value;
        }

        public int SyncBlkIndex
        {
            get
            {
                Debug.Assert(HasSyncBlk);
                return GetRegion(SyncBlkIndexMask);
            }

            set
            {
                Debug.Assert(HasSyncBlk);
                SetRegion(SyncBlkIndexMask, value);
            }
        }

        public int LockRecursionLevel
        {
            get
            {
                Debug.Assert(!HasSyncBlk);
                return GetRegion(LockRecursionLevelMask);
            }

            set
            {
                Debug.Assert(!HasSyncBlk);
                SetRegion(LockRecursionLevelMask, value);
            }
        }

        public int ThreadId
        {
            get
            {
                Debug.Assert(!HasSyncBlk);
                return GetRegion(ThreadIdMask);
            }

            set
            {
                Debug.Assert(!HasSyncBlk);
                SetRegion(ThreadIdMask, value);
            }
        }
        #endregion

        public static int Size => sizeof(ObjectHeader);
    }
}