using System.Collections;
using System.Collections.Generic;

namespace Mios.Localization {
	public class DictionaryDecorator<TKey, TValue> : IDictionary<TKey, TValue> {
		protected readonly IDictionary<TKey, TValue> Wrapped;
		public DictionaryDecorator(IDictionary<TKey,TValue> wrapped) {
			this.Wrapped = wrapped;
		}
		public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
			return Wrapped.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public virtual void Add(KeyValuePair<TKey, TValue> item) {
			Wrapped.Add(item);
		}
		public virtual void Clear() {
			Wrapped.Clear();
		}
		public virtual bool Contains(KeyValuePair<TKey, TValue> item) {
			return Wrapped.Contains(item);
		}
		public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
			Wrapped.CopyTo(array, arrayIndex);
		}
		public virtual bool Remove(KeyValuePair<TKey, TValue> item) {
			return Wrapped.Remove(item);
		}
		public virtual int Count {
			get { return Wrapped.Count; }
		}
		public virtual bool IsReadOnly {
			get { return true; }
		}
		public virtual bool ContainsKey(TKey key) {
			return Wrapped.ContainsKey(key);
		}
		public virtual void Add(TKey key, TValue value) {
			Wrapped.Add(key, value);
		}
		public virtual bool Remove(TKey key) {
			return Wrapped.Remove(key);
		}
		public virtual bool TryGetValue(TKey key, out TValue value) {
			return Wrapped.TryGetValue(key, out value);
		}
		public virtual TValue this[TKey key] {
			get { return Wrapped[key]; }
			set { Wrapped[key] = value; }
		}
		public virtual ICollection<TKey> Keys {
			get { return Wrapped.Keys; }
		}
		public virtual ICollection<TValue> Values {
			get { return Wrapped.Values; }
		}
	}
}