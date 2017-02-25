using System.Collections;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Parser {

public class Command {
    protected string _verb;
    protected string _arguments;

    protected ConditionalCommand _condition;
    protected ConditionalCommand _anticondition;

    public Command(
        string             verb,
        string             arguments,
        ConditionalCommand condition = null,
        ConditionalCommand anticondition = null
    ) {
        this._verb = verb;
        this._arguments = arguments;

        // If this is non-null, we expect this command to be true before
        // executing this command.
        this._condition = condition;

        // If this is non-null, we expect this command to be false before
        // executing this command.
        this._anticondition = anticondition;
    }

    public virtual bool Execute(State state) {
        if ((this._condition != null) && !this._condition.Execute(state)) {
            return false;
        }

        if ((this._anticondition != null) && this._anticondition.Execute(
            state
        )) {
            return false;
        }

        return true;
    }

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        string anticondition = "";

        if (this._anticondition != null) {
            anticondition = " (ifnot " + this._anticondition + ")";
        }

        return "Command - " + this._verb + " : " + this._arguments +
               condition + anticondition;
    }

}

public class ConditionalCommand : Command {

    public ConditionalCommand(
        string             verb,
        string             arguments,
        ConditionalCommand condition = null,
        ConditionalCommand anticondition = null
    ) : base(verb, arguments, condition, anticondition) { }

    public override bool Execute(State state) {
        if (!base.Execute(state)) {
            return false;
        }

        if (this._verb == "else") {
            return true;
        }

        Regex argumentRegex = new Regex(
            @"\$(?<variable>\w+)\s+(?<op>(is|>|<|>=|<=))\s+(?<target>\d+)"
        );

        Match match = argumentRegex.Match(this._arguments);

        if (match.Success) {
            string variableName = match.Groups["variable"].Value;
            string op = match.Groups["op"].Value;

            int variable = state.GetInt(variableName);
            int target   = Int32.Parse(match.Groups["target"].Value);

            if (op == "is") {
                return variable == target;
            }

            if (op == ">") {
                return variable > target;
            }

            if (op == "<") {
                return variable < target;
            }

            if (op == ">=") {
                return variable >= target;
            }

            if (op == "<=") {
                return variable <= target;
            }
        }

        return false;
    }   // Execute

}

public class SetCommand : Command {

    public SetCommand(
        string             arguments,
        ConditionalCommand condition = null,
        ConditionalCommand anticondition = null
    ) : base("set", arguments, condition, anticondition) { }

    public override bool Execute(State state) {
        if (!base.Execute(state)) {
            return false;
        }

        Regex argumentRegex = new Regex(
            @"\$(?<variable>\w+)\s+to\s+\$?(?<target>\w+)?\s*" +
            @"((?<modifier>[\+-]?)\s*(?<intValue>\d+))?" +
            "(\"(?<stringValue>.*?)\")?"
        );

        Match match = argumentRegex.Match(this._arguments);

        if (match.Success) {
            string variableName = match.Groups["variable"].Value;

            if (match.Groups["intValue"].Success) {
                int val = Int32.Parse(match.Groups["intValue"].Value);

                if (
                    match.Groups["target"].Success &&
                    match.Groups["modifier"].Success
                ) {
                    int variable = state.GetInt(variableName);

                    string target   = match.Groups["target"].Value;
                    string modifier = match.Groups["modifier"].Value;

                    if (target == "it") {
                        if (modifier == "+") {
                            variable += val;
                        } else {
                            variable -= val;
                        }
                    } else {
                        int targetVal = state.GetInt(target);

                        if (modifier == "+") {
                            variable = targetVal + val;
                        } else {
                            variable = targetVal - val;
                        }
                    }

                    state.Set(variableName, variable);
                } else {
                    state.Set(variableName, val);
                }
            } else {
                state.Set(
                    variableName,
                    match.Groups["stringValue"].Value
                );
            }

            return true;
        }

        return false;
    }   // Execute

}

public class CountdownCommand : Command {

    public CountdownCommand(
        string             arguments,
        ConditionalCommand condition = null,
        ConditionalCommand anticondition = null
    ) : base("countdown", arguments, condition, anticondition) { }

    public IEnumerator Execute(State state, Parser parser) {
        if (!base.Execute(state)) {
            yield return null;
        }

        Regex argumentRegex = new Regex(
            @"(?<duration>\d+)s\s*->\s*(?<target>.+)"
        );

        Match match = argumentRegex.Match(this._arguments);

        if (match.Success) {
            int duration = Int32.Parse(match.Groups["duration"].Value);

            yield return new WaitForSeconds(duration);

            string target = match.Groups["target"].Value;

            parser.SetPage(target);
        }

        yield return null;
    }

}

}  // namespace Parser
