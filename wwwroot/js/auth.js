export function setAuthToken(key, value) {
    if (key && value) {
        localStorage.setItem(key, value);
    }
}

export function getAuthToken(key) {
    if (key) {
        return localStorage.getItem(key);
    }
    return null;
}

export function removeAuthToken(key) {
    if (key) {
        localStorage.removeItem(key);
    }
}