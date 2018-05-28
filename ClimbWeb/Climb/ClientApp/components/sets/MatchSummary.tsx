import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchSummaryProps {
    game: ClimbClient.Game;
    match: ClimbClient.MatchDto;
    onSelect: (match: ClimbClient.MatchDto) => void;
}

export class MatchSummary extends React.Component<IMatchSummaryProps> {
    constructor(props: IMatchSummaryProps) {
        super(props);
    }

    render() {
        const match = this.props.match;
        const game = this.props.game;

        const p1Characters = match.player1Characters.map((c, i) => <span key={i}>{c}</span>);
        const p2Characters = match.player2Characters.map((c, i) => <span key={i}>{c}</span>);

        const stageView = this.renderStage(match, game);

        return (
            <div className="match-summary-container" onClick={() => this.props.onSelect(match)}>
                <div className="match-summary-index">Match {match.index + 1}</div>
                <div className="match-summary-info">
                    <div>{p1Characters}</div>
                    <div>{match.player1Score} - {match.player2Score}</div>
                    <div>{p2Characters}</div>
                </div>
                {stageView}
            </div>
        );
    }

    private renderStage(match: ClimbClient.MatchDto, game: ClimbClient.Game) {
        if (match.stageID == null) {
            return <div></div>
        }
        
        const stage = game.stages.find(s => s.id === match.stageID);
        if (!stage) throw new Error(`Could not find stage with ID '${match.stageID}'.`);
        const stageName = stage.name;

        return <div className="match-summary-stage">{stageName}</div>;
    }
}