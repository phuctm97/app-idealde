using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Idealde.Modules.CodeEditor.Autocomplete;
using Idealde.Modules.CodeEditor.Config;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor.Views
{
    /// <summary>
    /// Interaction logic for CodeEditorView.xaml
    /// </summary>
    public partial class CodeEditorView : UserControl, ICodeEditorView
    {
        public const string CodeSuggestionsDirectoryName = "Code-Suggestions";

        public const string ColorSchemesDirectoryName = "Color-Schemes";

        public const string ConfigFileName = "Config.txt";

        public static readonly DependencyProperty ResourcesDirectoryProperty =
            DependencyProperty.Register("ResourcesDirectory", typeof(string), typeof(CodeEditorView),
                new PropertyMetadata(string.Empty, OnResourcesDirectoryChanged));

        public static readonly DependencyProperty LexerProperty =
            DependencyProperty.Register("Lexer", typeof(Lexer), typeof(CodeEditorView),
                new PropertyMetadata(Lexer.Null, OnLexerChanged));


        public CodeEditorView()
        {
            InitializeComponent();

            // Scintilla keyword storages
            _keyWord1 = new Dictionary<Lexer, string>();

            _keyWord2 = new Dictionary<Lexer, string>();

            // Wire autocomplete menu
            _autocompleteMenu = new AutocompleteMenu
            {
                TargetControlWrapper = new ScintillaWrapper(ScintillaEditor),
                AllowsTabKey = true
            };

            ScintillaFolding();
        }

        private void ScintillaInitialize(object sender, RoutedEventArgs e)
        {
        }

        private static void OnLexerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorView;

            editor?.ReloadLexer();
        }

        public Lexer Lexer
        {
            get { return (Lexer)GetValue(LexerProperty); }
            set { SetValue(LexerProperty, value); }
        }


        private static void OnResourcesDirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorView;

            var resourcesDirectory = e.NewValue as string;

            if ( // resources directory exists
                !Directory.Exists(resourcesDirectory) ||

                // color-schemes directory exists
                !Directory.Exists($"{resourcesDirectory}\\{ColorSchemesDirectoryName}") ||

                // code-suggestions directory exists
                !Directory.Exists($"{resourcesDirectory}\\{CodeSuggestionsDirectoryName}") ||

                // config file exists
                !File.Exists($"{resourcesDirectory}\\{ConfigFileName}"))
            {
                // roll back assignment
                editor?.SetValue(e.Property, e.OldValue);
                return;
            }

            editor?.ReloadConfig();
        }

        public string ResourcesDirectory
        {
            get { return (string)GetValue(ResourcesDirectoryProperty); }
            set { SetValue(ResourcesDirectoryProperty, value); }
        }

        private readonly AutocompleteMenu _autocompleteMenu;

        private readonly Dictionary<Lexer, string> _keyWord1;

        private readonly Dictionary<Lexer, string> _keyWord2;


        private void ReloadConfig()
        {
            var config =
                Newtonsoft.Json.JsonConvert.DeserializeObject<Config.Config>(
                    File.ReadAllText($"{ResourcesDirectory}\\{ConfigFileName}"));

            // update FontSize and FontFamily
            foreach (var item in ScintillaEditor.Styles)
            {
                item.Font = config.FontFamily;
                item.Size = config.FontSize;
            }

            LoadColorScheme(config.ColorSchemeName);
        }

        private void ReloadLexer()
        {
            ScintillaEditor.Lexer = Lexer;

            // Scintilla keywords for highlighting new language
            if (_keyWord1.ContainsKey(ScintillaEditor.Lexer))
                ScintillaEditor.SetKeywords(0, _keyWord1[ScintillaEditor.Lexer]);
            if (_keyWord2.ContainsKey(ScintillaEditor.Lexer))
                ScintillaEditor.SetKeywords(1, _keyWord2[ScintillaEditor.Lexer]);

            // Autocomplete suggestions for new language
            ReloadAutocompleteMenu();
        }


        private void ScintillaFolding()
        {
            ScintillaEditor.SetProperty("fold", "1");
            ScintillaEditor.SetProperty("fold.compact", "1");

            // Configure a margin to display folding symbols
            ScintillaEditor.Margins[2].Type = MarginType.Symbol;
            ScintillaEditor.Margins[2].Mask = Marker.MaskFolders;
            ScintillaEditor.Margins[2].Sensitive = true;
            ScintillaEditor.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (var i = 25; i <= 31; i++)
            {
                ScintillaEditor.Markers[i].SetForeColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                ScintillaEditor.Markers[i].SetBackColor(System.Drawing.ColorTranslator.FromHtml("#AAAAAA"));
            }

            // Configure folding markers with respective symbols
            ScintillaEditor.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            ScintillaEditor.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            ScintillaEditor.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            ScintillaEditor.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            ScintillaEditor.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            ScintillaEditor.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            ScintillaEditor.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            ScintillaEditor.AutomaticFold = AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change;
        }

        private void ReloadAutocompleteMenu()
        {
            _autocompleteMenu.Clear();

            switch (ScintillaEditor.Lexer)
            {
                case Lexer.Container:
                    break;
                case Lexer.Null:
                    break;
                case Lexer.Ada:
                    break;
                case Lexer.Asm:
                    break;
                case Lexer.Batch:
                    break;
                case Lexer.Cpp:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Cpp.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Cpp.txt");
                    }
                    break;
                case Lexer.Css:
                    break;
                case Lexer.Fortran:
                    break;
                case Lexer.FreeBasic:
                    break;
                case Lexer.Html:
                    break;
                case Lexer.Json:
                    break;
                case Lexer.Lisp:
                    break;
                case Lexer.Lua:
                    break;
                case Lexer.Pascal:
                    break;
                case Lexer.Perl:
                    break;
                case Lexer.PhpScript:
                    break;
                case Lexer.PowerShell:
                    break;
                case Lexer.Properties:
                    break;
                case Lexer.PureBasic:
                    break;
                case Lexer.Python:
                    break;
                case Lexer.Ruby:
                    break;
                case Lexer.Smalltalk:
                    break;
                case Lexer.Sql:
                    break;
                case Lexer.Vb:
                    break;
                case Lexer.VbScript:
                    break;
                case Lexer.Verilog:
                    break;
                case Lexer.Xml:
                    break;
                case Lexer.BlitzBasic:
                    break;
                case Lexer.Markdown:
                    break;
                case Lexer.R:
                    break;
                default:
                    break;
            }
        }

        private void LoadColorScheme(string colorScheme)
        {
            if (!Directory.Exists($"{ResourcesDirectory}\\{ColorSchemesDirectoryName}\\{colorScheme}")) return;

            // Cpp
            if (File.Exists($"{ResourcesDirectory}\\{ColorSchemesDirectoryName}\\{colorScheme}\\Cpp.json"))
            {
                var cppColorScheme =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<ColorScheme>(
                        File.ReadAllText($"{ResourcesDirectory}\\{ColorSchemesDirectoryName}\\{colorScheme}\\Cpp.json"));

                ScintillaEditor.Styles[ScintillaNET.Style.Default].BackColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.DFBackColor.R, cppColorScheme.DFBackColor.G,
                        cppColorScheme.DFBackColor.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Default].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.DFForeColor.R, cppColorScheme.DFForeColor.G,
                        cppColorScheme.DFForeColor.B);

                ScintillaEditor.StyleClearAll();

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Identifier].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Identifier.R, cppColorScheme.Identifier.G,
                        cppColorScheme.Identifier.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Comment].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Comment.R, cppColorScheme.Comment.G,
                        cppColorScheme.Comment.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.CommentLine].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.CommentLine.R, cppColorScheme.CommentLine.G,
                        cppColorScheme.CommentLine.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.CommentDoc].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.CommentDoc.R, cppColorScheme.CommentDoc.G,
                        cppColorScheme.CommentDoc.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Number].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Number.R, cppColorScheme.Number.G,
                        cppColorScheme.Number.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.String].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.String.R, cppColorScheme.String.G,
                        cppColorScheme.String.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Character].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Character.R, cppColorScheme.Character.G,
                        cppColorScheme.Character.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Preprocessor].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Preprocessor.R, cppColorScheme.Preprocessor.G,
                        cppColorScheme.Preprocessor.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Operator].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Operator.R, cppColorScheme.Operator.G,
                        cppColorScheme.Operator.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Regex].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Regex.R, cppColorScheme.Regex.G, cppColorScheme.Regex.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.CommentLineDoc].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.CommentLineDoc.R, cppColorScheme.CommentLineDoc.G,
                        cppColorScheme.CommentLineDoc.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Word].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Word.R, cppColorScheme.Word.G, cppColorScheme.Word.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.Word2].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.Word2.R, cppColorScheme.Word2.G, cppColorScheme.Word2.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.CommentDocKeyword].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.CommentDocKeyword.R, cppColorScheme.CommentDocKeyword.G,
                        cppColorScheme.CommentDocKeyword.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.CommentDocKeywordError].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.CommentDocKeywordError.R,
                        cppColorScheme.CommentDocKeywordError.G, cppColorScheme.CommentDocKeywordError.B);

                ScintillaEditor.Styles[ScintillaNET.Style.Cpp.GlobalClass].ForeColor =
                    System.Drawing.Color.FromArgb(cppColorScheme.GlobalClass.R, cppColorScheme.GlobalClass.G,
                        cppColorScheme.GlobalClass.B);

                if (_keyWord1.ContainsKey(Lexer.Cpp))
                    _keyWord1[Lexer.Cpp] = cppColorScheme.KeyWord1;
                else
                    _keyWord1.Add(Lexer.Cpp, cppColorScheme.KeyWord1);

                if (_keyWord2.ContainsKey(Lexer.Cpp))
                    _keyWord2[Lexer.Cpp] = cppColorScheme.KeyWord2;
                else
                    _keyWord2.Add(Lexer.Cpp, cppColorScheme.KeyWord2);
            }

            // Lua
            if (File.Exists($"{ResourcesDirectory}\\{ColorSchemesDirectoryName}\\{colorScheme}\\Lua.json"))
            {
            }
        }
    }


}
