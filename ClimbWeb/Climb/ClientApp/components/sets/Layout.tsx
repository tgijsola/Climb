import * as React from "react";
import { Route } from "react-router-dom";

import { ClimbClient } from "../../gen/climbClient";
import ApplicationUser = ClimbClient.ApplicationUser;

import { Navbar } from "../_common/Navbar";
import { Submit } from "./Submit";

interface ILayoutProps {
    user : ApplicationUser | null;
}

export class Layout extends React.Component<ILayoutProps> {
    render() {
        return (
            <div id="container">
                <Navbar user={this.props.user}/>
                <Route exact path="/sets/:setId" component={ Submit } />
            </div>
        );
    }
}