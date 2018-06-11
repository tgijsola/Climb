System.register(["react", "react-dom", "react-hot-loader", "react-router-dom", "./SetPage"], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    function renderApp() {
        // This code starts up the React app when it runs in a browser. It sets up the routing
        // configuration and injects the app into a DOM element.
        const baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
        ReactDOM.render(React.createElement(react_hot_loader_1.AppContainer, null,
            React.createElement(react_router_dom_1.BrowserRouter, { basename: baseUrl },
                React.createElement(SetPage_1.SetPage, null))), document.getElementById("react-app"));
    }
    var React, ReactDOM, react_hot_loader_1, react_router_dom_1, SetPage_1;
    return {
        setters: [
            function (React_1) {
                React = React_1;
            },
            function (ReactDOM_1) {
                ReactDOM = ReactDOM_1;
            },
            function (react_hot_loader_1_1) {
                react_hot_loader_1 = react_hot_loader_1_1;
            },
            function (react_router_dom_1_1) {
                react_router_dom_1 = react_router_dom_1_1;
            },
            function (SetPage_1_1) {
                SetPage_1 = SetPage_1_1;
            }
        ],
        execute: function () {
            renderApp();
        }
    };
});
//# sourceMappingURL=ReactApp.js.map