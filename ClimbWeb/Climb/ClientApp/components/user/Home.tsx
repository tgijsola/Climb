import * as React from "react";
import { RouteComponentProps } from "react-router";
import { ClimbClient } from "../../gen/climbClient";
import { RingLoader } from "react-spinners";

interface IUserHomeProps {
    userId: string;
}

interface IUserHomeState {
    user: ClimbClient.UserDto | undefined;
}

export class Home extends React.Component<RouteComponentProps<IUserHomeProps>, IUserHomeState> {
    private readonly userClient: ClimbClient.UserClient;

    constructor(props: RouteComponentProps<IUserHomeProps>) {
        super(props);

        this.userClient = new ClimbClient.UserClient(window.location.origin);

        this.state = {
            user: undefined,
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

        return (
            <div>
                <h2 id="subtitle">Home</h2>
                <div>Hello {user.username}</div>
                <img src={user.profilePic}/>
            </div>
        );
    }

    private loadUser() {
        const userId = this.props.match.params.userId;
        this.userClient.get(userId)
            .then(user => this.setState({ user: user }))
            .catch(reason => alert(`Could not load usern${reason}`));
    }
}