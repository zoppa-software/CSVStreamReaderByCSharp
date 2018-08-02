using System.Collections.Generic;

namespace SplitFormts.Values
{
    /// <summary>生成済み値をキャッシュするコレクション。</summary>
    internal sealed class ValueCache
    {
        #region "struct"

        /// <summary>世代と値をペアで保持する。</summary>
        private struct GenValue
        {
            /// <summary>世代。</summary>
            public int generation;

            /// <summary>値参照。</summary>
            public readonly IValueObject value;

            /// <summary>コンストラクタ。</summary>
            /// <param name="obj">値参照。</param>
            public GenValue(IValueObject obj)
            {
                this.generation = 0;
                this.value = obj;
            }

            /// <summary>ハッシュコード値を取得する。</summary>
            /// <returns>ハッシュコード値。</returns>
            public override int GetHashCode()
            {
                return this.value.GetHashCode();
            }

            /// <summary>等価比較を行う。</summary>
            /// <param name="obj">比較対象。</param>
            /// <returns>比較結果。</returns>
            public override bool Equals(object obj)
            {
                return this.value.Equals(obj);
            }
        }

        #endregion

        #region "fields"

        /// <summary>保持が確定した値。</summary>
        private Dictionary<IValueObject, IValueObject> confirmed;

        /// <summary>生成された値キャッシュ。</summary>
        private Dictionary<IValueObject, GenValue> cache;

        #endregion

        #region "constructor"

        /// <summary>コンストラクタ。</summary>
        public ValueCache()
        {
            this.confirmed = new Dictionary<IValueObject, IValueObject>();
            this.cache = new Dictionary<IValueObject, GenValue>();
        }

        #endregion

        #region "methods"

        /// <summary>キャッシュから値を取得する。</summary>
        /// <param name="key">キーなる値。</param>
        /// <param name="chars">読み込み文字リスト。</param>
        /// <param name="st">文字開始位置。</param>
        /// <param name="ed">文字終了位置。</param>
        /// <returns>取得した値。</returns>
        public IValueObject GetCache(ValueKey key, List<char> chars, int st, int ed)
        {
            GenValue pair;
            IValueObject result;
            if (this.confirmed.TryGetValue(key, out result)) {
                // 確定テーブルより値を取得
            }
            else if (this.cache.TryGetValue(key, out pair)) {
                // キャッシュより値を取得（世代をインクリメント）
                pair.generation++;
                result = pair.value;
            }
            else {
                // 値を作成し、キャッシュに追加
                result = new ValueObject(chars, st, ed);
                this.cache.Add(result, new GenValue(result));

                // キャッシュが多くなったとき、参照数が複数の項目を確定領域へ
                if (this.cache.Count > 10000) {
                    foreach (var c in this.cache.Values) {
                        if (c.generation > 0) {
                            this.confirmed.Add(c.value, c.value);
                        }
                    }
                    this.cache.Clear();
                }
            }
            return result;
        }

        #endregion
    }
}
