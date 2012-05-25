using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mios.Localization.Implementations {
	public class TextDictionaryHandler : ILocalizationReader {
		public virtual ILocalizationDictionary Read(string sourceFile) {
			if(!File.Exists(sourceFile)) return null;
			using(var reader = new StreamReader(sourceFile)) {
				return Load(reader);
			}
		}
		public virtual ILocalizationDictionary Load(TextReader reader) {
			return Load(ReadLines(reader));
		}
		protected ILocalizationDictionary Load(IEnumerable<string> lines) {
			var dictionary = new LocalizationDictionary();
			string key = null, locale = null, value = null, whitespace = null;
			foreach(var line in lines) {
				if(line.TrimStart().StartsWith("#")) {
					// Comment
				} else if(line.TrimStart().StartsWith("@include")) {
					// Include command
				} else if(line.Trim()==String.Empty) {
					// Skip blank lines
				} else if(ParseMultilineValue(line, whitespace, ref value)) {
					if(key == null) throw new Exception("Expected key before entry");
					if(locale == null) throw new Exception("Expected entry before continuation");
					dictionary[locale,key] += Environment.NewLine+value;
				} else if(ParseValue(line, ref whitespace, ref locale, ref value)) {
					if(key == null) throw new Exception("Expected key before entry");
					dictionary[locale,key] = value;
				} else if(ParseKey(line, ref key)) {
					// Key
				} else {
					throw new Exception("Unexpected line '"+line+"'");
				}
			}
			return dictionary;
		}

		static readonly Regex MultilineValuePattern = new Regex(@"^(\s+)");
		static bool ParseMultilineValue(string str, string whitespace, ref string value) {
			if(whitespace==null) return false;
			var m = MultilineValuePattern.Match(str);
			if(!m.Success || !m.Groups[1].Value.StartsWith(whitespace) || m.Groups[1].Value.Length==whitespace.Length) return false;
			value = str.Substring(m.Length);
			return true;
		}

		static readonly Regex ValuePattern = new Regex(@"^(\s+)(\w+)\s+");
		static bool ParseValue(string str, ref string whitespace, ref string language, ref string value) {
			var m = ValuePattern.Match(str);
			if(!m.Success) return false;
			whitespace = m.Groups[1].Value;
			language = m.Groups[2].Value;
			value = str.Substring(m.Length);
			return true;
		}

		static readonly Regex KeyPattern = new Regex(@"^\S");
		static bool ParseKey(string str, ref string key) {
			var m = KeyPattern.Match(str);
			if(!m.Success) return false;
			key = str.Trim();
			return true;
		}

		protected virtual IEnumerable<string> ReadLines(TextReader reader) {
			string line;
			while((line=reader.ReadLine())!=null) {
				yield return line;
			}
		}
	}
}