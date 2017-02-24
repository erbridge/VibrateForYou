public class Command {
    private string _verb;
    private string _arguments;

    private Command _condition;
    private Command _anticondition;

    public Command(
        string  verb,
        string  arguments,
        Command condition = null,
        Command anticondition = null
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
