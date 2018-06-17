import * as React from "react";
import * as ReactDOM from "react-dom";
import { AppContainer } from "react-hot-loader";
import { Submit } from "./Submit";

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <Submit />
        </AppContainer>,
        document.getElementById("react-app")
    );
}

renderApp();