import { FunctionalComponent, h } from "preact";
import { Circular } from "styled-loaders";

import { Game, ifLoaded, LoadingState, Round } from "../../types/types";

const Modal: FunctionalComponent<{ readonly hide: () => void, readonly challenges: ReadonlyArray<Round> }> = ({ hide }) =>
    (<div class="modal is-active">
        <div class="modal-background" />
        <div class="modal-content">
            <header class="modal-card-head">
                <p class="modal-card-title">Create New Game</p>
                <button class="delete" aria-label="close" onClick={hide} />
            </header>
            <section class="modal-card-body">
                <div class="field">
                    <label class="label">Access Code</label>
                    <div class="control">
                        <input class="input is-primary" type="text" />
                    </div>
                </div>
            </section>
            <footer class="modal-card-foot">
                <button class="button is-success">Save changes</button>
                <button class="button" onClick={hide}>Cancel</button>
            </footer>
        </div>
    </div>);

const Row: FunctionalComponent<{ readonly g: Game, readonly resetGame: ((g: Guid) => void) }> = ({ g, resetGame }) =>
  (<article class="message">
        <div class="message-header">
            <p>Code: {g.accessKey}</p>
        </div>
        <div class="message-body">
            <div class="message-content">
                Rounds:
                <ul>
                    {a.rounds.map(b => <li key={b.challengeId}>{b.title}</li>)}
                </ul>
                <button class="button">Reset Game</button>
            </div>
        </div>
    </article>);

type Props = ({
    readonly myGames: LoadingState<ReadonlyArray<Game>>;
    readonly allChallenges: LoadingState<ReadonlyArray<Round>>;
    readonly showCreate: boolean
    readonly toggleCreate: (state: boolean) => void;
});

const FuncComp: FunctionalComponent<Readonly<Props>> = ({ myGames, allChallenges, showCreate, toggleCreate }) =>
    ifLoaded(myGames, g =>
        (<div><section class="accordions">
            {g.map((a: Game) => <Row a={a} key={a.id} />)}
        </section>
            {ifLoaded(allChallenges, c => (showCreate ? <Modal hide={() => toggleCreate(false)} challenges={c} /> : null), () => null)}
            <button className="button" onClick={() => toggleCreate(!showCreate)}>Create New</button>
        </div>),
        () =>
            <Circular />);

export default FuncComp;
