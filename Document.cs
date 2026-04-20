using IniParser.Lines;
using System.Text;

namespace IniParser
{
    public sealed class Document
    {
        private readonly List<IniLine> _lines = new();
        private readonly Dictionary<string, Section> _sections =
            new(StringComparer.OrdinalIgnoreCase);

        internal void InsertLine(int index, IniLine line)
            => _lines.Insert(index, line);

        internal void RemoveLine(IniLine line)
            => _lines.Remove(line);

        internal int FindSectionInsertionIndex(int headerIndex)
        {
            int i = headerIndex + 1;
            while (i < _lines.Count && _lines[i] is KeyLine)
                i++;

            return i;
        }

        public static Document Load(string path, Encoding? encoding = null)
        {
            encoding ??= Encoding.Unicode;
            var doc = new Document();

            Section? current = null;

            foreach (var raw in File.ReadAllLines(path, encoding))
            {
                int index = doc._lines.Count;
                var trimmed = raw.TrimStart();
                var leading = raw[..^trimmed.Length];

                if (trimmed.Length == 0 || trimmed.StartsWith(";"))
                {
                    doc._lines.Add(new RawLine(raw));
                    continue;
                }

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    var name = trimmed[1..^1];
                    current = new Section(name, doc, index);
                    doc._sections[name] = current;
                    doc._lines.Add(new SectionLine(name));
                    continue;
                }

                int semicolon = trimmed.IndexOf(';');
                var comment = semicolon >= 0 ? trimmed[semicolon..] : null;
                var content = semicolon >= 0 ? trimmed[..semicolon] : trimmed;

                int equals = content.IndexOf('=');
                if (equals < 0)
                {
                    doc._lines.Add(new RawLine(raw));
                    continue;
                }

                var key = content[..equals].Trim();
                var value = content[(equals + 1)..].Trim();

                var keyLine = new KeyLine(
                    leading,
                    key,
                    value.Length == 0 ? null : value,
                    comment
                );

                current?.Register(keyLine);
                doc._lines.Add(keyLine);
            }

            return doc;
        }

        public Section GetSection(string name)
            => _sections.TryGetValue(name, out var s)
                ? s
                : CreateSection(name);

        public Section CreateSection(string name)
        {
            if (_sections.TryGetValue(name, out var existing))
                return existing;

            _lines.Add(new RawLine(""));
            int index = _lines.Count;
            _lines.Add(new SectionLine(name));

            var section = new Section(name, this, index);
            _sections[name] = section;
            return section;
        }

        public void Save(string path, Encoding? encoding = null)
        {
            encoding ??= Encoding.Unicode;
            using var writer = new StreamWriter(path, false, encoding);
            foreach (var line in _lines)
                writer.WriteLine(line.Serialize());
        }
    }
}