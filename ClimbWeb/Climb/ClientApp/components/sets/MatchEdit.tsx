import * as React from "react";

import { ClimbClient } from "../../gen/climbClient";

interface IMatchEditProps {
    game: ClimbClient.Game;
    match: ClimbClient.MatchDto;
    onEdit: (match: ClimbClient.MatchDto) => void;
    onCancel: () => void;
    onDelete: () => void;
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
        const game = this.props.game;

        if (!game.characters || !game.stages) throw new Error();
        if (!match.player1Characters || !match.player2Characters) throw new Error();

        const characters = game.characters.map(c => <option key={c.id} value={c.id}>{c.name}</option>);
        const stages = game.stages.map(s => <option key={s.id} value={s.id}>{s.name}</option>)
        const canOk = match.player1Score != match.player2Score;

        return (
            <div id="match-edit-container">
                <button onClick={this.props.onDelete}>Delete</button>
                <div id="match-edit-title">Match {match.index + 1}</div>
                {this.renderPlayerInputs(game, 1, characters, match.player1Characters)}
                <div className="match-edit-input-group-divider"></div>
                {this.renderPlayerInputs(game, 2, characters, match.player2Characters)}
                <div className="match-edit-input-group-divider"></div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Stage</div>
                    <div>
                        <select className="match-edit-input" value={match.stageID} onChange={(e: any) =>
                            this.updateStage(parseInt(e.currentTarget.value))}>{stages}</select>
                    </div>
                </div>
                <div className="match-edit-buttons">
                    <button onClick={this.props.onCancel}>Cancel</button>
                    <button disabled={!canOk} onClick={() => this.props.onEdit(this.state.match)}>Ok</button>
                </div>
            </div>
        );
    }

    private renderPlayerInputs(game: ClimbClient.Game,
        playerNumber: number,
        characters: any,
        characterValues: number[]) {
        const match = this.state.match;
        const score = playerNumber === 1 ? match.player1Score : match.player2Score;

        const characterInputs = [];
        for (let i = 0; i < game.charactersPerMatch; i++) {
            characterInputs.push(
                <select className="match-edit-input"
                        value={characterValues[i]}
                        onChange={(e: any) => this.updateCharacter(playerNumber, i, parseInt(e.currentTarget.value))}>
                    {characters}
                </select>);
        }

        return (
            <div>
                <div className="match-edit-player-title">Player {playerNumber}</div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">Score</div>
                    <div>
                        <input className="match-edit-input"
                            type="number"
                            value={score}
                            min="0" max={game.maxMatchPoints}
                            onChange={(e: any) => this.updateScore(playerNumber, parseInt(e.currentTarget.value))}/>
                    </div>
                </div>
                <div className="match-edit-input-group">
                    <div className="match-edit-input-label">{`Character${game.charactersPerMatch > 1 ? 's' : ''}`}</div>
                    <div className="match-edit-characters">{characterInputs}</div>
                </div>
            </div>
        );
    }

    private updateScore(playerNumber: number, score: number) {
        const match = this.state.match;
        if (playerNumber === 1) {
            match.player1Score = score;
        } else {
            match.player2Score = score;
        }

        this.setState({ match: match });
    }

    private updateCharacter(playerNumber: number, characterIndex: number, characterID: number) {
        const match = this.state.match;

        const characters = playerNumber === 1 ? match.player1Characters : match.player2Characters;
        if (characters == null) throw new Error();
        characters[characterIndex] = characterID;

        this.setState({ match: match });
    }

    private updateStage(stageID: number) {
        const match = this.state.match;
        match.stageID = stageID;
        this.setState({ match: match });
    }
}