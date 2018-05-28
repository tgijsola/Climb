import * as React from "react";
import { Link } from "react-router-dom";

import "../../css/setDetails.less";
import { ClimbClient } from "../../gen/climbClient";

interface ISetDetailsProps {
    set: ClimbClient.Set;
    player1: ClimbClient.LeagueUser | undefined;
    player2: ClimbClient.LeagueUser | undefined;
}

export class SetDetails extends React.Component<ISetDetailsProps> {
    render() {
        if (!this.props.player1 || !this.props.player2) return <div></div>;

        const set = this.props.set;
        return (
            <div className="set-details-container">
                <span>{`Set ${set.id}`}</span>
                <span>{`${this.props.player1.id} v ${this.props.player2.id}`}</span>
                <a className="btn btn-primary" role="button" href={"/sets/" +set.id}>Fight</a>
            </div>
        );
    }
}