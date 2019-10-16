import { ControlledEditor } from "@monaco-editor/react";
import { FunctionalComponent, h } from "preact";
import { RunResult } from "../../types/types";

interface Props {
    readonly code: string;
    readonly codeChanged: (s: string) => void;
    readonly submitCode: (s: string) => void;
    readonly errors?: RunResult;
}

const openInAction = (actionName: ("debug" | "preview"), code: string) =>
    window.open(`/codefile/${actionName}?code=${encodeURI(code)}`, "_blank");

const getScore = (code: string) => code
    .replace(/\s/g, "")
    .length.toString();

const Comp: FunctionalComponent<Readonly<Props>> = ({ code, submitCode }) => {
    return (<div>
        <div class="field">
            <label class="label">Code</label>
            <div class="control">
                <ControlledEditor
                    value={code}
                    className="editor"
                    height="40vh"
                    language="csharp"
                />
            </div>
        </div>
        <div class="field">
            <label class="label">Count</label>
            <div class="control">
                <label id="Count">{getScore(code)}</label>
            </div>
        </div>
        <div class="field is-grouped">
            <div class="control">
                <button class="button is-primary" onClick={() => submitCode(code)}>Submit</button>
            </div>

            <div class="buttons has-addons">
                <button class="button" onClick={() => openInAction("preview", code)}>View .CS file</button>

                <button class="button" onClick={() => openInAction("debug", code)}>View debug .CS file</button>
            </div>
        </div>
    </div>);
};

export default Comp;
