import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IState {
    games: ClimbClient.Game[] | null;
}

export class Create extends React.Component<RouteComponentProps<any> | undefined, IState> {
    constructor(props: RouteComponentProps<any> | undefined) {
        super(props);

        this.onSubmit = this.onSubmit.bind(this);

        this.state = { games: null };
    }

    componentDidMount() {
        this.loadGames();
    }

    render() {
        if (this.state.games == null) {
            return <RingLoader color={"#123abc"} />;
        }

        const games = this.state.games.map(game => <option key={game.id} value={game.id}>{game.name}</option>);

        return (
            <div>
                <form onSubmit={this.onSubmit}>
                    <select id="gameInput">
                        {games}
                    </select>
                    <input id="leagueNameInput" type="text"/>
                    <button>Submit</button>
                </form>
            </div>
        );
    }

    private onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const leaguesClient = new ClimbClient.LeagueClient(window.location.origin);
        const gameInput = (document.getElementById("gameInput") as HTMLSelectElement);
        const gameId = +gameInput.options[gameInput.selectedIndex].value;
        const leagueName = (document.getElementById("leagueNameInput") as HTMLInputElement).value;

        leaguesClient.create(gameId, leagueName)
            .then(game => {
                window.location.assign("/leagues");
            })
            .catch(reason => alert(`Could not create league!\n${reason}`));
    }

    private loadGames() {
        const gameClient = new ClimbClient.GameClient(window.location.origin);
        gameClient.listAll()
            .then(games => {
                this.setState({ games: games });
            })
            .catch(reason => alert(`Could not load games\n${reason}`));
    }
}