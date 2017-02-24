using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Parser {

    private Dictionary<string, Page> _pages;

    public Parser(string name) {
        this._pages = new Dictionary<string, Page>();

        string rawScript = Resources.Load(
            "NarrativeScripts/" +
            name
        ).ToString();

        Regex groupTitleRegex = new Regex(@"(^|\n)\*(.+)\n");

        foreach (Match match in groupTitleRegex.Matches(rawScript)) {
            string title = match.Groups[2].Value;

            int startIndex = match.Groups[2].Index + title.Length;
            int length     = rawScript.Length - startIndex;

            Match nextMatch = match.NextMatch();

            if (nextMatch.Success) {
                length = nextMatch.Groups[2].Index - 1 - startIndex;
            }

            string rawScriptSection = rawScript.Substring(startIndex, length);

            Debug.Log(title);

            this._pages.Add(title, new Page(rawScriptSection));
        }
    }

}
