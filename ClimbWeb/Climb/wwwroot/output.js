var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
System.register("SetPage", ["react"], function (exports_1, context_1) {
    "use strict";
    var React, SetPage;
    var __moduleName = context_1 && context_1.id;
    return {
        setters: [
            function (React_1) {
                React = React_1;
            }
        ],
        execute: function () {
            SetPage = /** @class */ (function (_super) {
                __extends(SetPage, _super);
                function SetPage() {
                    return _super !== null && _super.apply(this, arguments) || this;
                }
                SetPage.prototype.render = function () {
                    return React.createElement("div", null, "Hello World!");
                };
                return SetPage;
            }(React.Component));
            exports_1("SetPage", SetPage);
        }
    };
});
System.register("ReactApp", ["react", "react-dom", "react-hot-loader", "react-router-dom", "SetPage"], function (exports_2, context_2) {
    "use strict";
    var React, ReactDOM, react_hot_loader_1, react_router_dom_1, SetPage_1;
    var __moduleName = context_2 && context_2.id;
    function renderApp() {
        // This code starts up the React app when it runs in a browser. It sets up the routing
        // configuration and injects the app into a DOM element.
        var baseUrl = document.getElementsByTagName("base")[0].getAttribute("href");
        ReactDOM.render(React.createElement(react_hot_loader_1.AppContainer, null,
            React.createElement(react_router_dom_1.BrowserRouter, { basename: baseUrl },
                React.createElement(SetPage_1.SetPage, null))), document.getElementById("react-app"));
    }
    return {
        setters: [
            function (React_2) {
                React = React_2;
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
