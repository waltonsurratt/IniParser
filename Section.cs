using IniParser.Lines;

namespace IniParser
{
    public sealed class Section
    {
        public string Name { get; }

        private readonly Document _document;
        private readonly int _headerLineIndex;

        private readonly Dictionary<string, KeyLine> _keys =
            new(StringComparer.OrdinalIgnoreCase);

        internal Section(string name, Document document, int headerLineIndex)
        {
            Name = name;
            _document = document;
            _headerLineIndex = headerLineIndex;
        }

        internal void Register(KeyLine line)
        {
            _keys[line.Name] = line;
        }

        public Key? GetKey(string name)
        {
            return _keys.TryGetValue(name, out var line)
                ? new Key(line)
                : null;
        }

        public Key CreateKey(string name)
        {
            if (_keys.TryGetValue(name, out var existing))
                return new Key(existing);

            var line = new KeyLine("", name, null, null);

            // ✅ Insert directly after last key in this section
            int insertIndex = _document.FindSectionInsertionIndex(_headerLineIndex);
            _document.InsertLine(insertIndex, line);

            _keys[name] = line;
            return new Key(line);
        }

        public bool RemoveKey(string name)
        {
            if (!_keys.Remove(name, out var line))
                return false;

            _document.RemoveLine(line);
            return true;
        }

        internal IEnumerable<KeyLine> KeyLines => _keys.Values;
    }
}