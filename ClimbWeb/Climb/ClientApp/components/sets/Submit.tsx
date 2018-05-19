import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";
import {MatchSummary} from "./MatchSummary";
import {MatchEdit} from "./MatchEdit";

interface ISetSubmitState {
    set: ClimbClient.SetDto | null;
    selectedMatch: ClimbClient.MatchDto | null;
    game: ClimbClient.Game | null;
}

export class Submit extends React.Component<RouteComponentProps<any>, ISetSubmitState> {
    client: ClimbClient.SetClient;
    setId: number;

    constructor(props: RouteComponentProps<any>) {
        super(props);

        this.client = new ClimbClient.SetClient(window.location.origin);
        this.setId = this.props.match.params.setId;

        this.state = {
            set: null,
            selectedMatch: null,
            game: null,
        };

        this.onSubmit = this.onSubmit.bind(this);
        this.onAddMatch = this.onAddMatch.bind(this);
        this.onMatchEdited = this.onMatchEdited.bind(this);
        this.onMatchCancelled = this.onMatchCancelled.bind(this);
    }

    componentDidMount() {
        this.loadSet();
    }

    render() {
        const game = this.state.game;
        const set = this.state.set;
        if (!set || !set.matches || !game) return <RingLoader color={"#123abc"}/>;

        if (this.state.selectedMatch != null) {
            return <MatchEdit match={this.state.selectedMatch}
                              game={game}
                              onEdit={this.onMatchEdited}
                              onCancel={this.onMatchCancelled}/>;
        }

        const matches = set.matches.map(
            (m, i) => <MatchSummary key={i}
                                    game={game}
                                    match={m}
                                    onSelect={m => this.setState({ selectedMatch: m })}/>);

        return (
            <div>
                <div id="set-submit-header">
                    <div id="set-submit-player-info">
                        <div className="set-submit-player">
                            <div className="set-submit-player-top">
                                <img src="https://cdn2.iconfinder.com/data/icons/professions/512/user_boy_avatar-64.png"/>
                                <div className="set-submit-player-meta">
                                    <div className="set-submit-player-rank">#4</div>
                                    <div className="set-submit-player-trend">↓</div>
                                </div>
                            </div>
                        </div>

                        <div id="set-submit-score-container">
                            <div className="set-submit-score right">{set.player1Score}</div>
                            <div id="set-submit-score-divide">-</div>
                            <div className="set-submit-score left">{set.player2Score}</div>
                        </div>

                        <div className="set-submit-player">
                            <div className="set-submit-player-top">
                                <div className="set-submit-player-meta">
                                    <div className="set-submit-player-rank">#4</div>
                                    <div className="set-submit-player-trend">↓</div>
                                </div>
                                <img src="https://cdn2.iconfinder.com/data/icons/professions/512/user_boy_avatar-64.png"/>
                            </div>
                        </div>
                    </div>

                    <div id="set-submit-player-names">
                        <div className="set-submit-player-name left">Player Name</div>
                        <div className="set-submit-player-name right">Player Name</div>
                    </div>
                </div>

                <div>{matches}</div>
                <div className="match-summary-buttons">
                    <button onClick={this.onSubmit}>Submit</button>
                    <button onClick={this.onAddMatch}>Add Match</button>
                </div>
            </div>
        );
    }

    private loadSet() {
        this.client.get(this.setId)
            .then(set => {
                if (set.matches != null) {
                    set.matches.sort((a, b) => a.index - b.index);
                }
                this.setState({ set: set });
                console.log(set);
                this.loadGame(set.gameID);
            })
            .catch(reason => `Could not load set\n${reason}`);
    }

    private loadGame(gameId: number) {
        const gameClient = new ClimbClient.GameClient(window.location.origin);
        gameClient.get(gameId)
            .then(game => this.setState({ game: game }))
            .catch(reason => alert(`Can't load game\n${reason}`));
    }

    private onMatchCancelled() {
        this.setState({ selectedMatch: null });
    }

    private onMatchEdited(match: ClimbClient.MatchDto) {
        const set = this.state.set;
        if (!set || !set.matches) throw new Error();

        set.matches[match.index] = match;

        this.setState({
            selectedMatch: null,
            set: set,
        });
    }

    private onSubmit() {
        const set = this.state.set;
        if (!set || !set.matches) throw new Error();

        const setRequest = new ClimbClient.SubmitRequest();
        setRequest.setID = set.id;
        setRequest.matches = new Array<ClimbClient.MatchForm>(set.matches.length);

        for (let i = 0; i < set.matches.length; i++) {
            const match = set.matches[i];
            const matchForm = new ClimbClient.MatchForm();
            matchForm.init(match);
            setRequest.matches[i] = matchForm;
        }

        this.client.submit(setRequest)
            .then(() => {
                console.log("Set submitted!");
                window.location.reload();
            })
            .catch(reason => alert(`Could not submit set\n${reason}`));
    }

    private onAddMatch() {
        const set = this.state.set;
        if (!set || !set.matches) throw new Error();

        const newMatch = new ClimbClient.MatchDto();
        newMatch.index = set.matches.length;

        if (newMatch.index > 0) {
            const prevMatch = set.matches[newMatch.index - 1];
            if (!prevMatch.player1Characters || !prevMatch.player2Characters) throw new Error();
            newMatch.player1Characters = prevMatch.player1Characters.slice(0);
            newMatch.player2Characters = prevMatch.player2Characters.slice(0);
        } else {
            newMatch.player1Characters = [];
            newMatch.player2Characters = [];
        }

        this.setState({ selectedMatch: newMatch });
    }
}