import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IState {
    league: ClimbClient.League | null;
}

export class Home extends React.Component<RouteComponentProps<any>, IState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.onSubmit = this.onSubmit.bind(this);

        this.state = { league: null };
    }

    componentDidMount() {
        this.loadLeague();
    }

    render() {
        if (this.state.league == null) {
            return <RingLoader color={"#123abc"}/>;
        }

        return <h1>Welcome to {this.state.league.name}</h1>;
    }

    private onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const leaguesClient = new ClimbClient.LeagueClient(window.location.origin);
        const gameInput = (document.getElementById("gameInput") as HTMLSelectElement);
        const gameId = +gameInput.options[gameInput.selectedIndex].value;
        const leagueName = (document.getElementById("leagueNameInput") as HTMLInputElement).value;

        leaguesClient.create(gameId, leagueName)
            .then(game => {
                console.log(game);
                window.location.assign("/leagues");
            })
            .catch(reason => alert(`Could not create league!\n${reason}`));
    }

    private loadLeague() {
        const leagueClient = new ClimbClient.LeagueClient(window.location.origin);
        const leagueId = this.props.match.params.leagueId;
        leagueClient.get(leagueId)
            .then(league => {
                console.log(league);
                this.setState({ league: league });
            })
            .catch(reason => alert(`Could not load games\n${reason}`));
    }
}