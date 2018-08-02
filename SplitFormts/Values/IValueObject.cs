using System;

namespace SplitFormts.Values
{
    /// <summary>値インターフェイス。</summary>
    public interface IValueObject
    {
        #region "properties"

        /// <summary>インスタンスの種類を取得する。</summary>
        /// <value>インスタンス種類値。</value>
        /// <returns>インスタンス種類値。</returns>
        ValueObjectTypeEnum ValueObjectType { get; }

        /// <summary>整数値取得プロパティ。</summary>
        /// <value>整数型。</value>
        /// <returns>インスタンスが格納している整数値。</returns>
        /// <exception cref="NotSupportedException">インスタンスが整数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        int IntegerValue { get; }

        /// <summary>実数値取得プロパティ。</summary>
        /// <value>実数型。</value>
        /// <returns>インスタンスが格納している実数値。</returns>
        /// <exception cref="NotSupportedException">インスタンスが実数値を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納している実数値を取得する。</remarks>
        double DoubleValue { get; }

        /// <summary>文字列値取得プロパティ。</summary>
        /// <value>文字列型。</value>
        /// <returns>インスタンスが格納している文字列値。</returns>
        /// <remarks>インスタンスが格納してい文字列値を取得する。</remarks>
        string StringValue { get; }

        /// <summary>日付値取得プロパティ。</summary>
        /// <value>日付型。</value>
        /// <returns>インスタンスが格納している日付値。</returns>
        /// <exception cref="NotSupportedException">インスタンスが日付型を格納していないとき発生。</exception>
        /// <remarks>インスタンスが格納してい日付型を取得する。</remarks>
        DateTime DateValue { get; }

        /// <summary>Null許容整数値取得プロパティ。</summary>
        /// <value>Null許容整数型。</value>
        /// <returns>インスタンスが格納している整数値。</returns>
        /// <remarks>インスタンスが格納している整数値を取得する。</remarks>
        int? IntegerValueOrNull { get; }

        /// <summary>実数値取得プロパティ。実数として取得できなかったときNull。</summary>
        /// <value>Null許容実数型。</value>
        /// <returns>実数値。実数として取得できなかったときNull。</returns>
        double? DoubleValueOrNull { get; }

        /// <summary>日付値取得プロパティ。日付として取得できなかったときNull。</summary>
        /// <value>Null許容日付型。</value>
        /// <returns>日付値。日付として取得できなかったときNull。</returns>
        DateTime? DateValueOrNull { get; }

        /// <summary>空白値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>空白値を格納していれば、真を返す。</returns>
        bool IsEmpty { get; }

        /// <summary>整数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納していれば、真を返す。</returns>
        bool IsInteger { get; }

        /// <summary>実数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>実数値を格納していれば、真を返す。</returns>
        bool IsDouble { get; }

        /// <summary>文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>文字列値を格納していれば、真を返す。</returns>
        bool IsString { get; }

        /// <summary>日付文字列値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>日付文字列値を格納していれば、真を返す。</returns>
        bool IsDate { get; }

        /// <summary>数値か確認するプロパティ。</summary>
        /// <value>真偽値。</value>
        /// <returns>整数値を格納しているので真を返す。</returns>
        bool IsNumber { get; }

        #endregion
    }
}
