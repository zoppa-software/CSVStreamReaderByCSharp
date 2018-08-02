namespace SplitFormts.Csv
{
    /// <summary>読み取りモード。</summary>
    public enum CsvReadMode
    {
        /// <summary>" のエスケープを使用する。</summary>
        UseEscape,

        /// <summary>" のエスケープを使用し、','間のトリムを行わない。</summary>
        UseEscapeNoTrim,

        /// <summary>','の区切りのみ行う。</summary>
        NotEscape,
    }
}
