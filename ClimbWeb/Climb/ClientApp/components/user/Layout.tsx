import * as React from "react";
import { Switch, Route } from "react-router-dom";
import { Login } from "./Login";
import { Register } from "./Register";
import { Home } from "./Home";
import { Navbar } from "../_common/Navbar";
import { ClimbClient } from "../../gen/climbClient";
import ApplicationUser = ClimbClient.ApplicationUser;

interface ILayoutProps {
    user: ApplicationUser | null;
}

export class Layout extends React.Component<ILayoutProps> {
    render() {
        return (
            <div>
                <Navbar user={this.props.user}/>
                <div id="container">
                    <h1 id="title">Climb</h1>
                    <h2>User</h2>
                    <Switch>
                        <Route exact path="/user" component={ Login }/>
                        <Route exact path="/user/register" component={ Register }/>
                        <Route exact path="/user/:userId" component={ Home }/>
                    </Switch>
                </div>
            </div>
        );
    }
}