import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchSummaryProps {
    match: ClimbClient.MatchDto;
    onSelect: (match: ClimbClient.MatchDto) => void;
}

export class MatchSummary extends React.Component<IMatchSummaryProps> {
    constructor(props: IMatchSummaryProps) {
        super(props);
    }

    render() {
        const match = this.props.match;

        if (match.player1Characters == null) return null;
        if (match.player2Characters == null) return null;

        const p1Characters = match.player1Characters.map((c, i) => <span key={i}>{c}</span>);
        const p2Characters = match.player2Characters.map((c, i) => <span key={i}>{c}</span>);

        return (
            <div className="match-summary-container" onClick={() => this.props.onSelect(match)}>
                <div className="match-summary-index">Match {match.index + 1}</div>
                <div className="match-summary-info">
                    <div>{p1Characters}</div>
                    <div>{match.player1Score} - {match.player2Score}</div>
                    <div>{p2Characters}</div>
                </div>
            </div>
        );
    }
}