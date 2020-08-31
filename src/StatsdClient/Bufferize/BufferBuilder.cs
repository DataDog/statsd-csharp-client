using System;
using System.Text;

namespace StatsdClient.Bufferize
{
    /// <summary>
    /// Append string values to a fixed size bytes buffer.
    /// </summary>
    internal class BufferBuilder
    {
        private static readonly Encoding _encoding = Encoding.UTF8;
        private readonly IBufferBuilderHandler _handler;
        private readonly byte[] _buffer;
        private readonly byte[] _separator;
        private readonly char[] _charsBuffers;

        public BufferBuilder(
            IBufferBuilderHandler handler,
            int bufferCapacity,
            string separator)
        {
            _buffer = new byte[bufferCapacity];
            _charsBuffers = new char[bufferCapacity];
            _handler = handler;
            _separator = _encoding.GetBytes(separator);
            if (_separator.Length >= _buffer.Length)
            {
                throw new ArgumentException("separator is greater or equal to the bufferCapacity");
            }
        }

        public int Length { get; private set; }

        public int Capacity => _buffer.Length;

        public static byte[] GetBytes(string message)
        {
            return _encoding.GetBytes(message);
        }

        public void Add(SerializedMetric serializedMetric)
        {
            var length = serializedMetric.CopyToChars(_charsBuffers);

            if (length < 0)
            {
                throw new InvalidOperationException($"The metric size exceeds the internal buffer capacity {_charsBuffers.Length}: {serializedMetric.ToString()}");
            }

            var byteCount = _encoding.GetByteCount(_charsBuffers, 0, length);

            if (byteCount > Capacity)
            {
                throw new InvalidOperationException($"The metric size exceeds the buffer capacity {Capacity}: {serializedMetric.ToString()}");
            }

            if (Length != 0)
            {
                byteCount += _separator.Length;
            }

            if (Length + byteCount > Capacity)
            {
                this.HandleBufferAndReset();
            }

            if (Length != 0)
            {
                Array.Copy(_separator, 0, _buffer, Length, _separator.Length);
                Length += _separator.Length;
            }

            // GetBytes requires the buffer to be big enough otherwise it throws, that is why we use GetByteCount.
            Length += _encoding.GetBytes(_charsBuffers, 0, length, _buffer, Length);
        }

        public void HandleBufferAndReset()
        {
            if (Length > 0)
            {
                _handler.Handle(_buffer, Length);
                Length = 0;
            }
        }
    }
}
