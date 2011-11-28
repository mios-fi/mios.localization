using System;
using System.Collections;
using System.Collections.Generic;

namespace Mios.Localization {
	public class ReadOnlyDictionary<TKey, TValue> : DictionaryDecorator<TKey, TValue> {
		public ReadOnlyDictionary(IDictionary<TKey,TValue> wrapped) : base(wrapped) {
		}
		public override bool IsReadOnly {
			get { return true; }
		}
		public override void Add(KeyValuePair<TKey, TValue> item) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public override void Clear() {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public override bool Remove(KeyValuePair<TKey, TValue> item) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public override void Add(TKey key, TValue value) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public override bool Remove(TKey key) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public override TValue this[TKey key] {
			set { throw new InvalidOperationException("Dictionary is read only"); }
		}
	}
}