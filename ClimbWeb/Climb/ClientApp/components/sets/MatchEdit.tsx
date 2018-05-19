import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchEditProps {
    match: ClimbClient.MatchDto;
    onCancel: () => void;
}

export class MatchEdit extends React.Component<IMatchEditProps> {
    constructor(props: IMatchEditProps) {
        super(props);
    }

    render() {
        const match = this.props.match;

        return (
            <div>
                <span>Match {match.index + 1}</span>
                <button onClick={this.props.onCancel}>Cancel</button>
            </div>
        );
    }


}