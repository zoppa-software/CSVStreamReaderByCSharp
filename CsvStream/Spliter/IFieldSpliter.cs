using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvStream.Spliter
{
    /// <summary>項目分割を行う機能のインターフェイス。</summary>
    internal interface IFieldSpliter
    {
        /// <summary>読込モードを取得する。</summary>
        /// <value>真偽値。</value>
        CsvReadMode ReadMode
        {
            get;
        }

        /// <summary>CSVを一行分取り込み、値変換した ValueObject配列を取得する。</summary>
        /// <returns>ValueObject 配列。</returns>
        IValueObject[] ReadGroup();

        FiledPosition ReadPosition(Stream stream, Encoding srcEncoding);
    }
}
