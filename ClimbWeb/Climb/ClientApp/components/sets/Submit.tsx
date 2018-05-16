import * as React from "react";
import { RouteComponentProps } from "react-router";
import { RingLoader } from "react-spinners";

import { ClimbClient } from "../../gen/climbClient";

export class Submit extends React.Component<RouteComponentProps<any> | undefined> {

    constructor(props: RouteComponentProps<any> | undefined) {
        super(props);

     
    }

    componentDidMount() {
    }

    render() {
        return <div>Welcome to set {this.props.match.params.setId}</div>
    }

}