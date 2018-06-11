define("SetPage", ["require", "exports", "react"], function (require, exports, React) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class SetPage extends React.Component {
        render() {
            return React.createElement("div", null, "Hello World!");
        }
    }
    exports.SetPage = SetPage;
});
define("ReactApp", ["require", "exports", "react", "react-dom", "react-hot-loader", "SetPage"], function (require, exports, React, ReactDOM, react_hot_loader_1, SetPage_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    function renderApp() {
        console.log("here");
        ReactDOM.render(React.createElement(react_hot_loader_1.AppContainer, null,
            React.createElement(SetPage_1.SetPage, null)), document.getElementById("react-app"));
    }
    renderApp();
});
//# sourceMappingURL=script.js.map