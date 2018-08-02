using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SplitFormts.Values;

namespace SplitFormts.Csv
{
    /// <summary>CSVファイル読取ストリームクラス。</summary>
    /// <remarks>
    /// 読取ストリームに対して、CSV解析機能、For each 機能を追加したクラス。
    /// </remarks>
    public sealed class CsvStreamReader
        : IDisposable
    {
        #region "const"

        /// <summary>デフォルトバッファサイズ。</summary>
        public const int DEFAULT_BUFF_SIZE = 1024;

        #endregion

        #region "inner class"

        //private sealed class EncodingWrapper
        //    : 
        //{

        //}

        #endregion

        #region "fields"

        /// <summary>内部ストリーム。</summary>
        private StreamReader innerStream;

        /// <summary>カンマ区切り機能。</summary>
        private CsvSpliter spliter;

        #endregion

        #region "properteis"

        /// <summary>読込モードを取得する。</summary>
        /// <value>真偽値。</value>
        public CsvReadMode ReadMode
        {
            get {
                return this.spliter.Mode;
            }
        }

        /// <summary>ストリームが終端ならば真を返す。</summary>
        public bool EndOfStream
        {
            get {
                return (this.innerStream.EndOfStream && this.spliter.EndOfBuffer);
            }
        }

        /// <summary>列挙操作クラスを取得する。</summary>
        /// <returns>列挙操作クラス。</returns>
        public IEnumerable<IList<IValueObject>> Enumerator
        {
            get {
                while (!this.EndOfStream) {
                    yield return this.ReadGroup();
                }
            }
        }

        #endregion

        #region "constructor"

        /// <summary>指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        public CsvStreamReader(Stream stream)
        {
            this.innerStream = new StreamReader(stream);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvStreamReader(Stream stream, bool detectEncodingfromByteOrderMarks)
        {
            this.innerStream = new StreamReader(stream, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディングを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        public CsvStreamReader(Stream stream, Encoding encoding)
        {
            this.innerStream = new StreamReader(stream, encoding);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプションを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvStreamReader(Stream stream, Encoding encoding, bool detectEncodingfromByteOrderMarks)
        {
            this.innerStream = new StreamReader(stream, encoding, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプション、バッファサイズを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        public CsvStreamReader(Stream stream, Encoding encoding, bool detectEncodingfromByteOrderMarks, int bufferSize)
        {
            this.innerStream = new StreamReader(stream, encoding, detectEncodingfromByteOrderMarks, bufferSize);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, bufferSize);
        }

        /// <summary>指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        public CsvStreamReader(string path)
        {
            this.innerStream = new StreamReader(path);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvStreamReader(string path, bool detectEncodingfromByteOrderMarks)
        {
            this.innerStream = new StreamReader(path, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディングを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        public CsvStreamReader(string path, Encoding encoding)
        {
            this.innerStream = new StreamReader(path, encoding);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプションを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        public CsvStreamReader(string path, Encoding encoding, bool detectEncodingfromByteOrderMarks)
        {
            this.innerStream = new StreamReader(path, encoding, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプション、バッファサイズを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        public CsvStreamReader(string path, Encoding encoding, bool detectEncodingfromByteOrderMarks, int bufferSize)
        {
            this.innerStream = new StreamReader(path, encoding, detectEncodingfromByteOrderMarks, bufferSize);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, CsvReadMode.UseEscape, bufferSize);
        }

        /// <summary>指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(Stream stream, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(stream);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(Stream stream, bool detectEncodingfromByteOrderMarks, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(stream, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディングを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(Stream stream, Encoding encoding, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(stream, encoding);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプションを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(Stream stream, Encoding encoding, bool detectEncodingfromByteOrderMarks, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(stream, encoding, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプション、バッファサイズを設定して、指定したストリーム用の新しいインスタンスを初期化する。</summary>
        /// <param name="stream">入力元ストリーム。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(Stream stream, Encoding encoding, bool detectEncodingfromByteOrderMarks, int bufferSize, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(stream, encoding, detectEncodingfromByteOrderMarks, bufferSize);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, bufferSize);
        }

        /// <summary>指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(string path, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(path);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>バイト順マーク検出オプションを使用して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(string path, bool detectEncodingfromByteOrderMarks, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(path, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディングを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(string path, Encoding encoding, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(path, encoding);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプションを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(string path, Encoding encoding, bool detectEncodingfromByteOrderMarks, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(path, encoding, detectEncodingfromByteOrderMarks);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        /// <summary>文字エンコーディング、バイト順マーク検出オプション、バッファサイズを設定して、指定したファイル用の新しいインスタンスを初期化する。</summary>
        /// <param name="path">ファイルパス。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <param name="detectEncodingfromByteOrderMarks">バイト順マーク検出オプション。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        /// <param name="readMode">読込モード。</param>
        public CsvStreamReader(string path, Encoding encoding, bool detectEncodingfromByteOrderMarks, int bufferSize, CsvReadMode readMode)
        {
            this.innerStream = new StreamReader(path, encoding, detectEncodingfromByteOrderMarks, bufferSize);
            this.spliter = CsvSpliter.CreateSpliter(this.innerStream, readMode, DEFAULT_BUFF_SIZE);
        }

        #endregion

        #region "method"

        /// <summary>文字列から CSVストリームを生成する。</summary>
        /// <param name="input">解析する文字列。</param>
        /// <returns>CSVストリーム。</returns>
        public static CsvStreamReader FromString(string input)
        {
            var mem = new MemoryStream(Encoding.Unicode.GetBytes(input));
            return new CsvStreamReader(mem, Encoding.Unicode);
        }

        //---------------------------------------------------------------------
        // TextReader用メソッド
        //---------------------------------------------------------------------
        /// <summary>リソースを解放する。</summary>
        public void Dispose()
        {
            this.innerStream?.Dispose();
            this.innerStream = null;
        }

        /// <summary>テキスト リーダーから次の文字を読み取り、1 文字分だけ文字位置を進めます。</summary>
        /// <returns>テキスト リーダーからの次の文字。それ以上読み取り可能な文字がない場合は -1。</returns>
        public int Read()
        {
            return (this.innerStream?.Read() ?? -1);
        }

        /// <summary>指定した最大文字数を現在のリーダーから読み取り、バッファーの指定したインデックス位置にそのデータを書き込みます。</summary>
        /// <param name="buffer">このメソッドが戻るとき、指定した文字配列が含まれています。</param>
        /// <param name="index">書き込みを開始する buffer 内の位置。</param>
        /// <param name="count">読み取り対象の最大文字数。</param>
        /// <returns>読み取られた文字数。</returns>
        public int Read(char[] buffer, int index, int count)
        {
            return (this.innerStream?.Read(buffer, index, count) ?? 0);
        }

        /// <summary>指定した最大文字数を現在のテキスト リーダーから非同期に読み取り、バッファーの指定したインデックス位置にそのデータを書き込みます。</summary>
        /// <param name="buffer">このメソッドが戻るとき、指定した文字配列が含まれています。</param>
        /// <param name="index">書き込みを開始する buffer 内の位置。</param>
        /// <param name="count">読み取り対象の最大文字数。</param>
        /// <returns>読み取られた文字数。</returns>
        public Task<int> ReadAsync(char[] buffer, int index, int count)
        {
            return this.innerStream.ReadAsync(buffer, index, count);
        }

        /// <summary>ストリームより現在地から改行までの文字列を取得する。</summary>
        /// <returns>一行文字列。</returns>
        /// <remarks>
        /// ストリームが指しているポイントより改行までの文字列を取得する。
        /// </remarks>
        public string ReadLine()
        {
            var res = new StringBuilder();
            this.spliter.ReadSplitLine(res);
            return res.ToString();
        }

        /// <summary>非同期にてストリームより現在地から改行までの文字列を取得する。</summary>
        /// <returns>一行文字列。</returns>
        /// <remarks>
        /// ストリームが指しているポイントより改行までの文字列を取得する。
        /// </remarks>
        public Task<string> ReadLineAsync()
        {
            return Task.Run(() => {
                var res = new StringBuilder();
                this.spliter.ReadSplitLine(res);
                return res.ToString();
            });
        }

        /// <summary>CSVファイルを一行分取り込み、値変換した ValueObject 配列を取得する。</summary>
        /// <returns>ValueObject 配列。</returns>
        public IList<IValueObject> ReadGroup()
        {
            return this.spliter.ReadGroup();
        }

        /// <summary>CSVファイルを一行分取り込み、値変換した ValueObject 配列を取得する。</summary>
        /// <returns>ValueObject 配列。</returns>
        public Task<IList<IValueObject>> ReadGroupAsync()
        {
            return Task.Run(() => {
                return this.spliter.ReadGroup();
            });
        }

        #endregion
    }
}
