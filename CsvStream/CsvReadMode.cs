using System;
using System.Collections.Generic;
using System.Text;

namespace CsvStream
{
    /// <summary>読み取りモード。</summary>
    public enum CsvReadMode
    {
        /// <summary>" のエスケープを使用する。</summary>
        UsedEscape,

        /// <summary>" のエスケープを使用し、','間のトリムを行わない。</summary>
        UsedEscapeNoTrim,

        /// <summary>','の区切りのみ行う。</summary>
        NotEscape,
    }
}
