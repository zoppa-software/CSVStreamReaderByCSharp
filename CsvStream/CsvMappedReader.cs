using CsvStream.Spliter;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace CsvStream
{
    /// <summary>CSVファイルマップクラス。</summary>
    /// <remarks>
    /// CSVファイルをメモリマップして読み込むための機能。
    /// </remarks>
    public sealed class CsvMappedReader
        : IDisposable
    {
        #region "enum"

        /// <summary>ファイルエンコードタイプ。</summary>
        public enum EncodeType
        {
            /// <summary>S-JIS。</summary>
            CP932,

            /// <summary>UTF 8。</summary>
            UTF8,

            /// <summary>UTF 16。</summary>
            UTF16,
        }

        #endregion

        #region "fields"

        /// <summary>入力ストリーム。</summary>
        private Stream stream;

        /// <summary>読み込みエンコード。</summary>
        private readonly Encoding sourceEncoding;

        /// <summary>読込モード。</summary>
        private readonly IFieldSpliter fieldSpliter;

        /// <summary>文字列デコーダー。</summary>
        //private IStringDecoder stringDecoder;

        #endregion

        #region "constructor"

        /// <summary>指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        public CsvMappedReader(Stream stream)
        {
            this.stream = stream;
            this.sourceEncoding = this.SelectFileEncoding(true);
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvMappedReader(Stream stream,
                               bool detectEncodingfromByteOrderMarks)
        {
            this.stream = stream;
            this.sourceEncoding = this.SelectFileEncoding(detectEncodingfromByteOrderMarks);
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>文字エンコーディングを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        public CsvMappedReader(Stream stream, Encoding encoding)
        {
            this.stream = stream;
            this.sourceEncoding = encoding;
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        public CsvMappedReader(string path)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = this.SelectFileEncoding(true);
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvMappedReader(string path, bool detectEncodingfromByteOrderMarks)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = this.SelectFileEncoding(detectEncodingfromByteOrderMarks);
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>文字エンコーディングを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        public CsvMappedReader(string path, Encoding encoding)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = encoding;
            this.fieldSpliter = SplitFactory.CreateSpliter(CsvReadMode.UsedEscape);
        }

        /// <summary>指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(Stream stream, CsvReadMode readMode)
        {
            this.stream = stream;
            this.sourceEncoding = this.SelectFileEncoding(true);
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(Stream stream,
                               bool detectEncodingfromByteOrderMarks,
                               CsvReadMode readMode)
        {
            this.stream = stream;
            this.sourceEncoding = this.SelectFileEncoding(detectEncodingfromByteOrderMarks);
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        /// <summary>文字エンコーディングを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(Stream stream,
                               Encoding encoding,
                               CsvReadMode readMode)
        {
            this.stream = stream;
            this.sourceEncoding = encoding;
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        /// <summary>指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(string path, CsvReadMode readMode)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = this.SelectFileEncoding(true);
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(string path,
                               bool detectEncodingfromByteOrderMarks,
                               CsvReadMode readMode)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = this.SelectFileEncoding(detectEncodingfromByteOrderMarks);
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        /// <summary>文字エンコーディングを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvMappedReader(string path,
                               Encoding encoding,
                               CsvReadMode readMode)
        {
            this.stream = this.OpenFileStream(path);
            this.sourceEncoding = encoding;
            this.fieldSpliter = SplitFactory.CreateSpliter(readMode);
        }

        #endregion

        #region "methods"

        private Stream OpenFileStream(string path)
        {
            try {
                return new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception) {
                throw;
            }
        }

        private Encoding SelectFileEncoding(bool detectEncodingfromByteOrderMarks)
        {
            var boms = new byte[4];
            if (detectEncodingfromByteOrderMarks) {
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(boms, 0, boms.Length);

                if (boms[0] == 0xef &&
                    boms[1] == 0xbb &&
                    boms[2] == 0xbf) {
                    stream.Seek(3, SeekOrigin.Begin);
                    return Encoding.UTF8;
                }
                else if (boms[0] == 0xfe &&
                         boms[1] == 0xff) {
                    stream.Seek(2, SeekOrigin.Begin);
                    return Encoding.BigEndianUnicode;
                }
                else if (boms[0] == 0xff &&
                         boms[1] == 0xfe) {
                    stream.Seek(2, SeekOrigin.Begin);
                    return Encoding.Unicode;
                }
            }

            return Encoding.Default;
        }

        public void Dispose()
        {
            if (this.stream != null) {
                this.stream.Dispose();
                this.stream = null;
            }
        }

        public string ReadLine()
        {
            var answer = this.fieldSpliter.ReadPosition(this.stream, this.sourceEncoding);
            return this.sourceEncoding.GetString(answer.memory);
        }

        #endregion
    }
}
