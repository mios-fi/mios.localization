using System;
using System.Collections;
using System.Collections.Generic;

namespace Mios.Localization {

  internal class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue> {
	  private readonly IDictionary<TKey, TValue> wrapped;
    public ReadOnlyDictionary(IDictionary<TKey,TValue> wrapped) {
		  this.wrapped = wrapped;
		}
	  public bool IsReadOnly {
			get { return true; }
		}
		public void Add(KeyValuePair<TKey, TValue> item) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public void Clear() {
			throw new InvalidOperationException("Dictionary is read only");
		}
	  public bool Contains(KeyValuePair<TKey, TValue> item) {
	    throw new NotImplementedException();
	  }
	  public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
	    throw new NotImplementedException();
	  }
	  public bool Remove(KeyValuePair<TKey, TValue> item) {
			throw new InvalidOperationException("Dictionary is read only");
		}
	  public int Count {
      get { return wrapped.Count; }
	  }
	  public bool ContainsKey(TKey key) {
      return wrapped.ContainsKey(key);
	  }
	  public void Add(TKey key, TValue value) {
			throw new InvalidOperationException("Dictionary is read only");
		}
		public bool Remove(TKey key) {
			throw new InvalidOperationException("Dictionary is read only");
		}
	  public bool TryGetValue(TKey key, out TValue value) {
      return wrapped.TryGetValue(key, out value);
	  }
	  public TValue this[TKey key] {
      get { return wrapped[key]; }
			set { throw new InvalidOperationException("Dictionary is read only"); }
		}
	  public ICollection<TKey> Keys {
      get { return wrapped.Keys; }
	  }
	  public ICollection<TValue> Values {
      get { return wrapped.Values; }
	  }
	  public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
      return wrapped.GetEnumerator();
	  }
	  IEnumerator IEnumerable.GetEnumerator() {
	    return GetEnumerator();
	  }
	}
}