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
        this.onMatchEdited = this.onMatchEdited.bind(this);
    }

    componentDidMount() {
        this.loadSet();
    }

    render() {
        if (!this.state.set || !this.state.set.matches || !this.state.game) {
            return <RingLoader color={"#123abc"}/>;
        }

        if (this.state.selectedMatch != null) {
            return <MatchEdit
                       match={this.state.selectedMatch}
                       game={this.state.game}
                       onDone={this.onMatchEdited}/>;
        }

        const matches =
            this.state.set.matches.map(
                (m, i) => <MatchSummary key={i} match={m} onSelect={m => this.setState({ selectedMatch: m })}/>);

        return (
            <div>
                <div>{matches}</div>
                <button onClick={this.onSubmit}>Submit</button>
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

        let setRequest = new ClimbClient.SubmitRequest();
        setRequest.setID = set.id;
        setRequest.matches = new Array<ClimbClient.MatchForm>(set.matches.length);

        for (var i = 0; i < set.matches.length; i++) {
            let matchForm = new ClimbClient.MatchForm();
            matchForm.init(set.matches[i]);
            setRequest.matches[i] = matchForm;
        }

        this.client.submit(setRequest)
            .then(() => {
                console.log("Set submitted!");
                window.location.reload();
            })
            .catch(reason => alert(`Could not submit set\n${reason}`));
    }
}