using System.Text.RegularExpressions;

namespace Parser {

public class Choice {
    private string _text;
    private string _target;

    private ConditionalCommand _condition;

    public Choice(
        string             text,
        string             target,
        ConditionalCommand condition = null
    ) {
        this._text   = text;
        this._target = target;

        // If this is non-null, we expect this command to be true before
        // adding this choice.
        this._condition = condition;
    }

    public string GetText(State state) {
        if ((this._condition != null) && !this._condition.Execute(state)) {
            return "";
        }

        string text = this._text;

        Regex variableRegex = new Regex(@"\$(?<variableName>\w+)");

        foreach (Match match in variableRegex.Matches(text)) {
            string variableName = match.Groups["variableName"].Value;
            string variable     = state.GetString(variableName);

            text = text.Replace(match.Value, variable);
        }

        return text;
    }

    public string GetTarget() {
        return this._target;
    }

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        return "Choice - " + this._text + " -> " + this._target + condition;
    }

}

}  // namespace Parser
