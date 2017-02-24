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

    public string Get(State state) {
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

        return "Choice - " + this._text + " -> " + this._target + condition;
    }

}

}  // namespace Parser
