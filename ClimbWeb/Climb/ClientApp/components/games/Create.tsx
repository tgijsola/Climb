import * as React from "react";
import { RouteComponentProps } from "react-router";

import { ClimbClient } from "../../gen/climbClient";

export class Create extends React.Component<RouteComponentProps<{}>> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.onSubmit = this.onSubmit.bind(this);
    }

    render() {
        return (
            <div>
                <form onSubmit={this.onSubmit}>
                    <input id="gameNameInput" type="text"/>
                    <button>Submit</button>
                </form>
            </div>
        );
    }

    private onSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const gameClient = new ClimbClient.GameClient(window.location.origin);
        const gameName = (document.getElementById("gameNameInput") as HTMLInputElement).value;

        gameClient.create(gameName)
            .then(game => {
                console.log(game);
                window.location.assign("/games");
            })
            .catch(reason => alert(`Could not create game!\n${reason}`));
    }
}