import * as React from "react";
import { RouteComponentProps } from "react-router";
import { Link } from "react-router-dom";
import { ClimbClient } from "../../gen/climbClient";
import AccountClient = ClimbClient.AccountClient;

interface IRegisterState {
    user: ClimbClient.ApplicationUser | null;
}

export class Register extends React.Component<RouteComponentProps<{}>, IRegisterState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.onRegisterClick = this.onRegisterClick.bind(this);
        this.onRegisterSuccess = this.onRegisterSuccess.bind(this);
        this.onRegisterFail = this.onRegisterFail.bind(this);

        this.state = { user: null };
    }

    public render() {
        if (this.state.user != null) {
            return <h1>Hello {this.state.user.userName}</h1>;
        }

        return (
            <form id="signin-form" onSubmit={this.onRegisterClick}>
                <img className="mb-4" src="/images/logo-128x128.png" alt="" width="128" height="128"/>
                <h1 className="h3 mb-3 font-weight-normal">Register</h1>
                <input type="email" id="inputEmail" className="form-control" placeholder="Email address" required/>
                <input type="password" id="inputPassword" className="form-control" placeholder="Password" required/>
                <input type="password" id="inputConfirm" className="form-control" placeholder="Confirm Password" required/>
                <div className="mb-3">
                    <label>
                        <input type="checkbox" id="inputRememberMe" value="remember-me"/> Remember me
                    </label>
                </div>
                <button id="signin-button" className="btn btn-lg btn-block" type="submit">Register</button>
                <Link to={ "/account" }>Log In</Link>
            </form>
        );
    }

    private onRegisterClick(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const email = (document.getElementById("inputEmail") as HTMLInputElement).value;
        const password = (document.getElementById("inputPassword") as HTMLInputElement).value;
        const confirm = (document.getElementById("inputConfirm") as HTMLInputElement).value;

        const accountClient = new AccountClient(window.location.origin);
        accountClient.register(email, password, confirm)
            .then(this.onRegisterSuccess)
            .catch(this.onRegisterFail);
    }

    private onRegisterSuccess(user: ClimbClient.ApplicationUser | null) {
        this.setState({ user: user });
    }

    private onRegisterFail(reason: any) {
        alert("There was an error registering!");
    }
}