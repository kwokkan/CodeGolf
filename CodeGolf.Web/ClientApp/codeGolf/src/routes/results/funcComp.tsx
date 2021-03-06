import { FunctionalComponent, h } from "preact";
import { Circular } from "styled-loaders";

import { ifLoaded, LoadingState, Result } from "../../types/types";

interface Props {
  readonly results: LoadingState<ReadonlyArray<Result>>;
}

const Row: FunctionalComponent<Result> = ({ score, loginName, rank, avatarUri }) => (<tr>
  <td>{rank}.</td>
  <td>
    <figure class="image is-48x48"><img src={avatarUri} /></figure>
  </td>
  <td>{loginName}</td>
  <td>{score}</td>
</tr>);

const FuncComp: FunctionalComponent<Readonly<Props>> = ({ results }) => (
  <section class="section">
    <h1 class="title">Final Results</h1>
    <table class="table is-striped is-fullwidth">
      <thead>
        <tr>
          <th />
          <th />
          <th>Name</th>
          <th>Score</th>
        </tr>
      </thead>
      <tbody>
        {ifLoaded(results, r => r.map(a => <Row key={a.rank} {...a} />), () => [<Circular key={1} />])}
      </tbody>
    </table>
  </section>);

export default FuncComp;
