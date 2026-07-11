// Handles the login form on index.html

document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault(); // stop the page from reloading on submit

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;
    const errorMsg = document.getElementById("errorMsg");

    try {
        const res = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (res.ok) {
            // Login cookie is set automatically by the browser from the response
            window.location.href = "patients.html";
        } else {
            const data = await res.json();
            errorMsg.textContent = data.message || "Login failed";
        }
    } catch (err) {
        errorMsg.textContent = "Could not reach server";
    }

});