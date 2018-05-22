import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IGameHomeState {
    game: ClimbClient.Game | null;
}

export class Home extends React.Component<RouteComponentProps<any>, IGameHomeState> {
    private gameClient: ClimbClient.GameClient;
    private gameId: number;

    constructor(props: RouteComponentProps<any>) {
        super(props);

        this.gameClient = new ClimbClient.GameClient(window.location.origin);
        this.gameId = this.props.match.params.gameId;

        this.state = {
            game: null,
        }
    }

    componentDidMount() {
        this.loadGame();
    }

    render() {
        const game = this.state.game;
        if (!game) return <div id="loader"><RingLoader color={"#123abc"}/></div>

        const characters = game.characters.map(c => <li key={c.id}>{c.name}</li>);
        const stages = game.stages.map(s => <li key={s.id}>{s.name}</li>);

        return (
            <div>
                <h1>{game.name}</h1>
                <h4>Characters</h4>
                <ul>{characters}</ul>
                <h4>Stages</h4>
                <ul>{stages}</ul>
            </div>
        );
    }

    private loadGame() {
        this.gameClient.get(this.gameId)
            .then(game => this.setState({ game: game }))
            .catch(reason => alert(`Could not load game\n${reason}`));
    }
}