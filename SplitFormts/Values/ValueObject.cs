using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SplitFormts.Values
{
    /// <summary>変換可能な値を表す。</summary>
    [Serializable(), DebuggerDisplay("{StringValue}")]
    public sealed class ValueObject
        : IValueObject
    {
        #region "const"

        /// <summary>fnv-1 オフセット値。</summary>
        public const uint FNV_OFFSET_BASIS_32 = 2166136261U;

        /// <summary>fnv-1 プライム値。</summary>
        public const uint FNV_PRIME_32 = 16777619U;

        #endregion

        #region "struct"

        /// <summary>E形式の読み込み結果。</summary>
        private struct ExpoResult
        {
            /// <summary>E形式の書式ならば真。</summary>
            public bool enable;

            /// <summary>指数部の桁数。</summary>
            public int value;
        }

        #endregion

        #region "fields"

        /// <summary>値の文字列。</summary>
        private readonly string chars;

        /// <summary>評価後の値。</summary>
        private IValueObject value;

        #endregion

        #region "properties"

        /// <summary>評価後のインスタンスを取得する。</summary>
        private IValueObject Instance
        {
            get {
                if (this.value == null) {
                    this.value = this.GetValue(ValueString.Instance);
                }
                return this.value;
            }
        }

        /// <summary>インスタンスの種類を取得する。</summary>
        /// <value>インスタンス種類値。</value>
        /// <returns>インスタンス種類値。</returns>
        public ValueObjectTypeEnum ValueObjectType => this.Instance.ValueObjectType;

        /// <summary>整数値取得プロパティ。</summary>
        /// <value>整数型。</value>
        /// <returns>インスタンスが格納している整数値。</returns>
        /// <exception cref="ArgumentException">インスタンスが整数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        public int IntegerValue => this.Instance.IntegerValue;

        /// <summary>実数値取得プロパティ。</summary>
        /// <value>実数型。</value>
        /// <returns>インスタンスが格納している実数値。</returns>
        /// <exception cref="ArgumentException">インスタンスが実数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        public double DoubleValue => this.Instance.DoubleValue;

        /// <summary>文字列値取得プロパティ。</summary>
        /// <value>文字列型。</value>
        /// <returns>インスタンスが格納している文字列値。</returns>
        /// <remarks>インスタンスが格納してい文字列値を取得する。</remarks>
        public string StringValue => this.ToString();

        /// <summary>日付値取得プロパティ。</summary>
        /// <value>日付型。</value>
        /// <returns>インスタンスが格納している日付値。</returns>
        /// <exception cref="ArgumentException">インスタンスが日付型を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        public DateTime DateValue => this.Instance.DateValue;

        /// <summary>Null許容整数値取得プロパティ。</summary>
        /// <value>Null許容整数型。</value>
        /// <returns>インスタンスが格納している整数値またはnull。</returns>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        public int? IntegerValueOrNull => this.Instance.IntegerValueOrNull;

        /// <summary>Null許容実数値取得プロパティ。</summary>
        /// <value>Null許容実数型。</value>
        /// <returns>インスタンスが格納している実数値またはnull。</returns>
        /// <exception cref="ArgumentException">インスタンスが実数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        public double? DoubleValueOrNull => this.Instance.IntegerValueOrNull;

        /// <summary>Null許容日付値取得プロパティ。</summary>
        /// <value>Null許容日付型。</value>
        /// <returns>インスタンスが格納している日付値、またはNull。</returns>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        public DateTime? DateValueOrNull
        {
            get {
                if (this.value == null || ReferenceEquals(this.value, ValueString.Instance)) {
                    this.value = this.ConvartDate();
                }
                return this.value.DateValueOrNull;
            }
        }

        /// <summary>空白値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>空白値を格納していれば、真を返す。</returns>
        public bool IsEmpty => this.Instance.IsEmpty;

        /// <summary>整数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納していれば、真を返す。</returns>
        public bool IsInteger => this.Instance.IsInteger;

        /// <summary>実数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>実数値を格納していれば、真を返す。</returns>
        public bool IsDouble => this.Instance.IsDouble;

        /// <summary>文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>文字列値を格納していれば、真を返す。</returns>
        public bool IsString => true;

        /// <summary>日付文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>日付文字列値を格納していれば、真を返す。</returns>
        public bool IsDate
        {
            get {
                if  (this.value == null || ReferenceEquals(this.value, ValueString.Instance)) {
                    this.value = this.ConvartDate();
                }
                return this.value.IsDate;
            }
        }

        /// <summary>数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納しているので真を返す。</returns>
        public bool IsNumber => this.Instance.IsNumber;

        /// <summary>文字長を取得する。</summary>
        public int Length
        {
            get {
                return this.chars.Length;
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
                return this.chars[index];
            }
        }

        #endregion

        #region "constructor"

        /// <summary>コンストラクタ。</summary>
        /// <param name="chars">与えられた値文字列。</param>
        public ValueObject(string chars)
        {
            if (chars != string.Empty) {
                this.chars = chars;
                this.value = null;
            }
            else {
                this.chars = string.Empty;
                this.value = ValueEmpty.Empty;
            }
        }

        /// <summary>コンストラクタ。</summary>
        /// <param name="chars">読み込んだ文字列。</param>
        /// <param name="st">開始位置。</param>
        /// <param name="ed">終了位置。</param>
        internal ValueObject(List<char> chars, int st, int ed)
        {
            if (ed > st) {
                var temp = new char[ed - st];
                chars.CopyTo(st, temp, 0, ed - st);
                this.chars = new string(temp);
                this.value = null;
            }
            else {
                this.chars = string.Empty;
                this.value = ValueEmpty.Empty;
            }
        }

        #endregion

        #region "methods"

        /// <summary>評価した値のインスタンスを取得する。</summary>
        /// <param name="strobj">文字列値を表すインスタンス。</param>
        /// <returns>値インスタンス。</returns>
        private IValueObject GetValue(IValueObject strobj)
        {
            unsafe {
                fixed (char * cptr = this.chars) {
                    if (*cptr == '+') {
                        return this.ConvertNumber(cptr + 1, cptr + this.chars.Length, false, strobj);
                    }
                    else if (*cptr == '-') {
                        return this.ConvertNumber(cptr + 1, cptr + this.chars.Length, true, strobj);
                    }
                    else {
                        return this.ConvertNumber(cptr, cptr + this.chars.Length, false, strobj);
                    }
                }
            }
        }

        /// <summary>値文字列を数値変換する。</summary>
        /// <param name="cptr">文字列ポインタ。</param>
        /// <param name="sentinel">終端ポインタ。</param>
        /// <param name="numberSign">負数ならば真。</param>
        /// <param name="strobj">文字値インスタンス。</param>
        /// <returns>値インスタンス。</returns>
        private unsafe IValueObject ConvertNumber(char* cptr,
                                                  char* sentinel,
                                                  bool numberSign,
                                                  IValueObject strobj)
        {
            ulong v = 0;
            int digit = -1;
            bool use_exp = false;
            int exp_v = -1;
            bool ld_zero = false;
            ExpoResult eres;

            // 一文字の数値もなければ None値を返す
            if (cptr == sentinel) {
                return ValueEmpty.Empty;
            }

            // 仮数部を取得する
            for (; cptr < sentinel; ++cptr) {
                // 1. 数値の計算をする
                //    1-1. 数値の有効範囲を超えるならばエラー
                // 2. 小数点位置を取得する
                // 3. 指数部（e）を取得する
                if (*cptr >= '0' && *cptr <= '9') {
                    if (v < ulong.MaxValue / 10) {      // 1
                        v = v * 10 + (ulong)(*cptr - '0');
                        if (digit >= 0) {
                            digit++;
                        }
                        else {
                            ld_zero = true;
                        }
                    }
                    else {                              // 2-1
                        return strobj;
                    }
                }
                else if (*cptr == '.') {                // 3
                    if (ld_zero && digit < 0) {
                        digit = 0;
                    }
                    else {
                        return strobj;
                    }
                }
                else if (*cptr == 'e' || *cptr == 'E') {
                    use_exp = true;                     // 4
                    break;
                }
                else {
                    return strobj;
                }
            }

            // 仮数部を取得する
            if (cptr < sentinel - 1 && use_exp) {
                eres = this.CalcExponentConvert(cptr + 1, sentinel);
                if (eres.enable) {
                    exp_v = eres.value - (digit > 0 ? digit : 0);
                    if (exp_v < -308 || exp_v > 308) {
                        return strobj;
                    }
                }
            }
            else {
                exp_v = - (digit > 0 ? digit : 0);
            }

            if (digit < 0 && !use_exp) {
                // 整数値を取得する
                //
                // 1. 負の整数変換
                // 2. 正の整数変換
                if (numberSign) {
                    if (v <= (uint)int.MaxValue + 1) {       // 1
                        return new ValueInteger(-(int)v);
                    }
                    else {
                        return strobj;
                    }
                }
                else {
                    if (v <= long.MaxValue) {               //2
                        return new ValueInteger((int)v);
                    }
                    else {
                        return strobj;
                    }
                }
            }
            else {
                // 実数値を取得する
                //
                // 1. 負の指数なら除算
                // 2. 正の指数なら積算
                // 3. 0の指数なら使用しない
                // 4. 値を保持
                double dv = 0;
                if (exp_v < 0) {
                    double abs_e = 1;                   // 1
                    for (int i = 0; i < Math.Abs(exp_v); ++i) {
                        abs_e *= 10;
                    }
                    dv = (double)v / abs_e;
                }
                else if (exp_v > 0) {
                    double abs_e = 1;                   // 2
                    for (int i = 0; i < Math.Abs(exp_v); ++i) {
                        abs_e *= 10;
                    }
                    dv = (double)v * abs_e;
                }
                else {
                    dv = (double)v;                     // 3
                }
                if (dv <= double.MaxValue) {            // 4
                    return new ValueDouble(numberSign ? (double)-dv : (double)dv);
                }
                else {
                    return strobj;
                }
            }
        }

        /// <summary>実数の指数部を取得する。</summary>
        /// <param name="cptr">文字列ポインタ。</param>
        /// <param name="sentinel">終端ポインタ。</param>
        /// <returns>指数部の情報。</returns>
        private unsafe ExpoResult CalcExponentConvert(char* cptr, char* sentinel)
        {
            var res = new ExpoResult();
            res.enable = false;
            res.value = 0;
            bool sign = false;

            // 符号を取得する
            if (cptr < sentinel - 1) {
                if (*cptr == '+') {
                    sign = false;
                    cptr++;
                }
                else if (*cptr == '-') {
                    sign = true;
                    cptr++;
                }
            }

            // 指数部を計算する
            for (; cptr < sentinel; ++cptr) {
                if (*cptr >= '0' && *cptr <= '9') {
                    res.value = res.value * 10 + (*cptr - '0');
                    if (res.value >= 308) {
                        return res;
                    }
                }
                else {
                    return res;
                }
            }

            // 符号を設定
            if (sign) {
                res.value = -res.value;
            }
            res.enable = true;
            return res;
        }


        /// <summary>値文字列を日付型を含めて変換する。</summary>
        /// <returns>値インスタンス。</returns>
        private IValueObject ConvartDate()
        {
            DateTime d;
            var s = this.StringValue;
            if (DateTime.TryParse(s, out d)) {
                return new ValueDateTime(d);
            }
            else {
                return this.GetValue(ValueString.InstanceOfDayChecked);
            }
        }

        /// <summary>ハッシュコード値を取得する。</summary>
        /// <returns>ハッシュコード値。</returns>
        public override int GetHashCode()
        {
            unsafe {
                fixed (char* cptr = chars) {
                    byte* bptr = (byte*)cptr;
                    var hash = ValueObject.FNV_OFFSET_BASIS_32;
                    for (; bptr < cptr + this.chars.Length; ++bptr) {
                        hash = (ValueObject.FNV_PRIME_32 * hash) ^ *bptr;
                    }
                    return (int)hash;
                }
            }
        }

        /// <summary>インスタンスの等価比較を行う。</summary>
        /// <param name="obj">比較対象。</param>
        /// <returns>比較結果。</returns>
        public override bool Equals(object obj)
        {
            var other = obj as ValueObject;
            if (other != null) {
                if (this.chars.Length == other.chars.Length) {
                    for (int i = 0; i < this.chars.Length; ++i) {
                        if (this.chars[i] != other.chars[i]) { return false; }
                    }
                    return true;
                }
                else {
                    return false;
                }
            }
     
            var okey = obj as ValueKey;
            if (okey != null) {
                if (this.chars.Length == okey.Length) {
                    for (int i = 0; i < this.chars.Length; ++i) {
                        if (this.chars[i] != okey[i]) { return false; }
                    }
                    return true;
                }
                else {
                    return false;
                }
            }

            return false;
        }

        /// <summary>インスタンスの文字列表現を取得する。</summary>
        /// <returns>文字列。</returns>
        public override string ToString()
        {
            return this.chars;
        }

        #endregion
    }
}
