using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Idealde.Modules.CodeEditor.Models;
using ScintillaNET;

namespace Idealde.Modules.CodeEditor.Views
{
    /// <summary>
    /// Interaction logic for CodeEditorView.xaml
    /// </summary>
    public partial class CodeEditorView : UserControl, ICodeEditorView
    {
        #region CodeEditorView Definition
        public const string CodeSuggestionsDirectoryName = "Code-Suggestions";

        public const string ColorSchemesDirectoryName = "Color-Schemes";

        public const string ConfigFileName = "Config.txt";

        public static readonly DependencyProperty ResourcesDirectoryProperty =
            DependencyProperty.Register("ResourcesDirectory", typeof(string), typeof(CodeEditorView),
                new PropertyMetadata(string.Empty, OnResourcesDirectoryChanged));

        public static readonly DependencyProperty LexerProperty =
            DependencyProperty.Register("Lexer", typeof(Lexer), typeof(CodeEditorView),
                new PropertyMetadata(Lexer.Null, OnLexerChanged));
        public bool IsDirty { get; private set; }

        // Define autocomplete menu and key word
        private readonly AutocompleteMenu _autocompleteMenu;

        private readonly Dictionary<Lexer, string> _keyWord1;

        private readonly Dictionary<Lexer, string> _keyWord2;
        #endregion // Definition

        #region Lexer property
        public Lexer Lexer
        {
            get { return (Lexer)GetValue(LexerProperty); }
            set { SetValue(LexerProperty, value); }
        }

        private static void OnLexerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as CodeEditorView;

            editor?.ReloadLexer();
        }
        #endregion //Lexer property

        #region ResourcesDirectory property
        public string ResourcesDirectory
        {
            get { return (string)GetValue(ResourcesDirectoryProperty); }
            set { SetValue(ResourcesDirectoryProperty, value); }
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
                if (e.OldValue != null && ((string) e.OldValue) == "") return;
                editor?.SetValue(e.Property, e.OldValue);
                return;
            }

