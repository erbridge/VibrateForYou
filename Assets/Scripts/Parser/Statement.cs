public class Statement {
    private string _text;

    private Command _condition;

    public Statement(string text, Command condition = null) {
        this._text = text;

        // If this is non-null, we expect this command to be true before
        // adding this statement.
        this._condition = condition;
    }

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        return "Statement - " + this._text +  condition;
    }

}
