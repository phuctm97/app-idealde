using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AutocompleteMenuNS;

namespace Idealde.Modules.CodeEditor.Models
{

    public enum AutocompleteItemCategory
    {
        Method = 0,
        Keyword = 1
    }


    public class AutocompleteMenu : AutocompleteMenuNS.AutocompleteMenu
    {
        List<AutocompleteItem> _suggestions = new List<AutocompleteItem>();
        public List<AutocompleteItem> Suggestions { get { return _suggestions; } }

        public AutocompleteMenu()
        {
            AutoCompleteInitialize();
        }

        private void AutoCompleteInitialize()
        {
            this.Font = new System.Drawing.Font("consolas", 11);
            this.ImageList = new System.Windows.Forms.ImageList();
            this.SetAutocompleteItems(Suggestions);
        }

        public bool LoadSuggestions(string fileName)
        {
            string key = String.Empty;

            string _initElementContent = string.Empty;

            if (File.Exists(fileName))
            {
                _initElementContent = File.ReadAllText(fileName);
            }
            else return false;

            Regex _regex;
            MatchCollection _matches;
            string itemList;

            //Load keywords from file
            _regex = new Regex("(?s)(?<=keywords:)(.*?)(?=-end-)");

            itemList = _regex.Match(_initElementContent).Value;

            _matches = Regex.Matches(itemList, @"(?<=\||\n|^)(.*?)(?=\||\n|\z)");
            if (_matches.Count > 0)
                foreach (Match match in _matches)
                {
                    this.Add(match.Value, AutocompleteItemCategory.Keyword);
                }

            //Load methods from file
            _regex = new Regex("(?s)(?<=methods:)(.*?)(?=-end-)");

            itemList = _regex.Match(_initElementContent).Value;

            _matches = Regex.Matches(itemList, @"(?<=\||\n|^)(.*?)(?=\||\n|\z)");
            if (_matches.Count > 0)
                foreach (Match match in _matches)
                {
                    this.Add(match.Value, AutocompleteItemCategory.Method);
                }
            return true;
        }



        // Fix: thay vì add thì cho SetImage( loại, đường dẫn ), có thể ép kiểu enum sang int
        public bool SetImage(AutocompleteItemCategory category, string fileName)
        {
            if (File.Exists(fileName))
            {
                this.ImageList.Images.Add(System.Drawing.Image.FromFile(fileName));
                this.ImageList.Images.SetKeyName((int)category, fileName);
                return true;
            }
            else
                return false;
        }

        public bool Add(string value, AutocompleteItemCategory category)
        {
            AutocompleteItem item = null;
            switch (category)
            {
                case AutocompleteItemCategory.Keyword:
                    item = new AutocompleteItem(value);
                    break;
                case AutocompleteItemCategory.Method:
                    item = new MethodAutocompleteItem(value);// { ImageIndex = (int)category };
                    break;
                default:
                    return false;
            }


            if (item.Text != "")
                if (!_suggestions.Exists(x => x.Text == item.Text))
                {
                    Suggestions.Add(item);
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Clear all suggestion items
        /// </summary>
        public void Clear()
        {
            Suggestions.Clear();
            this.SetAutocompleteItems(Suggestions);
        }
    }



}