            editor?.ReloadConfig();
        }
        #endregion // ResourcesDirectory property

        #region CodeEditorView initialization
        public CodeEditorView()
        {
            InitializeComponent();
            IsDirty = false;
            // Scintilla keyword storages
            _keyWord1 = new Dictionary<Lexer, string>();

            _keyWord2 = new Dictionary<Lexer, string>();

            // Wire autocomplete menu
            _autocompleteMenu = new AutocompleteMenu
            {
                TargetControlWrapper = new ScintillaWrapper(ScintillaEditor),
                AllowsTabKey = true,
                AppearInterval = 1
            };
            
            ScintillaFolding();
        }

        private void ScintillaInitialize(object sender, RoutedEventArgs e)
        {
        }
        #endregion // initialization

        // reload configuration ( After resources directory changed )
        private void ReloadConfig()
        {
            var config =
                Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(
                    File.ReadAllText($"{ResourcesDirectory}\\{ConfigFileName}"));

            // update FontSize and FontFamily
            foreach (var item in ScintillaEditor.Styles)
            {
                item.Font = config.FontFamily;
                item.Size = config.FontSize;
            }

            LoadColorScheme(config.ColorSchemeName);
        }

        // reload lexer ( After Lexer changed )
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
            ScintillaFolding();
        }

        // code Folding ( after Lexer changed )
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

        // Load AutocompleteItem ( after Lexer changed )
        private void ReloadAutocompleteMenu()
        {
            _autocompleteMenu.Clear();

            switch (ScintillaEditor.Lexer)
            {
                case Lexer.Container:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Container.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Container.txt");
                    }
                    break;
                case Lexer.Null:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Null.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Null.txt");
                    }
                    break;
                case Lexer.Ada:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Ada.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Ada.txt");
                    }
                    break;
                case Lexer.Asm:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Asm.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Asm.txt");
                    }
                    break;
                case Lexer.Batch:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Batch.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Batch.txt");
                    }
                    break;
                case Lexer.Cpp:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Cpp.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Cpp.txt");
                    }
                    break;
                case Lexer.Css:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Css.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Css.txt");
                    }
                    break;
                case Lexer.Fortran:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Fortran.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Fortran.txt");
                    }
                    break;
                case Lexer.FreeBasic:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\FreeBasic.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\FreeBasic.txt");
                    }
                    break;
                case Lexer.Html:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Html.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Html.txt");
                    }
                    break;
                case Lexer.Json:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Json.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Json.txt");
                    }
                    break;
                case Lexer.Lisp:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Lisp.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Lisp.txt");
                    }
                    break;
                case Lexer.Lua:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Lua.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Lua.txt");
                    }
                    break;
                case Lexer.Pascal:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Pascal.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Pascal.txt");
                    }
                    break;
                case Lexer.Perl:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Perl.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Perl.txt");
                    }
                    break;
                case Lexer.PhpScript:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PhpScript.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PhpScript.txt");
                    }
                    break;
                case Lexer.PowerShell:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PowerShell.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PowerShell.txt");
                    }
                    break;
                case Lexer.Properties:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Properties.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Properties.txt");
                    }
                    break;
                case Lexer.PureBasic:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PureBasic.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\PureBasic.txt");
                    }
                    break;
                case Lexer.Python:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Python.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Python.txt");
                    }
                    break;
                case Lexer.Ruby:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Ruby.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Ruby.txt");
                    }
                    break;
                case Lexer.Smalltalk:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Smalltalk.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Cpp.txt");
                    }
                    break;
                case Lexer.Sql:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Sql.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Sql.txt");
                    }
                    break;
                case Lexer.Vb:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Vb.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Vb.txt");
                    }
                    break;
                case Lexer.VbScript:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\VbScript.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\VbScript.txt");
                    }
                    break;
                case Lexer.Verilog:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Verilog.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Verilog.txt");
                    }
                    break;
                case Lexer.Xml:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Xml.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Xml.txt");
                    }
                    break;
                case Lexer.BlitzBasic:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\BlitzBasic.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\BlitzBasic.txt");
                    }
                    break;
                case Lexer.Markdown:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Markdown.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\Markdown.txt");
                    }
                    break;
                case Lexer.R:
                    if (File.Exists($"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\R.txt"))
                    {
                        _autocompleteMenu.LoadSuggestions(
                            $"{ResourcesDirectory}\\{CodeSuggestionsDirectoryName}\\R.txt");
                    }
                    break;
                default:
                    break;
            }
        }

        // Load color scheme ( after ResourcesDirectory changed )
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

        #region Scintilla event handler ( Display line number )

        // OnTextChanged to display line number
        private int _maxRowCharLength;
        private void OnTextChanged(object sender, System.EventArgs e)
        {
            IsDirty = true;
            IsDirtyChanged?.Invoke(this, e);
            int newMaxRowCharLength = ScintillaEditor.Lines.Count.ToString().Length;
            if (_maxRowCharLength == newMaxRowCharLength) return;
            ScintillaEditor.Margins[0].Width = ScintillaEditor.TextWidth(ScintillaNET.Style.LineNumber, new string('9', newMaxRowCharLength + 1)) + 2;
            _maxRowCharLength = newMaxRowCharLength;
        }

        #endregion

        #region CodeEditorView behaviors

        public event EventHandler IsDirtyChanged;

        public void SetResourceDirectory(string directory)
        {
            ResourcesDirectory = directory;
        }

        public void SetLexer(Lexer lexer)
        {
            Lexer = lexer;
        }

        public void Goto(int row, int column)
        {
            ScintillaEditor.Lines[row].Goto();
            ScintillaEditor.GotoPosition(ScintillaEditor.CurrentPosition + column);
        }

        public void SetContent(string text)
        {
            ScintillaEditor.Text = text;
        }
        public string GetContent()
        {
            return ScintillaEditor.Text;
        }
        #endregion //behaviors
        
    }
}
