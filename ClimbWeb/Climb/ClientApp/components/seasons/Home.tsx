import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IState {
    season: ClimbClient.Season | null;
    league: ClimbClient.League | null;
}

export class Home extends React.Component<RouteComponentProps<any>, IState> {
    seasonClient: ClimbClient.SeasonClient;

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.seasonClient = new ClimbClient.SeasonClient(window.location.origin);

        this.state = {
            season: null,
            league: null,
        };
    }

    componentDidMount() {
        this.loadSeason();
    }

    render() {
        if (this.state.season == null || this.state.league == null) {
            return <RingLoader color={"#123abc"}/>;
        }

        return (
            <div>
                <h1>Welcome to {this.state.league.name + " Season " + (this.state.season.index + 1)}</h1>
            </div>
        );
    }

    private loadSeason() {
        const seasonId = this.props.match.params.seasonId;
        this.seasonClient.get(seasonId)
            .then(response => {
                console.log(response);
                if (response.season != null && response.league != null) {
                    this.setState({ season: response.season });
                    this.setState({ league: response.league });
                }
            })
            .catch(reason => alert(`Could not load season\n${reason}`));
    }
}