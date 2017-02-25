using System.Collections.Generic;

namespace Parser {

public class State {

    private Dictionary<string, int>    _intVariables;
    private Dictionary<string, string> _stringVariables;

    public State() {
        this._intVariables    = new Dictionary<string, int>();
        this._stringVariables = new Dictionary<string, string>();
    }

    public int GetInt(string variableName) {
        return this._intVariables.ContainsKey(variableName) ?
               this._intVariables[variableName] : 0;
    }

    public string GetString(string variableName) {
        return this._stringVariables.ContainsKey(variableName) ?
               this._stringVariables[variableName] : "";
    }

    public void Set(string variableName, int val) {
        this._intVariables[variableName] = val;
    }

    public void Set(string variableName, string val) {
        this._stringVariables[variableName] = val;
    }

}

}  // namespace Parser
