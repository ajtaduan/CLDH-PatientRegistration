// Runs when the page loads - checks if user actually logged in first
async function checkAuth() {
    const res = await fetch("/api/auth/me");
    if (!res.ok) {
        // Not logged in, kick back to log in page
        window.location.href = "index.html";
        return;
    }
    const data = await res.json();
    document.getElementById("welcomeMsg").textContent = `Hi, ${data.username}`;

}
// Loads all patients from the API and fills the table
async function loadPatients() {
    const res = await fetch("/api/patients");
    if (!res.ok) return;

    const patients = await res.json();
    const tbody = document.getElementById("patientTableBody");
    tbody.innerHTML = ""; // clear old rows first

    patients.forEach(p => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${p.fullName}</td>
            <td>${new Date(p.birthDate).toLocaleDateString()}</td>
            <td>${p.gender}</td>
            <td>${p.contactNumber}</td>
            <td>${p.address}</td>
            <td>
                <button onclick="editPatient(${p.id})" style="width:auto">Edit</button>
                <button onclick="deletePatient(${p.id})" style="width:auto; background:#c0392b">Delete</button>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Opens the modal in "Add" mode
document.getElementById("addBtn").addEventListener("click", () => {
    document.getElementById("formTitle").textContent = "Add Patient";
    document.getElementById("patientForm").reset();
    document.getElementById("patientId").value = "";
    document.getElementById("formModal").style.display = "flex";
});

document.getElementById("cancelBtn").addEventListener("click", () => {
        document.getElementById("formModal").style.display = "none";
});

// Opens the modal in "Edit" mode. pre filled with existing patient data
async function editPatient(id) {
    const res = await fetch(`/api/patients/${id}`);
    if (!res.ok) return;

    const p = await res.json();
    document.getElementById("formTitle").textContent = "Edit Patient";
    document.getElementById("patientId").value = p.id;
    document.getElementById("fullName").value = p.fullName;
    document.getElementById("birthDate").value = p.birthDate.split("T")[0]; // trim time part
    document.getElementById("gender").value = p.gender;
    document.getElementById("contactNumber").value = p.contactNumber;
    document.getElementById("address").value = p.address;

    document.getElementById("formModal").style.display = "flex";
}

async function deletePatient(id) {
    if (!confirm("Delete this patient record?")) return;

    const res = await fetch(`/api/patients/${id}`, { method: "DELETE" });
    if (res.ok) loadPatients();
}

//Handles both Add and Edit submissions - decides which based on wheter patientId is set
document.getElementById("patientForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const id = document.getElementById("patientId").value;
    const body = {
        fullName: document.getElementById("fullName").value,
        birthDate: document.getElementById("birthDate").value,
        gender: document.getElementById("gender").value,
        contactNumber: document.getElementById("contactNumber").value,
        address: document.getElementById("address").value
    }

    const url = id ? `/api/patients/${id}` : "/api/patients";
    const method = id ? "PUT" : "POST";

    const res = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(body)
    });

    if (res.ok) {
        document.getElementById("formModal").style.display = "none";
        loadPatients();
    } else {
        document.getElementById("formError").textContent = "Failed to save patient";
    }
});

document.getElementById("logoutBtn").addEventListener("click", async () => {
        await fetch(`/api/auth/logout`, { method: "POST" });
        window.location.href = "index.html";
});

// Run on page load
checkAuth();
loadPatients();
