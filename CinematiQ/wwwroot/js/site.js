const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

function toggleNotifications(event) {
    event.stopPropagation();
    var notifications = document.getElementById("notifications");
    if (notifications.style.display === "none" || notifications.style.display === "") {
        notifications.style.display = "block";
    } else {
        notifications.style.display = "none";
    }
}

window.onclick = function (event) {
    var notifications = document.getElementById("notifications");
    if (notifications && notifications.style.display === "block") {
        if (!event.target.closest('.notification-container')) {
            notifications.style.display = "none";
        }
    }
}

