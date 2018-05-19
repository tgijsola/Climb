import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchEditProps {
    game: ClimbClient.Game;
    match: ClimbClient.MatchDto;
    onEdit: (match: ClimbClient.MatchDto) => void;
    onCancel: () => void;
}

interface IMatchEditState {
    match: ClimbClient.MatchDto;
}

export class MatchEdit extends React.Component<IMatchEditProps, IMatchEditState> {
    constructor(props: IMatchEditProps) {
        super(props);

        const match = this.props.match;
        const matchToEdit = new ClimbClient.MatchDto(match);

        if (!match.player1Characters || !match.player2Characters) throw new Error();
        matchToEdit.player1Characters = match.player1Characters.slice(0);
        matchToEdit.player2Characters = match.player2Characters.slice(0);

        this.state = {
            match: matchToEdit,
        }
    }

    render() {
        const match = this.state.match;

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
                        <select className="match-edit-input" value={match.stageID} onChange={e => this.updateStage(parseInt(e.currentTarget.value))}>{stages}</select>
                    </div>
                </div>
                <div className="match-edit-buttons">
                    <button onClick={this.props.onCancel}>Cancel</button>
                    <button onClick={() => this.props.onEdit(this.state.match)}>Ok</button>
                </div>
            </div>
        );
    }

    private renderPlayerInputs(playerNumber: number, characters: any, characterValues: number[]) {
        const match = this.state.match;
        const score = playerNumber === 1 ? match.player1Score : match.player2Score;

        return (
            <div>
                <div className="match-edit-player-title">Player {playerNumber}</div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Score</div>
                    <div>
                        <input className="match-edit-input" type="number" value={score} min="0" max="2" onChange={e => this.updateScore(playerNumber, parseInt(e.currentTarget.value))}/>
                    </div>
                </div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Characters</div>
                    <div className="match-edit-characters">
                        <select className="match-edit-input" value={characterValues[0]} onChange={e => this.updateCharacter(playerNumber, 0, parseInt(e.currentTarget.value))}>{characters}</select>
                        <select className="match-edit-input" value={characterValues[1]} onChange={e => this.updateCharacter(playerNumber, 1, parseInt(e.currentTarget.value))}>{characters}</select>
                        <select className="match-edit-input" value={characterValues[2]} onChange={e => this.updateCharacter(playerNumber, 2, parseInt(e.currentTarget.value))}>{characters}</select>
                    </div>
                </div>
            </div>
        );
    }

    private updateScore(playerNumber: number, score: number) {
        let match = this.state.match;
        if (playerNumber === 1) {
            match.player1Score = score;
        } else {
            match.player2Score = score;
        }

        this.setState({match: match});
    }

    private updateCharacter(playerNumber: number, characterIndex: number, characterID: number) {
        let match = this.state.match;

        let characters = playerNumber === 1 ? match.player1Characters : match.player2Characters;
        if (characters == null) throw new Error();
        characters[characterIndex] = characterID;

        this.setState({match: match});
    }

    private updateStage(stageID: number) {
        let match = this.state.match;
        match.stageID = stageID;
        this.setState({match: match});
    }
}