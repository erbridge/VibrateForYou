using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

public class Page {

    private List<Command>   _commands;
    private List<Statement> _statements;
    private List<Choice>    _choices;

    public Page(string rawScript) {
        this._commands = new List<Command>();

        this._statements = new List<Statement>();
        this._choices    = new List<Choice>();

        Regex commandRegex = new Regex(
            @"\((?<verb>\w+?):(?<arguments>[^\)]+?)\)"
        );
        Regex otherChoiceRegex = new Regex(
            @"\[\[(?<text>.+?)->(?<target>.+?)\]\]"
        );
        Regex selfChoiceRegex = new Regex(@"\[\[(?<target>.+)\]\]");

        Command condition = null;

        foreach (string rawLine in rawScript.Split('\n')) {
            string line = rawLine.Trim();

            if (line.Length == 0) {
                condition = null;

                continue;
            }

            Match match = commandRegex.Match(line);

            if (match.Success) {
                string verb = match.Groups["verb"].Value;
                string arguments = match.Groups["arguments"].Value;

                // Debug.Log("Command - " + verb + " : " + arguments);

                if ((verb == "if") || (verb == "elseif") || (verb == "else")) {
                    condition = new Command(verb, arguments, null, condition);
                } else {
                    this._commands.Add(new Command(verb, arguments, condition));
                }

                continue;
            }

            if ((condition != null) && (line == "]")) {
                continue;
            }

            match = otherChoiceRegex.Match(line);

            if (match.Success) {
                string text   = match.Groups["text"].Value;
                string target = match.Groups["target"].Value;

                this._choices.Add(new Choice(text, target, condition));

                continue;
            }

            match = selfChoiceRegex.Match(line);

            if (match.Success) {
                string target = match.Groups["target"].Value;

                this._choices.Add(new Choice(target, target, condition));

                continue;
            }

            this._statements.Add(new Statement(line, condition));
        }

        foreach (Command command in this._commands) {
            Debug.Log(command.ToString());
        }

        foreach (Statement statement in this._statements) {
            Debug.Log(statement.ToString());
        }

        foreach (Choice choice in this._choices) {
            Debug.Log(choice.ToString());
        }
    }

}
