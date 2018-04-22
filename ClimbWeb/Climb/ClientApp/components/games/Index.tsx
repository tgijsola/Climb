import * as React from "react";
import { RouteComponentProps } from "react-router";
import { Link } from "react-router-dom";

import { ClimbClient } from "../../gen/climbClient";

import { RingLoader } from "react-spinners";

import "bootstrap";

interface IIndexState {
    games: ClimbClient.Game[] | null
}

export class Index extends React.Component<RouteComponentProps<{}>, IIndexState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.state = { games: null };
    }

    componentDidMount() {
        this.loadGames();
    }

    render() {
        let games: any;

        if (this.state.games != null) {
            if (this.state.games.length === 0) {
                games = <span>No games created yet!</span>;
            } else {
                games = this.state.games.map((game) => <li key={game.id}>{game.name}</li>);
            }
        } else {
            games = <div></div>;
        }


        return (
            <div>
                <RingLoader
                    color={"#123abc"}
                    loading={this.state.games == null}/>
                
                <Link to={ "/games/create" } className={"btn btn-danger"} role={"button"} >Create New Game</Link>

                {games}
            </div>
        );
    }

    private loadGames() {
        const gameClient = new ClimbClient.GameClient("http://192.168.196.1:45455");
        gameClient.listAll()
            .then(games => {
                console.log(games);
                this.setState({ games: games });
            })
            .catch(reason => alert(`Could not load games!\n${reason}`));
    }
}