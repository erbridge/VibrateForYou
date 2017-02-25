using System.Text.RegularExpressions;

namespace Parser {

public class Statement {
    private string _text;

    private ConditionalCommand _condition;

    public Statement(string text, ConditionalCommand condition = null) {
        this._text = text;

        // If this is non-null, we expect this command to be true before
        // adding this statement.
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

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        return "Statement - " + this._text +  condition;
    }

}

}  // namespace Parser
