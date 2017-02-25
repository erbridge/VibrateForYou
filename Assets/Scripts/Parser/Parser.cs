using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace Parser {

public class Parser : MonoBehaviour {
    public event Action<float>  eventOnTyping;
    public event Action<string> eventOnNewStatement;
    public event Action eventOnReceived;
    public event Action eventOnRead;
    public event Action<List<KeyValuePair<string, string> > >
    eventOnChangeChoices;

    private State _state;
    private Dictionary<string, Page> _pages;

    void Start() {
        // this.Init("Jaimie", "you've matched!");
    }

    public void Init(string name, string startingTitle) {
        this._state = new State();
        this._pages = new Dictionary<string, Page>();

        string rawScript = Resources.Load(
            "NarrativeScripts/" +
            name
        ).ToString().Replace("\r", "");

        Regex groupTitleRegex = new Regex(@"(^|\n)\*(.+)\n");

        foreach (Match match in groupTitleRegex.Matches(rawScript)) {
            string title = match.Groups[2].Value;

            int startIndex = match.Groups[2].Index + title.Length;
            int length     = rawScript.Length - startIndex;

            Match nextMatch = match.NextMatch();

            if (nextMatch.Success) {
                length = nextMatch.Groups[2].Index - 1 - startIndex;
            }

            string rawScriptSection = rawScript.Substring(startIndex, length);

            this._pages.Add(title, new Page(rawScriptSection));
        }

        this.SetPage(startingTitle, true);
    }

    public void SetPage(string title, bool skipDelays = false) {
        // FIXME: This needs to be more specific than stopping all coroutines.
        this.StopAllCoroutines();
        this.StartCoroutine(UpdateListeners(title, skipDelays));
    }

    private IEnumerator UpdateListeners(string title, bool skipDelays) {
        if (!skipDelays) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 5f));
        }

        if (this.eventOnReceived != null) {
            this.eventOnReceived();
        }

        if (!skipDelays) {
            yield return new WaitForSeconds(
                2f + UnityEngine.Random.Range(0.8f, 1.5f) *
                UnityEngine.Random.Range(1f, 4f)
            );
        }

        if (this.eventOnRead != null) {
            this.eventOnRead();
        }

        if (!skipDelays) {
            yield return new WaitForSeconds(
                0.5f + UnityEngine.Random.Range(0.8f, 1.5f) *
                UnityEngine.Random.Range(2f, 4f)
            );
        }

        List<CountdownCommand> countdownCommands = this.RunPageCommands(title);

        foreach (string statement in this.GetPageStatements(title)) {
            if (!skipDelays) {
                float duration = statement.Length * 0.2f;

                if (UnityEngine.Random.value < 0.25f) {
                    do {
                        float hesitaitonDuration = duration *
                        UnityEngine.Random.Range(
                            0.1f,
                            2f
                        );

                        if (this.eventOnTyping != null) {
                            this.eventOnTyping(hesitaitonDuration);
                        }

                        yield return new WaitForSeconds(hesitaitonDuration);
                    } while (UnityEngine.Random.value < 0.1f);
                }

                if (this.eventOnTyping != null) {
                    this.eventOnTyping(duration);
                }

                yield return new WaitForSeconds(duration);
            }

            if (this.eventOnNewStatement != null) {
                this.eventOnNewStatement(statement);
            }

            Debug.Log(statement);
        }

        yield return new WaitForSeconds(1);

        List<KeyValuePair<string, string> > choices = this.GetPageChoices(
            title
        );

        if (this.eventOnChangeChoices != null) {
            this.eventOnChangeChoices(choices);
        }

        foreach (CountdownCommand command in countdownCommands) {
            this.RunCountdownCommand(command);

            Debug.Log("Ran " + command);
        }

        yield return null;
    }   // UpdateListeners

    private List<CountdownCommand> RunPageCommands(string title) {
        Page page = this._pages[title];

        List<Command> commands = page.GetCommands();

        List<CountdownCommand> output = new List<CountdownCommand>();

        foreach (Command command in commands) {
            if (command as CountdownCommand != null) {
                output.Add(command as CountdownCommand);
            } else if (command.Execute(this._state)) {
                Debug.Log("Ran " + command);
            }
        }

        return output;
    }

    private List<string> GetPageStatements(string title) {
        Page page = this._pages[title];

        List<Statement> statements = page.GetStatements();

        List<string> output = new List<string>();

        foreach (Statement statement in statements) {
            string message = statement.GetText(this._state);

            if (message.Length > 0) {
                output.Add(message);
            }
        }

        return output;
    }

    private List<KeyValuePair<string, string> > GetPageChoices(string title) {
        Page page = this._pages[title];

        List<Choice> choices = page.GetChoices();

        List<KeyValuePair<string, string> > output =
        new List<KeyValuePair<string, string> >();

        foreach (Choice choice in choices) {
            string message = choice.GetText(this._state);

            if (message.Length > 0) {
                output.Add(
                    new KeyValuePair<string, string>(
                        message,
                        choice.GetTarget()
                    )
                );

                Debug.Log(message + " -> " + choice.GetTarget());
            }
        }

        return output;
    }

    private void RunCountdownCommand(CountdownCommand command) {
        this.StartCoroutine(command.Execute(this._state, this));
    }

}

}  // namespace Parser
