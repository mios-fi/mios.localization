using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mios.Localization.Readers {
	public class EmbeddedTextLocalizationReader : TextLocalizationReader {
		private static readonly Regex DictionaryStartPattern = new Regex(@"^@\*\s+DICTIONARY");
		private static readonly Regex WhitespaceDetectionPattern = new Regex(@"^\s*");
		protected override IEnumerable<string> ReadLines(TextReader reader) {
			bool inDictionary = false;
			string line, whitespace = null;
			while((line=reader.ReadLine())!=null) {
				if(!inDictionary) {
					inDictionary = DictionaryStartPattern.IsMatch(line);
					continue;
				}
				if(line.StartsWith("*@")) {
					inDictionary = false;
					whitespace = null;
					continue;
				}
				if(whitespace==null) {
					if(line.Trim()==String.Empty) continue;
					var m = WhitespaceDetectionPattern.Match(line);
					whitespace = m.Value;
				}
				if(!line.StartsWith(whitespace)) continue;
				yield return line.Substring(whitespace.Length);
			}
		}
	}
}
