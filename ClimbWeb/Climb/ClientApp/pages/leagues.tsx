import * as React from "react";
import * as ReactDOM from "react-dom";
import { AppContainer } from "react-hot-loader";
import { BrowserRouter } from "react-router-dom";

import { Layout } from "../components/leagues/Layout";

import "../css/site.less";
import "../css/pages/games.less";

function renderApp() {
    const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href")!;
    ReactDOM.render(
        <AppContainer>
            <BrowserRouter basename={ baseUrl }>
                <div>
                    <Layout user={null} />
                </div>
            </BrowserRouter>
        </AppContainer>,
        document.getElementById("react-app")
    );
}

renderApp();