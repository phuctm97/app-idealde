namespace Idealde.Modules.CodeEditor.Models
{

    internal class RGB
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    internal class ColorScheme
    {
        public string KeyWord1 { get; set; }
        public string KeyWord2 { get; set; }
        public RGB DFBackColor { get; set; }
        public RGB DFForeColor { get; set; }
        public RGB Identifier { get; set; }
        public RGB Comment { get; set; }
        public RGB CommentLine { get; set; }
        public RGB CommentDoc { get; set; }
        public RGB Number { get; set; }
        public RGB String { get; set; }
        public RGB Character { get; set; }
        public RGB Preprocessor { get; set; }
        public RGB Operator { get; set; }
        public RGB Regex { get; set; }
        public RGB CommentLineDoc { get; set; }
        public RGB Word { get; set; }
        public RGB Word2 { get; set; }
        public RGB CommentDocKeyword { get; set; }
        public RGB CommentDocKeywordError { get; set; }
        public RGB GlobalClass { get; set; }


    }
}
