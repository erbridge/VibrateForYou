using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Parser {

public class Parser {
    public event Action<string> eventOnNewStatement;
    public event Action<List<KeyValuePair<string, string> > >
    eventOnChangeChoices;

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
    }

    public void SetPage(string title) {
        this.RunPageCommands(title);

        foreach (string statement in this.GetPageStatements(title)) {
            if (eventOnNewStatement != null) {
                eventOnNewStatement(statement);
            }
        }

        if (eventOnChangeChoices != null) {
            eventOnChangeChoices(this.GetPageChoices(title));
        }
    }

    private void RunPageCommands(string title) {
        Page page = this._pages[title];

        List<Command> commands = page.GetCommands();

        foreach (Command command in commands) {
            if (command.Execute(this._state)) {
                Debug.Log("Ran " + command);
            }
        }
    }

    private List<string> GetPageStatements(string title) {
        Page page = this._pages[title];

        List<Statement> statements = page.GetStatements();

        List<string> output = new List<string>();

        foreach (Statement statement in statements) {
            string message = statement.GetText(this._state);

            if (message.Length > 0) {
                output.Add(message);
            }
        }

        return output;
    }

    private List<KeyValuePair<string, string> > GetPageChoices(string title) {
        Page page = this._pages[title];

        List<Choice> choices = page.GetChoices();

        List<KeyValuePair<string, string> > output =
        new List<KeyValuePair<string, string> >();

        foreach (Choice choice in choices) {
            string message = choice.GetText(this._state);

            if (message.Length > 0) {
                output.Add(
                    new KeyValuePair<string, string>(
                        message,
                        choice.GetTarget()
                    )
                );
            }
        }

        return output;
    }

}

}  // namespace Parser
