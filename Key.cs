using IniParser.Lines;

namespace IniParser
{
    public sealed class Key
    {
        private readonly KeyLine _line;

        internal Key(KeyLine line)
        {
            _line = line;
        }

        public string Name => _line.Name;
        public string? Value => _line.Value;

        public Key SetValue(string? value)
        {
            _line.Value = value;
            return this;
        }
    }
}