using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SplitFormts.Values;

namespace SplitFormts.Csv
{
    /// <summary>カンマ区切り機能。</summary>
    public abstract class CsvSpliter
    {
        #region "inner class"

        private struct Pointer
        {
            public int srcStart;

            public int srcEnd;

            public int dstStart;

            public int dstEnd;

            public void AjustLine(List<char> buffer)
            {
                if (this.srcStart < this.dstStart) {
                    for (int d = this.dstStart, s = this.srcStart; d < this.dstEnd; ++d, ++s) {
                        buffer[s] = buffer[d];
                    }
                }
                if (this.dstEnd - this.dstStart < this.srcEnd - this.srcStart) {
                    for (int j = 0; j < (this.srcEnd - this.srcStart) - (this.dstEnd - this.dstStart); ++j) {
                        buffer.RemoveAt(buffer.Count - 1);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class SplitByUsedEscape
            : CsvSpliter
        {
            /// <summary>読込モードを取得する。</summary>
            /// <value>真偽値。</value>
            public override CsvReadMode Mode => CsvReadMode.UseEscape;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="innerStream"></param>
            /// <param name="bufferSize"></param>
            public SplitByUsedEscape(StreamReader innerStream, int bufferSize)
                : base(innerStream, bufferSize)
            { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="resbuf"></param>
            /// <returns></returns>
            public override List<int> ReadSplitLine(StringBuilder resbuf = null)
            {
                bool esc = false;
                bool seqst = true;
                char prev = '\0';
                var pos = new Pointer();

                this.oneLineBuffer.Clear();
                var res = new List<int>();
                res.Add(0);

                while (true) {
                    if (this.charIndex <= 0) {
                        this.charIndex = this.innerStream.ReadBlock(this.charBuffer, 0, this.charBuffer.Length);
                        if (this.charIndex <= 0) { break; }
                    }

                    for (int i = 0; i < this.charIndex; ++i) {
                        char c = this.charBuffer[i];

                        switch (c) {
                            case ',':
                                resbuf?.Append(c);
                                if (!esc) {
                                    pos.AjustLine(this.oneLineBuffer);
                                    res.Add(this.oneLineBuffer.Count);
                                    pos.srcStart = this.oneLineBuffer.Count;
                                    pos.dstStart = pos.srcStart;
                                    seqst = true;
                                }
                                else {
                                    seqst = false;
                                }
                                break;

                            case '"':
                                resbuf?.Append(c);
                                seqst = false;
                                if (esc) {
                                    esc = false;
                                }
                                else {
                                    if (pos.srcStart == pos.srcEnd) {
                                        esc = true;
                                    }
                                    else if (prev == '"') {
                                        this.oneLineBuffer.Add('"');
                                        esc = true;
                                    }
                                }
                                pos.dstEnd = this.oneLineBuffer.Count;
                                break;

                            case '\n':
                                seqst = false;
                                if (prev == '\r') {
                                    this.oneLineBuffer.RemoveAt(this.oneLineBuffer.Count - 1);
                                    pos.srcEnd--;
                                    if (resbuf != null) {
                                        resbuf.Remove(resbuf.Length - 1, 1);
                                    }
                                }
                                if (esc) {
                                    if (prev == '\r') {
                                        this.oneLineBuffer.Add(prev);
                                        resbuf?.Append(prev);
                                    }
                                    this.oneLineBuffer.Add(c);
                                    resbuf?.Append(c);
                                }
                                else {
                                    var clen = this.charIndex - (i + 1);
                                    Array.Copy(this.charBuffer, i + 1, this.charBuffer, 0, clen);
                                    this.charIndex = clen;
                                    pos.AjustLine(this.oneLineBuffer);
                                    res.Add(this.oneLineBuffer.Count);
                                    return res;
                                }
                                pos.dstEnd = this.oneLineBuffer.Count;
                                break;

                            default:
                                resbuf?.Append(c);
                                this.oneLineBuffer.Add(c);
                                if (char.IsWhiteSpace(c)) {
                                    if (seqst) { pos.dstStart++; }
                                }
                                else {
                                    seqst = false;
                                    pos.dstEnd = this.oneLineBuffer.Count;
                                }
                                pos.srcEnd = this.oneLineBuffer.Count;
                                break;
                        }

                        prev = c;
                    }

                    this.charIndex = 0;

                    // 改行エスケープがあれば次を取り込む
                    if (this.innerStream.Peek() == -1) {
                        break;
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class SplitByUsedEscapeNoTrim
            : CsvSpliter
        {
            /// <summary>読込モードを取得する。</summary>
            /// <value>真偽値。</value>
            public override CsvReadMode Mode => CsvReadMode.UseEscapeNoTrim;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="innerStream"></param>
            /// <param name="bufferSize"></param>
            public SplitByUsedEscapeNoTrim(StreamReader innerStream, int bufferSize)
                : base(innerStream, bufferSize)
            { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="resbuf"></param>
            /// <returns></returns>
            public override List<int> ReadSplitLine(StringBuilder resbuf)
            {
                bool esc = false;
                char prev = '\0';
                var pos = new Pointer();

                this.oneLineBuffer.Clear();
                var res = new List<int>();
                res.Add(0);

                while (true) {
                    if (this.charIndex <= 0) {
                        this.charIndex = this.innerStream.ReadBlock(this.charBuffer, 0, this.charBuffer.Length);
                        if (this.charIndex <= 0) { break; }
                    }

                    for (int i = 0; i < this.charIndex; ++i) {
                        char c = this.charBuffer[i];

                        switch (c) {
                            case ',':
                                resbuf?.Append(c);
                                if (!esc) {
                                    res.Add(this.oneLineBuffer.Count);
                                    pos.srcStart = this.oneLineBuffer.Count;
                                }
                                break;

                            case '"':
                                resbuf?.Append(c);
                                if (esc) {
                                    esc = false;
                                }
                                else {
                                    if (pos.srcStart == pos.srcEnd) {
                                        esc = true;
                                    }
                                    else if (prev == '"') {
                                        this.oneLineBuffer.Add('"');
                                        esc = true;
                                    }
                                }
                                break;

                            case '\n':
                                if (prev == '\r') {
                                    this.oneLineBuffer.RemoveAt(this.oneLineBuffer.Count - 1);
                                    pos.srcEnd--;
                                    if (resbuf != null) {
                                        resbuf.Remove(resbuf.Length - 1, 1);
                                    }
                                }
                                if (esc) {
                                    if (prev == '\r') {
                                        this.oneLineBuffer.Add(prev);
                                        resbuf?.Append(prev);
                                    }
                                    this.oneLineBuffer.Add(c);
                                    resbuf?.Append(c);
                                }
                                else {
                                    var clen = this.charIndex - (i + 1);
                                    Array.Copy(this.charBuffer, i + 1, this.charBuffer, 0, clen);
                                    this.charIndex = clen;
                                    res.Add(this.oneLineBuffer.Count);
                                    return res;
                                }
                                break;

                            default:
                                resbuf?.Append(c);
                                this.oneLineBuffer.Add(c);
                                pos.srcEnd = this.oneLineBuffer.Count;
                                break;
                        }

                        prev = c;
                    }

                    this.charIndex = 0;

                    // 改行エスケープがあれば次を取り込む
                    if (this.innerStream.Peek() == -1) {
                        break;
                    }
                }
                return res;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class SplitByNotEscape
            : CsvSpliter
        {
            /// <summary>読込モードを取得する。</summary>
            /// <value>真偽値。</value>
            public override CsvReadMode Mode => CsvReadMode.NotEscape;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="innerStream"></param>
            /// <param name="bufferSize"></param>
            public SplitByNotEscape(StreamReader innerStream, int bufferSize)
                : base(innerStream, bufferSize)
            { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="resbuf"></param>
            public override List<int> ReadSplitLine(StringBuilder resbuf)
            {
                char prev = '\0';
                var pos = new Pointer();

                this.oneLineBuffer.Clear();
                var res = new List<int>();
                res.Add(0);

                while (true) {
                    if (this.charIndex <= 0) {
                        this.charIndex = this.innerStream.ReadBlock(this.charBuffer, 0, this.charBuffer.Length);
                        if (this.charIndex <= 0) { break; }
                    }

                    for (int i = 0; i < this.charIndex; ++i) {
                        char c = this.charBuffer[i];

                        switch (c) {
                            case ',':
                                resbuf?.Append(c);
                                res.Add(this.oneLineBuffer.Count);
                                pos.srcStart = this.oneLineBuffer.Count;
                                break;

                            case '\n':
                                if (prev == '\r') {
                                    this.oneLineBuffer.RemoveAt(this.oneLineBuffer.Count - 1);
                                    pos.srcEnd--;
                                    if (resbuf != null) {
                                        resbuf.Remove(resbuf.Length - 1, 1);
                                    }
                                }
                                var clen = this.charIndex - (i + 1);
                                Array.Copy(this.charBuffer, i + 1, this.charBuffer, 0, clen);
                                this.charIndex = clen;
                                res.Add(this.oneLineBuffer.Count);
                                return res;

                            default:
                                resbuf?.Append(c);
                                this.oneLineBuffer.Add(c);
                                pos.srcEnd = this.oneLineBuffer.Count;
                                break;
                        }

                        prev = c;
                    }

                    this.charIndex = 0;

                    // 改行エスケープがあれば次を取り込む
                    if (this.innerStream.Peek() == -1) {
                        break;
                    }
                }
                return res;
            }
        }

        #endregion

        #region "fields"

        /// <summary>内部ストリーム。</summary>
        private readonly StreamReader innerStream;

        /// <summary>文字バッファ。</summary>
        private readonly char[] charBuffer;

        /// <summary>文字インデックス。</summary>
        private int charIndex;

        /// <summary>入力された一行分の文字列を記憶する。</summary>
        private readonly List<char> oneLineBuffer;

        /// <summary>値テンポラリ。</summary>
        private ValueKey temporary;

        /// <summary>値キャッシュ。</summary>
        private ValueCache cache;

        #endregion

        #region "properties"

        /// <summary>読込モードを取得する。</summary>
        /// <value>真偽値。</value>
        public abstract CsvReadMode Mode
        {
            get;
        }

        /// <summary>バッファが終了していたら真。</summary>
        public bool EndOfBuffer
        {
            get {
                return (this.charIndex <= 0);
            }
        }

        #endregion

        protected CsvSpliter(StreamReader innerStream, int bufferSize)
        {
            this.innerStream = innerStream;

            this.charBuffer = new char[bufferSize];
            this.charIndex = 0;

            this.oneLineBuffer = new List<char>();

            this.temporary = new ValueKey();
            this.cache = new ValueCache();
        }

        #region "method"

        /// <summary>モードに合わせた変換器を取得する。</summary>
        /// <param name="mode">読み取りモード。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        /// <returns>カンマ区切り機能。</returns>
        public static CsvSpliter CreateSpliter(StreamReader innerStream, CsvReadMode mode, int bufferSize)
        {
            switch (mode) {
                case CsvReadMode.UseEscape:
                    return new SplitByUsedEscape(innerStream, bufferSize);

                case CsvReadMode.UseEscapeNoTrim:
                    return new SplitByUsedEscapeNoTrim(innerStream, bufferSize);

                case CsvReadMode.NotEscape:
                    return new SplitByNotEscape(innerStream, bufferSize);

                default:
                    throw new ArgumentException("読み込みモードの設定がおかしい");
            }
        }

        /// <summary>" でエスケープされた範囲を考慮して文字列をバッファに取り込む。</summary>
        /// <param name="resbuf">戻り値バッファ。</param>
        /// <returns>区切り位置リスト。</returns>
        public abstract List<int> ReadSplitLine(StringBuilder resbuf);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<IValueObject> ReadGroup()
        {
            var splits = this.ReadSplitLine(null);

            var res = new List<IValueObject>();
            for (int i = 0; i < splits.Count - 1; ++i) {
                this.temporary.SetKey(this.oneLineBuffer, splits[i], splits[i + 1]);
                res.Add(this.cache.GetCache(this.temporary, this.oneLineBuffer, splits[i], splits[i + 1]));
            }
            return res.ToArray();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public IValueObject[] ReadGroupNonVonvert()
        //{
        //    var splits = this.ReadSplitLine(null);

        //    var res = new List<IValueObject>();
        //    for (int i = 0; i < splits.Count - 1; ++i) {
        //        this.temporary.SetValue(this.oneLineBuffer, splits[i], splits[i + 1]);
        //        if (this.cache.ContainsKey(this.temporary)) {
        //            res.Add(this.cache[this.temporary]);
        //        }
        //        else {
        //            res.Add(this.temporary.GetValue());
        //        }
        //    }
        //    return res.ToArray();
        //}

        #endregion
    }
}
