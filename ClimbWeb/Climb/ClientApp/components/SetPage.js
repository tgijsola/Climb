System.register(["react"], function (exports_1, context_1) {
    "use strict";
    var __moduleName = context_1 && context_1.id;
    var React, SetPage;
    return {
        setters: [
            function (React_1) {
                React = React_1;
            }
        ],
        execute: function () {
            SetPage = class SetPage extends React.Component {
                render() {
                    return React.createElement("div", null, "Hello World!");
                }
            };
            exports_1("SetPage", SetPage);
        }
    };
});
//# sourceMappingURL=SetPage.js.map