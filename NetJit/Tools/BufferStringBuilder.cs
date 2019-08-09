using System;

namespace NetJit.Tools
{
    public ref struct BufferStringBuilder
    {
        public Span<char> Buffer { get; }
        public Span<char> RemainingBuffer => Buffer.Slice(Position);
        public int Position { get; private set; }

        public bool IsBufferEmpty => Buffer.IsEmpty;
        public bool IsBufferFull => Position == Buffer.Length;
        public int RemainingCapacity => Buffer.Length - Position;

        public BufferStringBuilder(Span<char> buffer, int position = 0)
        {
            Buffer = buffer;
            Position = position;
        }

        public bool TryAdd(char c)
        {
            if (IsBufferFull) return false;

            Buffer[Position++] = c;
            return true;
        }

        public bool TryAddNewline() => TryAdd('\n');
        public bool TryAddNewline(ref int charsWritten) => TryAdd('\n', ref charsWritten);

        public bool TryAdd(char c, ref int charsWritten)
        {
            if (IsBufferFull) return false;

            Buffer[Position++] = c;
            charsWritten++;
            return true;
        }

        public bool TryAdd(string s)
        {
            if (!s.AsSpan().TryCopyTo(RemainingBuffer)) return false;

            Position += s.Length;
            return true;
        }
        public bool TryAdd(string s, ref int charsWritten)
        {
            if (!s.AsSpan().TryCopyTo(RemainingBuffer)) return false;
            
            Position += s.Length;
            charsWritten += s.Length;
            return true;
        }

        public bool TryAdd<T>(T t) => TryAdd(t.ToString());

        public bool TryAdd<T>(T t, ref int charsWritten) => TryAdd(t.ToString(), ref charsWritten);
    }
}