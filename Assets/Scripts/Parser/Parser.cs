using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Parser {

public class Parser {
    private State _state;
    private Dictionary<string, Page> _pages;

    public Parser(string name) {
        this._state = new State();
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

            this._pages.Add(title, new Page(rawScriptSection));
        }

        this.RunPageCommands("startconvo");

        foreach (string s in this.GetPageStatements("startconvo")) {
            Debug.Log(s);
        }

        foreach (string s in this.GetPageChoices("startconvo")) {
            Debug.Log(s);
        }
    }

    public void RunPageCommands(string title) {
        Page page = this._pages[title];

        List<Command> commands = page.GetCommands();

        foreach (Command command in commands) {
            command.Execute(this._state);

            Debug.Log(command);
        }
    }

    public List<string> GetPageStatements(string title) {
        Page page = this._pages[title];

        List<Statement> statements = page.GetStatements();

        List<string> output = new List<string>();

        foreach (Statement statement in statements) {
            string message = statement.Get(this._state);

            if (message.Length > 0) {
                output.Add(message);
            }
        }

        return output;
    }

    public List<string> GetPageChoices(string title) {
        Page page = this._pages[title];

        List<Choice> choices = page.GetChoices();

        List<string> output = new List<string>();

        foreach (Choice choice in choices) {
            string message = choice.Get(this._state);

            if (message.Length > 0) {
                output.Add(message);
            }
        }

        return output;
    }

}

}  // namespace Parser
