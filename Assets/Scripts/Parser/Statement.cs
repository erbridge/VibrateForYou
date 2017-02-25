using System.Text.RegularExpressions;
using UnityEngine;

namespace Parser {

public class Statement {
    private string _text;

    private ConditionalCommand _condition;

    private bool _isEither;

    public Statement(
        string             text,
        ConditionalCommand condition = null,
        bool               isEither = false
    ) {
        this._text = text;

        // If this is non-null, we expect this command to be true before
        // adding this statement.
        this._condition = condition;

        this._isEither = isEither;
    }

    public string GetText(State state) {
        if ((this._condition != null) && !this._condition.Execute(state)) {
            return "";
        }

        string text = this._text;

        if (this._isEither) {
            string[] options = text.Split('#');

            text = options[Random.Range(0, options.Length - 1)].Trim();
        }

        Regex variableRegex = new Regex(@"\$(?<variableName>\w+)");

        foreach (Match match in variableRegex.Matches(text)) {
            string variableName = match.Groups["variableName"].Value;
            string variable     = state.GetString(variableName);

            text = text.Replace(match.Value, variable);
        }

        return text;
    }

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        return "Statement - " + this._text +  condition;
    }

}

}  // namespace Parser
