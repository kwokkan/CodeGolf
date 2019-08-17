import { debounce } from "lodash";
import { Component, h, RenderableProps } from "preact";

import { getDemoChallenge, submitDemo, tryCompile } from "../../api";
import { getFunctionDeclaration } from "../../funcDeclaration";
import { ChallengeSet, LoadingState, RunResult } from "../../types/types";
import FuncComp from "./funcComp";

interface State { readonly challenge: LoadingState<ChallengeSet>; readonly code: string; readonly errors: LoadingState<RunResult | undefined>; }

export default class Comp extends Component<{}, State> {
  private readonly tryCompile = debounce(async () => {
    this.setState(s => ({ ...s, errors: { type: "Loading" } }));
    const errors = {
      type: "Loaded",
      data: { type: "CompileError", errors: await tryCompile(this.state.challenge.type === "Loaded" ? this.state.challenge.data.id : "", this.state.code) }
    } as LoadingState<RunResult>;
    this.setState(s => ({ ...s, errors }));
  }, 1000);
  constructor() {
    super();
    this.state = { challenge: { type: "Loading" }, code: "", errors: { type: "Loaded", data: undefined } };
  }
  public readonly componentDidMount = async () => {
    const challenge = await getDemoChallenge();
    this.setState(s => ({ ...s, challenge: { type: "Loaded", data: challenge } }));
  }
  public readonly render = (_: RenderableProps<{}>, { errors, code, challenge }: Readonly<State>) =>
    <FuncComp code={code} errors={errors} challenge={challenge} codeChanged={this.codeChanged} submitCode={this.submitCode} onCodeClick={this.onCodeClick} />

  public readonly codeChanged = async (code: string) => {
    this.setState(s => ({ ...s, code }));
    this.tryCompile();
  }
  private readonly onCodeClick = () => {
    if (this.state.code === "" && this.state.challenge.type === "Loaded") {
      this.setState({ ...this.state, code: getFunctionDeclaration(this.state.challenge.data) });
    }
  }
  private readonly submitCode = async (code: string, reCaptcha: string) => {
    this.setState(s => ({ ...s, errors: { type: "Loading" } }));
    const res = { type: "Loaded", data: await submitDemo(code, reCaptcha) } as LoadingState<RunResult>;
    this.setState(s => ({ ...s, errors: res }));
  }
}
