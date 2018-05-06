import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IState {
    season: ClimbClient.SeasonDto | null;
}

export class Home extends React.Component<RouteComponentProps<any>, IState> {
    seasonClient: ClimbClient.SeasonClient;

    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.seasonClient = new ClimbClient.SeasonClient(window.location.origin);

        this.state = {
            season: null
        };
        this.startSeason = this.startSeason.bind(this);
    }

    componentDidMount() {
        this.loadSeason();
    }

    render() {
        if (this.state.season == null) {
            return <RingLoader color={"#123abc"}/>;
        }

        let body: any;
        if (this.state.season.sets != null) {
            const sets = this.state.season.sets.map((s, i) => <li key={i}>{`set ${i}`}</li>);
            body = <ul>{sets}</ul>;
        } else {
            body = <button onClick={this.startSeason}>Start</button>;
        }

        return (
            <div>
                <h1>Welcome to {this.state.season.leagueName + " Season " + (this.state.season.index + 1)}</h1>
                {body}
            </div>
        );
    }

    private loadSeason() {
        const seasonId = this.props.match.params.seasonId;
        this.seasonClient.get(seasonId)
            .then(response => {
                console.log(response);
                this.setState({ season: response });
            })
            .catch(reason => alert(`Could not load season\n${reason}`));
    }

    private startSeason() {
        if (this.state.season == null) return;

        const seasonId = this.state.season.id;

        this.seasonClient.start(seasonId)
            .then(sets => {
                console.log(sets);
                //this.setState({ sets: sets });
            })
            .catch(reason => alert(`Could not start season\n${reason}`));
    }
}