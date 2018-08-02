using System;
using System.Collections.Generic;
using System.Text;

namespace CsvStream.ValueObjects
{
    /// <summary>読み取りデータの種類を表す列挙値。</summary>
    public enum ValueObjectTypeEnum
    {
        /// <summary>空白値タイプ。</summary>
        TypeEmpty,

        /// <summary>整数値（Byte）タイプ。</summary>
        TypeByte,

        /// <summary>整数値（Short）タイプ。</summary>
        TypeShort,

        /// <summary>整数値（Integer）タイプ。</summary>
        TypeInt,

        /// <summary>実数値（Single）タイプ。</summary>
        TypeFloat,

        /// <summary>実数値（Double）タイプ。</summary>
        TypeDouble,

        /// <summary>日付タイプ。</summary>
        TypeDate,

        /// <summary>文字列タイプ。</summary>
        TypeString,
    }
}
