import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ClimbClient } from "../../gen/climbClient";
import { RingLoader } from "react-spinners";

interface IUserHomeProps {
    userId: string;
}

interface IUserHomeState {
    user: ClimbClient.UserDto | undefined;
    profilePic: string | undefined;
}

class FileParameter implements ClimbClient.FileParameter {
    data: any;
    fileName: string;

    constructor(data: any, fileName: string) {
        this.data = data;
        this.fileName = fileName;
    }
}

export class Home extends React.Component<RouteComponentProps<IUserHomeProps>, IUserHomeState> {
    private readonly userClient: ClimbClient.UserClient;

    constructor(props: RouteComponentProps<IUserHomeProps>) {
        super(props);

        this.userClient = new ClimbClient.UserClient(window.location.origin);

        this.onUploadProfilePic = this.onUploadProfilePic.bind(this);

        this.state = {
            user: undefined,
            profilePic: undefined,
        }
    }

    componentDidMount() {
        this.loadUser();
    }

    render() {
        const user = this.state.user;
        if (!user) {
            return (
                <div id="loader">
                    <RingLoader color={"#123abc"}/>
                </div>
            );
        }

        const profilePic = this.state.profilePic;

        return (
            <div>
                <h2 id="subtitle">Home</h2>
                <div>Hello {user.username}</div>
                <img src={profilePic}/>

                <div>
                    <div className="input-group">
                        <label>New profile picture</label>
                        <input id="fileInput" type="file" accept="image/*"/>
                        <button onClick={this.onUploadProfilePic}>Upload</button>
                    </div>
                </div>
            </div>
        );
    }

    private loadUser() {
        const userId = this.props.match.params.userId;
        this.userClient.get(userId)
            .then(user => this.setState({
                user: user,
                profilePic: user.profilePic,
            }))
            .catch(reason => alert(`Could not load usern${reason}`));
    }

    private onUploadProfilePic() {
        if (!this.state.user) throw Error("User not loaded yet.");

        const id = this.state.user.id;
        const fileInput = (document.getElementById("fileInput") as HTMLInputElement);
        if (!fileInput || !fileInput.files) throw new Error("Could not find file input.");
        const file = fileInput.files[0];
        const fileParam = new FileParameter(file, file.name);

        this.userClient.uploadProfilePic(id, fileParam)
            .then(profilePicUrl => this.setState({ profilePic: profilePicUrl }))
            .catch(reason => `Could not upload profile picture\n${reason}`);
    }
}