using System;
using System.Collections.Generic;

namespace SplitFormts.Values
{
    /// <summary>キー値。</summary>
    internal sealed class ValueKey
        : IValueObject
    {
        #region "fields"

        /// <summary>ハッシュコード値。</summary>
        private int hashCode;

        /// <summary>読み込んだ文字列。</summary>
        private List<char> chars;

        /// <summary>範囲開始位置。</summary>
        private int st, ed;

        #endregion

        #region "properties"

        /// <summary>インスタンスの種類を取得する。</summary>
        /// <value>インスタンス種類値。</value>
        /// <returns>インスタンス種類値。</returns>
        public ValueObjectTypeEnum ValueObjectType => ValueObjectTypeEnum.TypeEmpty;

        /// <summary>整数値取得プロパティ。</summary>
        /// <value>整数型。</value>
        /// <returns>インスタンスが格納している整数値。</returns>
        /// <exception cref="ArgumentException">インスタンスが整数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        public int IntegerValue => 0;

        /// <summary>実数値取得プロパティ。</summary>
        /// <value>実数型。</value>
        /// <returns>インスタンスが格納している実数値。</returns>
        /// <exception cref="ArgumentException">インスタンスが実数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        public double DoubleValue => 0;

        /// <summary>文字列値取得プロパティ。</summary>
        /// <value>文字列型。</value>
        /// <returns>インスタンスが格納している文字列値。</returns>
        /// <remarks>インスタンスが格納してい文字列値を取得する。</remarks>
        public string StringValue => string.Empty;

        /// <summary>日付値取得プロパティ。</summary>
        /// <value>日付型。</value>
        /// <returns>インスタンスが格納している日付値。</returns>
        /// <exception cref="ArgumentException">インスタンスが日付型を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        public DateTime DateValue => DateTime.MinValue;

        /// <summary>Null許容整数値取得プロパティ。</summary>
        /// <value>Null許容整数型。</value>
        /// <returns>インスタンスが格納している整数値またはnull。</returns>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        public int? IntegerValueOrNull => null;

        /// <summary>Null許容実数値取得プロパティ。</summary>
        /// <value>Null許容実数型。</value>
        /// <returns>インスタンスが格納している実数値またはnull。</returns>
        /// <exception cref="ArgumentException">インスタンスが実数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        public double? DoubleValueOrNull => null;

        /// <summary>Null許容日付値取得プロパティ。</summary>
        /// <value>Null許容日付型。</value>
        /// <returns>インスタンスが格納している日付値、またはNull。</returns>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        public DateTime? DateValueOrNull => null;

        /// <summary>空白値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>空白値を格納していれば、真を返す。</returns>
        public bool IsEmpty => false;

        /// <summary>整数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納していれば、真を返す。</returns>
        public bool IsInteger => false;

        /// <summary>実数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>実数値を格納していれば、真を返す。</returns>
        public bool IsDouble => false;

        /// <summary>文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>文字列値を格納していれば、真を返す。</returns>
        public bool IsString => false;

        /// <summary>日付文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>日付文字列値を格納していれば、真を返す。</returns>
        public bool IsDate => false;

        /// <summary>数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納しているので真を返す。</returns>
        public bool IsNumber => false;

        /// <summary>文字長を取得する。</summary>
        public int Length
        {
            get {
                return this.ed - this.st;
            }
        }

        #endregion

        #region "indexer"

        /// <summary>インデクサ。</summary>
        /// <param name="index">添え字。</param>
        /// <returns>文字。</returns>
        public char this[int index]
        {
            get {
                return this.chars[this.st + index];
            }
        }

        #endregion

        #region "methods"

        /// <summary>キーに値を設定する。</summary>
        /// <param name="chars">文字列参照。</param>
        /// <param name="st">範囲開始位置。</param>
        /// <param name="ed">範囲終了位置。</param>
        public void SetKey(List<char> chars, int st, int ed)
        {
            // 参照を保持する
            this.chars = chars;
            this.st = st;
            this.ed = ed;

            // ハッシュコード値を保持する
            var hash = ValueObject.FNV_OFFSET_BASIS_32;
            for (int i = st; i < ed; ++i) {
                var v = (uint)this.chars[i];
                hash = (ValueObject.FNV_PRIME_32 * hash) ^ (v & 0xff);
                hash = (ValueObject.FNV_PRIME_32 * hash) ^ ((v >> 8) & 0xff);
            }
            this.hashCode = (int)hash;
        }

        /// <summary>ハッシュコード値を取得する。</summary>
        /// <returns>ハッシュコード値。</returns>
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        /// <summary>インスタンスの等価比較を行う。</summary>
        /// <param name="obj">比較対象。</param>
        /// <returns>比較結果。</returns>
        public override bool Equals(object obj)
        {
            if (obj is ValueObject) {
                var other = (ValueObject)obj;
                if (this.Length == other.Length) {
                    for (int i = this.st, j = 0; i < this.ed; ++i, ++j) {
                        if (this.chars[i] != other[j]) { return false; }
                    }
                    return true;
                }
                else {
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}
