public class Choice {
    private string _text;
    private string _target;

    private Command _condition;

    public Choice(string text, string target, Command condition = null) {
        this._text   = text;
        this._target = target;

        // If this is non-null, we expect this command to be true before
        // adding this choice.
        this._condition = condition;
    }

    public override string ToString() {
        string condition = "";

        if (this._condition != null) {
            condition = " (if " + this._condition + ")";
        }

        return "Choice - " + this._text + " -> " + this._target + condition;
    }

}
