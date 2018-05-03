import * as React from "react";
import { RouteComponentProps } from "react-router";
import { Link } from "react-router-dom";

import { ClimbClient } from "../../gen/climbClient";

import { RingLoader } from "react-spinners";

import "bootstrap";

interface IIndexState {
    leagues: ClimbClient.Game[] | null
}

export class Index extends React.Component<RouteComponentProps<{}>, IIndexState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.state = { leagues: null };
    }

    componentDidMount() {
        this.loadleagues();
    }

    render() {
        let leagues: any;

        if (this.state.leagues != null) {
            if (this.state.leagues.length === 0) {
                leagues = <span>No leagues created yet!</span>;
            } else {
                leagues = this.state.leagues.map(
                    (league) => <li key={league.id}>
                                    <Link to={"/leagues/" +league.id}>{league.name}</Link>
                                </li>);
            }
        } else {
            leagues = <div></div>;
        }


        return (
            <div>
                <RingLoader
                    color={"#123abc"}
                    loading={this.state.leagues == null}/>

                <Link to={ "/leagues/create" } className={"btn btn-danger"} role={"button"}>Create New League</Link>

                {leagues}
            </div>
        );
    }

    private loadleagues() {
        const leagueClient = new ClimbClient.LeagueClient(window.location.origin);
        leagueClient.listAll()
            .then(leagues => {
                console.log(leagues);
                this.setState({ leagues: leagues });
            })
            .catch(reason => alert(`Could not load leagues!\n${reason}`));
    }
}