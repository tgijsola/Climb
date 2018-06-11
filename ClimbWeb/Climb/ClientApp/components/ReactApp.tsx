import * as React from "react";
import * as ReactDOM from "react-dom";
import { AppContainer } from "react-hot-loader";
import { SetPage } from "./SetPage";

function renderApp() {
    ReactDOM.render(
        <AppContainer>
            <SetPage />
        </AppContainer>,
        document.getElementById("react-app")
    );
}

renderApp();