console.log("Loading init");

const handlers: (() => void)[] = [];
var ready = false;

function completed() {
    ready = true;
    document.removeEventListener("DOMContentLoaded", completed);
    window.removeEventListener("load", completed);
    while (handlers.length) {
        const handler = handlers.pop();
        if (handler != null) {
            handler();
        }
    }
}

// Document already ready?
if (document.readyState === "complete") {
    ready = true;
}
else {
    document.addEventListener("DOMContentLoaded", completed);
    window.addEventListener("load", completed);
}

export function onready(handler: () => void) {
    // Defer execution while the document is loading
    if (ready) {
        handler();
    }
    else {
        handlers.push(handler);
    }
}