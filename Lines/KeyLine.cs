namespace IniParser.Lines
{
    internal sealed class KeyLine : IniLine
    {
        public string LeadingWhitespace;
        public string Name;
        public string? Value;
        public string? InlineComment;

        public KeyLine(string leadingWhitespace, string name, string? value, string? comment)
        {
            LeadingWhitespace = leadingWhitespace;
            Name = name;
            Value = value;
            InlineComment = comment;
        }

        public override string Serialize()
        {
            var core = Value is null
                ? Name
                : $"{Name}={Value}";

            return InlineComment is null
                ? $"{LeadingWhitespace}{core}"
                : $"{LeadingWhitespace}{core}{InlineComment}";
        }
    }
}