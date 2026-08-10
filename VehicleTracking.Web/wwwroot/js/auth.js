async function login(username, password) {
    try {
        const response = await fetch("/api/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            credentials: "same-origin",
            body: JSON.stringify({
                username: username,
                password: password
            })
        });

        return response.status === 200;
    }
    catch (error) {
        return false;
    }
}
