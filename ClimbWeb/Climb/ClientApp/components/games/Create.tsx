import * as React from "react";
import { RouteComponentProps } from "react-router";

import { ClimbClient } from "../../gen/climbClient";

export class Create extends React.Component<RouteComponentProps<any> | undefined> {
    constructor(props: RouteComponentProps<any> | undefined) {
        super(props);

        this.onSubmit = this.onSubmit.bind(this);
    }

    render() {
        return (
            <div>
                <form onSubmit={this.onSubmit}>
                    <div className="input-group">
                        <label>Name</label>
                        <input id="gameNameInput" type="text"/>
                    </div>
                    <div className="input-group">
                        <label>Characters per match</label>
                        <input id="charactersPerMatchInput" type="number" min="1" value="1"/>
                    </div>
                    <div className="input-group">
                        <label>Max points per match</label>
                        <input id="maxMatchPointsInput" type="number" min="1" value="2"/>
                    </div>
                    <button>Submit</button>
                </form>
            </div>
        );
    }

    private onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const gameClient = new ClimbClient.GameClient(window.location.origin);
        const gameName = (document.getElementById("gameNameInput") as HTMLInputElement).value;
        const charactersPerMatch = (document.getElementById("charactersPerMatchInput") as HTMLInputElement).valueAsNumber;
        const maxMatchPoints = (document.getElementById("maxMatchPointsInput") as HTMLInputElement).valueAsNumber;

        gameClient.create(gameName, charactersPerMatch, maxMatchPoints)
            .then(game => {
                console.log(game);
                window.location.assign("/games");
            })
            .catch(reason => alert(`Could not create game!\n${reason}`));
    }
}