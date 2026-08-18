async function fetchWrapper(input, init, fallback) {
    try {
        const response = await fetch(input, init);
        
        if (response.ok) {
            return {
                success: true,
                message: null
            };
        }

        const data = await response.json();

        return {
            success: false,
            message: data.message ?? fallback
        };
    }
    catch (error) {
        return {
            success: false,
            message: "Unable to connect to the server."
        };
    }
}

async function login(username, password) {
    return await fetchWrapper("/api/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "same-origin",
        body: JSON.stringify({
            username,
            password
        })
    }, "Invalid login attempt.");
}

async function register(username, password, email) {
    return await fetchWrapper("/api/register", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "same-origin",
        body: JSON.stringify({
            username,
            password,
            email
        })
    }, "Invalid registration attempt.");
}

async function logout() {
    return await fetchWrapper("/api/logout", {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        },
        credentials: "same-origin",
    }, "Invalid logout attempt.");
}
