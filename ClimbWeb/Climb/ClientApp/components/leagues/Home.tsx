import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IState {
    league: ClimbClient.League | null;
    seasons: ClimbClient.Season[] | null;
}

export class Home extends React.Component<RouteComponentProps<any>, IState> {
    leagueClient: ClimbClient.LeagueClient;

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.leagueClient = new ClimbClient.LeagueClient(window.location.origin);

        this.onSubmit = this.onSubmit.bind(this);
        this.createSeason = this.createSeason.bind(this);

        this.state = {
            league: null,
            seasons: null,
        };
    }

    componentDidMount() {
        this.loadLeague();
    }

    render() {
        if (this.state.league == null) {
            return <RingLoader color={"#123abc"}/>;
        }

        const seasons = this.state.seasons == null
            ? <div></div>
            : this.state.seasons.map(s => <li>Season {s.index + 1}</li>);

        return (
            <div>
                <h1>Welcome to {this.state.league.name}</h1>
                <form onSubmit={this.createSeason}>
                    <div className="form-group">
                        <label>
                            Start Date <input id="startInput" type="date"/>
                        </label>
                    </div>
                    <div className="form-group">
                        <label>
                            End Date <input id="endInput" type="date"/>
                        </label>
                    </div>
                    <button>Create Season</button>
                </form>
                <h2>Season</h2>
                <ul>{seasons}</ul>
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
                console.log(game);
                window.location.assign("/leagues");
            })
            .catch(reason => alert(`Could not create league!\n${reason}`));
    }

    private loadLeague() {
        const leagueId = this.props.match.params.leagueId;
        this.leagueClient.get(leagueId)
            .then(league => {
                console.log(league);
                this.setState({ league: league });
                this.loadSeasons();
            })
            .catch(reason => alert(`Could not load games\n${reason}`));
    }

    private loadSeasons() {
        if (this.state.league == null) {
            alert("Page not loaded!");
            return;
        }

        this.leagueClient.getSeasons(this.state.league.id)
            .then(seasons => {
                console.log(seasons);
                this.setState({ seasons: seasons });
            })
            .catch(reason => alert(`Could not load seasons\n${reason}`));
    }

    private createSeason(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (this.state.league == null) {
            alert("Page not loaded!");
            return;
        }

        const seasonClient = new ClimbClient.SeasonClient(window.location.origin);
        const leagueId = this.state.league.id;
        const startInput = (document.getElementById("startInput") as HTMLInputElement).valueAsDate;
        const endInput = (document.getElementById("endInput") as HTMLInputElement).valueAsDate;

        seasonClient.create(leagueId, startInput, endInput)
            .then(season => {
                console.log(season);
                this.loadSeasons();
            })
            .catch(reason => {
                alert(reason);
            });
    }
}