namespace IniParser.Lines
{
    internal sealed class SectionLine : IniLine
    {
        public string Name { get; }

        public SectionLine(string name)
        {
            Name = name;
        }

        public override string Serialize()
            => $"[{Name}]";
    }
}