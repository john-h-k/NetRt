using System;
using Common;

namespace NetJit.Tools
{
    public ref struct BufferStringBuilder
    {
        public Span<char> Buffer { get; }
        public Span<char> RemainingBuffer => Buffer.Slice(Position);

        private int _position;

        public int Position
        {
            get => _position;
            private set
            {
                if (value < 0 || value >= Buffer.Length) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                _position = value;
            }
        }

        private int _tabs;
        public int Tabs
        {
            get => _tabs;
            set
            {
                if (value < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                _tabs = value;
            }
        }

        public bool IsBufferEmpty => Buffer.IsEmpty;
        public bool IsBufferFull => Position == Buffer.Length;
        public int RemainingCapacity => Buffer.Length - Position;
        
        public void Advance(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            Position += count;
        }

        public void Advance(int count, ref int charsWritten)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            Position += count;
            charsWritten += count;
        }

        public void Indent()
        {
            Tabs++;
        }

        public void UnIndent()
        {
            Tabs--;
        }

        public bool TryAddTabs()
        {
            for (var i = 0; i < Tabs; i++)
            {
                if (!TryAdd('\t')) return false;
            }

            return true;
        }

        public bool TryAddTabs(ref int charsWritten)
        {
            for (var i = 0; i < Tabs; i++)
            {
                if (!TryAdd('\t', ref charsWritten)) return false;
            }

            return true;
        }

        public BufferStringBuilder(Span<char> buffer, int position = 0, int tabs = 0)
        {
            Buffer = buffer;
            _tabs = 0;
            _position = 0;
            Tabs = tabs;
            Position = position;
        }

        public bool TryAdd(char c)
        {
            if (IsBufferFull) return false;

            Buffer[Position++] = c;
            return true;
        }

        public bool TryAddNewline()
        {
            for (var i = 0; i < Tabs; i++)
            {
                if (!TryAdd('\t')) return false;
            }

            if (!TryAdd('\n')) return false;
            return true;
        }

        public bool TryAddNewline(ref int charsWritten)
        {

            for (var i = 0; i < Tabs; i++)
            {
                if (!TryAdd('\t', ref charsWritten)) return false;
                
            }

            if (!TryAdd('\n', ref charsWritten)) return false;
            return true;
        }

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

        public bool TryAddWithSpace(string s)
        {
            if (!TryAdd(s)) return false;
            if (!TryAdd(' ')) return false;

            return true;
        }

        public bool TryAddWithSpace(string s, ref int charsWritten)
        {
            if (!TryAdd(s, ref charsWritten)) return false;
            if (!TryAdd(' ', ref charsWritten)) return false;

            return true;
        }

        public bool TryAdd<T>(T t) => TryAdd(t.ToString());

        public bool TryAdd<T>(T t, ref int charsWritten) => TryAddWithSpace(t.ToString(), ref charsWritten);
    }
}