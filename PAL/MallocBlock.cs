using System;
using System.Buffers;
using Common;
using Microsoft.Win32.SafeHandles;

namespace PAL
{
    public sealed class SafeMallocHandle : SafeHandleMinusOneIsInvalid
    {
        public SafeMallocHandle(IntPtr handle, bool ownsHandle = true) : base(ownsHandle)
        {
            base.handle = handle;
        }

        protected override unsafe bool ReleaseHandle()
        {
            CrtMemNativeMethods.free((void*)handle);
            return true;
        }
    }

    public sealed unsafe class MallocBlock<T> : MemoryManager<T> where T : unmanaged
    {
        private readonly SafeMallocHandle _handle;
        private readonly int _length;

        public static MallocBlock<T> Create(void* handle, IntPtr len) => new MallocBlock<T>(handle, len);
        public static MallocBlock<T> Allocate(int size)
        {
            void* handle = Crt.malloc(size);
            if (handle == null)
            {
                ThrowHelper.ThrowInsufficientMemoryException($"{nameof(Crt.malloc)} returned NULL");
            }
            return new MallocBlock<T>(handle, (IntPtr) size);
        }

        private MallocBlock(void* handle, IntPtr len)
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));
            _handle = new SafeMallocHandle((IntPtr)handle);
            _length = (int)len;
        }

        protected override void Dispose(bool disposing)
        {
            _handle.Dispose();
        }

        public override Span<T> GetSpan()
        {
            return new Span<T>((void*)_handle.DangerousGetHandle(), _length);
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            var success = false;

            do
            {
                _handle.DangerousAddRef(ref success);
            } while (!success);

            return new MemoryHandle(pointer: _handle.DangerousGetHandle().ToPointer(),
                                    pinnable: this);
        }

        public override void Unpin()
        {
            _handle.DangerousRelease();
        }
    }
}