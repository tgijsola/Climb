import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

interface IGameHomeState {
    game: ClimbClient.Game | null;
    characters: ClimbClient.Character[];
    stages: ClimbClient.Stage[];
}

export class Home extends React.Component<RouteComponentProps<any>, IGameHomeState> {
    private gameClient: ClimbClient.GameClient;
    private gameId: number;

    constructor(props: RouteComponentProps<any>) {
        super(props);

        this.gameClient = new ClimbClient.GameClient(window.location.origin);
        this.gameId = this.props.match.params.gameId;

        this.state = {
            game: null,
            characters: new Array<ClimbClient.Character>(),
            stages: new Array<ClimbClient.Stage>(),
        }

        this.onAddCharacter = this.onAddCharacter.bind(this);
        this.onAddStage = this.onAddStage.bind(this);
    }

    componentDidMount() {
        this.loadGame();
    }

    render() {
        const game = this.state.game;
        if (!game)
            return <div id="loader">
                       <RingLoader color={"#123abc"}/>
                   </div>

        const characters = this.state.characters.sort((a, b) => a.name.localeCompare(b.name)).map(c => <li key={c.id}>{c.name}</li>);
        const stages = this.state.stages.sort((a, b) => a.name.localeCompare(b.name)).map(s => <li key={s.id}>{s.name}</li>);

        return (
            <div>
                <h1>{game.name}</h1>
                <h4>Characters</h4>
                <div className="input-group">
                    <label>Character Name</label>
                    <input id="characterNameInput" name="characterNameInput" type="text"/>
                    <button onClick={this.onAddCharacter}>Add Character</button>
                </div>
                <ul>{characters}</ul>

                <h4>Stages</h4>
                <div className="input-group">
                    <label>Stage Name</label>
                    <input id="stageNameInput" name="stageNameInput" type="text"/>
                    <button onClick={this.onAddStage}>Add Stage</button>
                </div>
                <ul>{stages}</ul>
            </div>
        );
    }

    private loadGame() {
        this.gameClient.get(this.gameId)
            .then(game => this.setState({ game: game, characters: game.characters, stages: game.stages }))
            .catch(reason => alert(`Could not load game\n${reason}`));
    }

    private onAddCharacter() {
        const characterNameInput = (document.getElementById("characterNameInput") as HTMLInputElement);
        this.gameClient.addCharacter(this.gameId, characterNameInput.value)
            .then(character => {
                characterNameInput.value = "";
                const characters = this.state.characters;
                characters.push(character);
                this.setState({ characters: characters });
            })
            .catch(reason => alert(`Could not add character\n${reason}`));
    }
    
    private onAddStage() {
        const stageNameInput = (document.getElementById("stageNameInput") as HTMLInputElement);
        this.gameClient.addStage(this.gameId, stageNameInput.value)
            .then(stage => {
                stageNameInput.value = "";
                const stages = this.state.stages;
                stages.push(stage);
                this.setState({ stages: stages });
            })
            .catch(reason => alert(`Could not add stage\n${reason}`));
    }
}