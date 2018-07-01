class BaseClass {
    getBaseUrl(defaultUrl: string) {
        return window.location.origin;
    }

    getAuthorizationToken() {
        return localStorage.getItem("jwt");
    }
}