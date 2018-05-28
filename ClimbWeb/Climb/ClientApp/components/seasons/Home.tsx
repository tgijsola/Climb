import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

import { SetDetails } from "../sets/SetDetails";

interface IState {
    season: ClimbClient.Season | undefined;
    league: ClimbClient.League | undefined;
    participants: ClimbClient.LeagueUser[] | undefined;
    sets: ClimbClient.Set[] | undefined;
}

export class Home extends React.Component<RouteComponentProps<any>, IState> {
    seasonClient: ClimbClient.SeasonClient;
    leagueClient: ClimbClient.LeagueClient;

    constructor(props: RouteComponentProps<any>) {
        super(props);

        this.seasonClient = new ClimbClient.SeasonClient(window.location.origin);
        this.leagueClient = new ClimbClient.LeagueClient(window.location.origin);

        this.startSeason = this.startSeason.bind(this);

        this.state = {
            season: undefined,
            league: undefined,
            participants: undefined,
            sets: undefined,
        }
    }

    componentDidMount() {
        this.loadSeason();
    }

    render() {
        if (this.state.season == null ||
            this.state.league == null ||
            this.state.participants == null ||
            this.state.sets == null) {
            return <RingLoader color={"#123abc"}/>;
        }

        let body: any;
        if (this.state.sets.length !== 0) {
            const participants = this.state.participants;
            const sets =
                this.state.sets.map(
                    (s, i) => <SetDetails key={i}
                                          set={s}
                                          player1={participants.find(lu => lu.id === s.player1ID)}
                                          player2={participants.find(lu => lu.id === s.player2ID)}/>);
            body = <div>{sets}</div>;
        } else {
            body = <button onClick={this.startSeason}>Start</button>;
        }

        return (
            <div>
                <h1>Welcome to {this.state.league.name + " Season " + (this.state.season.index + 1)}</h1>
                {body}
            </div>
        );
    }

    private loadSeason() {
        const seasonId = this.props.match.params.seasonId;
        this.seasonClient.get(seasonId)
            .then(season => {
                this.setState({ season: season });

                this.leagueClient.get(season.leagueID)
                    .then(league => this.setState({ league: league }))
                    .catch(reason => alert(`Could not load league\n${reason}`));

                this.seasonClient.sets(seasonId)
                    .then(sets => this.setState({ sets: sets }))
                    .catch(reason => alert(`Could not load sets\n${reason}`));

                this.seasonClient.participants(seasonId)
                    .then(participants => this.setState({ participants: participants }))
                    .catch(reason => alert(`Could not load participants\n${reason}`));
            })
            .catch(reason => alert(`Could not load season\n${reason}`));
    }

    private startSeason() {
        if (this.state.season == null) return;

        const seasonId = this.state.season.id;

        this.seasonClient.start(seasonId)
            .then(sets => this.setState({ sets: sets }))
            .catch(reason => alert(`Could not start season\n${reason}`));
    }
}