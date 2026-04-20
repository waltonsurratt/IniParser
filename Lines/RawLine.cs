namespace IniParser.Lines
{
    internal sealed class RawLine : IniLine
    {
        public string Content { get; set; }

        public RawLine(string content)
        {
            Content = content;
        }

        public override string Serialize() => Content;
    }
}