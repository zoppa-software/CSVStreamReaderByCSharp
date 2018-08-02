using System;

namespace SplitFormts.Values
{
    /// <summary>空値を表す。</summary>
    internal sealed class ValueEmpty
        : IValueObject
    {
        #region "fields"

        /// <summary>静的な空値。</summary>
        private static ValueEmpty empty;  

        #endregion

        #region "properties"

        /// <summary>空値を取得する。</summary>
        public static IValueObject Empty
        {
            get {
                return empty;
            }
        }

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
        /// <returns>インスタンスが格納している整数値。</returns>
        /// <exception cref="ArgumentException">インスタンスが整数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        public int? IntegerValueOrNull => null;

        /// <summary>Null許容実数値取得プロパティ。</summary>
        /// <value>Null許容実数型。</value>
        /// <returns>インスタンスが格納している実数値。</returns>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        public double? DoubleValueOrNull => null;

        /// <summary>Null許容日付値取得プロパティ。</summary>
        /// <value>Null許容日付型。</value>
        /// <returns>インスタンスが格納している日付値。</returns>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        public DateTime? DateValueOrNull => null;

        /// <summary>整数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納していれば、真を返す。</returns>
        public bool IsEmpty => true;

        /// <summary>整数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納していれば、真を返す。</returns>
        public bool IsInteger => true;

        /// <summary>実数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>実数値を格納していれば、真を返す。</returns>
        public bool IsDouble => true;

        /// <summary>文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>文字列値を格納していれば、真を返す。</returns>
        public bool IsString => true;

        /// <summary>日付文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>日付文字列値を格納していれば、真を返す。</returns>
        public bool IsDate => true;

        /// <summary>数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納しているので真を返す。</returns>
        public bool IsNumber => true;

        #endregion

        #region "constructor"

        /// <summary>コンストラクタ。</summary>
        static ValueEmpty()
        {
            empty = new ValueEmpty();
        }

        #endregion
    }
}
