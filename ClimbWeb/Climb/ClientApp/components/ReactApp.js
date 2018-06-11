import * as React from "react";
import * as ReactDOM from "react-dom";
import { AppContainer } from "react-hot-loader";
import { SetPage } from "./SetPage";
function renderApp() {
    console.log("here");
    ReactDOM.render(React.createElement(AppContainer, null,
        React.createElement(SetPage, null)), document.getElementById("react-app"));
}
renderApp();
//# sourceMappingURL=ReactApp.js.map