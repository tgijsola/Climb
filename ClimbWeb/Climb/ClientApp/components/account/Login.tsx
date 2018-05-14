import * as React from "react";
import { RouteComponentProps } from "react-router";
import { Link } from "react-router-dom";

import { ClimbClient } from "../../gen/climbClient";
import AccountClient = ClimbClient.AccountClient;

import "bootstrap";

interface ILoginState {
    isLoggingIn: boolean;
}

export class Login extends React.Component<RouteComponentProps<any> | undefined, ILoginState> {
    constructor(props: RouteComponentProps<any> | undefined) {
        super(props);

        this.onLogInClick = this.onLogInClick.bind(this);
        this.onLogInSuccess = this.onLogInSuccess.bind(this);
        this.onLogInFail = this.onLogInFail.bind(this);

        this.state = { isLoggingIn: false };
    }

    render() {
        if (this.state.isLoggingIn) {
            return <h1>Logging in...</h1>;
        }

        return (
            <form id="signin-form" onSubmit={this.onLogInClick}>
                <img className="mb-4" src="/images/logo-128x128.png" alt="" width="128" height="128"/>
                <h1 className="h3 mb-3 font-weight-normal">Log In</h1>
                <input type="email" id="inputEmail" className="form-control" placeholder="Email address" required/>
                <input type="password" id="inputPassword" className="form-control" placeholder="Password" required/>
                <div className="mb-3">
                    <label>
                        <input type="checkbox" id="inputRememberMe" value="remember-me"/> Remember me
                    </label>
                </div>
                <button id="signin-button" className="btn btn-lg btn-block" type="submit">Sign in</button>
                <Link to={ "/account/register" }>Register</Link>
            </form>
        );
    }

    private onLogInClick(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const email = (document.getElementById("inputEmail") as HTMLInputElement).value;
        const password = (document.getElementById("inputPassword") as HTMLInputElement).value;
        const rememberMe = (document.getElementById("inputRememberMe") as HTMLInputElement).checked;

        const accountClient = new AccountClient(window.location.origin);
        accountClient.logIn(email, password, rememberMe)
            .then(this.onLogInSuccess)
            .catch(this.onLogInFail);
    }

    private onLogInSuccess(response: ClimbClient.LoginResponse) {
        if (response.token != null) {
            localStorage.setItem("jwt", response.token);
        }
    }

    private onLogInFail(reason: any) {
        alert("There was an error logging in!");
    }
}