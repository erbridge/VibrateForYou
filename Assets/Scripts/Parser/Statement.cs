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

        return this._text;
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
