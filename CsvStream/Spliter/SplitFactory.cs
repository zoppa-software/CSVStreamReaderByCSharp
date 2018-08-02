using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvStream.Spliter
{
    internal static class SplitFactory
    {
        public const int BUFFER_SIZE = 1024;

        protected class SplitCommon
        {
            protected byte[] readed;

            protected int readedLen;

            public SplitCommon()
            {
                this.readed = new byte[BUFFER_SIZE];
                this.readedLen = 0;
            }
        }

        private sealed class SplitByNotEscape
            : SplitCommon, IFieldSpliter
        {
            public CsvReadMode ReadMode => CsvReadMode.NotEscape;

            public IValueObject[] ReadGroup()
            {
                throw new NotImplementedException();
            }

            public FiledPosition ReadPosition(Stream stream, Encoding srcEncoding)
            {
                var len = stream.Read(this.readed, this.readedLen, this.readed.Length - this.readedLen);
                //srcEncoding.GetChars(this)

                //var bufferr = new byte[1024];
                //stream.Read(buffer, 0, buffer.length)

                return new FiledPosition();
            }
        }

        private sealed class SplitByUsedEscape
            : SplitCommon, IFieldSpliter
        {
            public CsvReadMode ReadMode => throw new NotImplementedException();

            public IValueObject[] ReadGroup()
            {
                throw new NotImplementedException();
            }

            public FiledPosition ReadPosition(Stream stream, Encoding srcEncoding)
            {
                //var bufferr = new byte[1024];
                //stream.Read(buffer, 0, buffer.length)

                return new FiledPosition();
            }
        }

        public static IFieldSpliter CreateSpliter(CsvReadMode mode)
        {
            switch (mode) 
            {
                case CsvReadMode.UsedEscape:
                    return new SplitByUsedEscape();

                case CsvReadMode.UsedEscapeNoTrim:
                    return new SplitByUsedEscape();

                case CsvReadMode.NotEscape:
                    return new SplitByUsedEscape();

                default:
                    throw new ArgumentException("読み込みモードの設定がおかしい");
            }
        }
    }
}
