﻿import * as React from "react";

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

// TODO: Show errors
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

        if (!game.characters) throw new Error();
        if (!match.player1Characters || !match.player2Characters) throw new Error();

        const characters = game.characters.map(c => <option key={c.id} value={c.id}>{c.name}</option>);
        const stageInput = this.renderStageInput(game, match);
        const canOk = this.canEditMatch(game, match);

        return (
            <div id="match-edit-container">
                <button onClick={this.props.onDelete}>Delete</button>
                <div id="match-edit-title">Match {match.index + 1}</div>
                {this.renderPlayerInputs(game, 1, characters, match.player1Characters)}
                <div className="match-edit-input-group-divider"></div>
                {this.renderPlayerInputs(game, 2, characters, match.player2Characters)}
                <div className="match-edit-input-group-divider"></div>
                {stageInput}
                <div className="match-edit-buttons">
                    <button onClick={this.props.onCancel}>Cancel</button>
                    <button disabled={!canOk} onClick={() => this.props.onEdit(this.state.match)}>Ok</button>
                </div>
            </div>
        );
    }

    private canEditMatch(game: ClimbClient.Game, match: ClimbClient.MatchDto): boolean {
        if (!match.player1Characters || !match.player2Characters) throw new Error();

        const differentScores = match.player1Score != match.player2Score;
        const aPlayerHasMaxScore = match.player1Score == game.maxMatchPoints || match.player2Score == game.maxMatchPoints;
        const differentCharacters = this.checkUniqueCharacters(match.player1Characters) && this.checkUniqueCharacters(match.player2Characters);
        return differentScores && aPlayerHasMaxScore && differentCharacters;
    }

    private checkUniqueCharacters(characters: number[]): boolean {
        if (characters.length === 1) return true;

        for (var i = 0; i < characters.length - 1; i++) {
            for (var j = i + 1; j < characters.length; j++) {
                if (characters[i] === characters[j]) return false;
            }
        }

        return true;
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
                        key={i}
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
                    <div className="match-edit-input-label">{`Character${game.charactersPerMatch > 1 ? "s" : ""}`
                    }</div>
                    <div className="match-edit-characters">{characterInputs}</div>
                </div>
            </div>
        );
    }

    private renderStageInput(game: ClimbClient.Game, match: ClimbClient.MatchDto) {
        if (!game.stages || game.stages.length === 0) {
            return null;
        }

        const stages = game.stages.map(s => <option key={s.id} value={s.id}>{s.name}</option>);

        return (
            <div className="match-edit-input-group">
                <div className="match-edit-input-label">Stage</div>
                <div>
                    <select className="match-edit-input" value={match.stageID} onChange={(e: any) =>
                        this.updateStage(parseInt(e.currentTarget.value))}>{stages}</select>
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