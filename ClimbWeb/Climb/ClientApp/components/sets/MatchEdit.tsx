import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchEditProps {
    game: ClimbClient.Game;
    match: ClimbClient.MatchDto;
    onDone: () => void;
}

export class MatchEdit extends React.Component<IMatchEditProps> {
    constructor(props: IMatchEditProps) {
        super(props);
    }

    render() {
        const match = this.props.match;

        if (this.props.game.characters == null || this.props.game.stages == null) throw new Error();
        if (match.player1Characters == null || match.player2Characters == null) throw new Error();

        const characters = this.props.game.characters.map(c => <option key={c.id} value={c.id}>{c.name}</option>);
        const stages = this.props.game.stages.map(s => <option key={s.id} value={s.id}>{s.name}</option>)

        return (
            <div id="match-edit-container">
                <div id="match-edit-title">Match {match.index + 1}</div>
                {this.renderPlayerInputs(1, characters, match.player1Characters)}
                <div className="match-edit-input-group-divider"></div>
                {this.renderPlayerInputs(2, characters, match.player2Characters)}
                <div className="match-edit-input-group-divider"></div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Stage</div>
                    <div>
                        <select className="match-edit-input">{stages}</select>
                    </div>
                </div>
                <div className="match-edit-buttons">
                    <button onClick={this.props.onDone}>Cancel</button>
                    <button onClick={this.onAccept}>Ok</button>
                </div>
            </div>
        );
    }

    private renderPlayerInputs(playerNumber: number, characters: any, characterValues: number[]) {
        return (
            <div>
                <div className="match-edit-player-title">Player {playerNumber}</div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Score</div>
                    <div>
                        <input className="match-edit-input" type="number"/>
                    </div>
                </div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Characters</div>
                    <div className="match-edit-characters">
                        <select className="match-edit-input" value={characterValues[0]}>{characters}</select>
                        <select className="match-edit-input" value={characterValues[1]}>{characters}</select>
                        <select className="match-edit-input" value={characterValues[2]}>{characters}</select>
                    </div>
                </div>
            </div>
        );
    }

    private onAccept() {
        // need to copy match at start and then assign it here


        this.props.onDone();
    }
}