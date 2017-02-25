using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Parser {

public class Page {

    private List<Command>   _commands;
    private List<Statement> _statements;
    private List<Choice>    _choices;

    public Page(string rawScript) {
        this._commands = new List<Command>();

        this._statements = new List<Statement>();
        this._choices    = new List<Choice>();

        Regex commandRegex = new Regex(
            @"\((?<verb>\w+?):(?<args>[^\)]*?)\)"
        );
        Regex otherChoiceRegex = new Regex(
            @"\[\[(?<text>.+?)->(?<target>.+?)\]\]"
        );
        Regex selfChoiceRegex = new Regex(@"\[\[(?<target>.+)\]\]");

        ConditionalCommand condition = null;

        foreach (string rawLine in rawScript.Split('\n')) {
            string line = rawLine.Trim();

            if (line.Length == 0) {
                condition = null;

                continue;
            }

            Match match = commandRegex.Match(line);

            if (match.Success) {
                string verb = match.Groups["verb"].Value;
                string args = match.Groups["args"].Value;

                if ((verb == "if") || (verb == "else-if") || (verb == "else")) {
                    condition = new ConditionalCommand(
                        verb,
                        args,
                        null,
                        condition
                    );
                } else if (verb == "set") {
                    this._commands.Add(new SetCommand(args, condition));
                } else if (verb == "countdown") {
                    this._commands.Add(new CountdownCommand(args, condition));
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
    }

    public List<Command> GetCommands() {
        return this._commands;
    }

    public List<Statement> GetStatements() {
        return this._statements;
    }

    public List<Choice> GetChoices() {
        return this._choices;
    }

}

}  // namespace Parser
