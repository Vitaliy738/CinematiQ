var notificationConnection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hub/notification")
    .build();

async function startNotificationConnection() {
    try {
        const newNotification = document.getElementById('NewNotification');
        newNotification.style.visibility = 'hidden';
        await notificationConnection.start().then();
    } catch (err) {
        console.error("Error connecting to NotificationHub: ", err);
        setTimeout(startNotificationConnection, 5000);
    }
}

function toggleNotifications(event) {
    event.stopPropagation();
    var notifications = document.getElementById("notifications");
    if (notifications.style.display === "none" || notifications.style.display === "") {
        notifications.style.display = "block";

        var notificationItems = document.querySelectorAll('#notificationList li');
        var notificationsId = Array.from(notificationItems).map(item => item.id);
        
        notificationConnection.send("NotificationToRead", notificationsId)
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

notificationConnection.on("ReceiveSendNotification", function (notification) {
    const notificationList = document.getElementById('notificationList');
    const li = document.createElement('li');
    li.textContent = notification.content;
    li.id = notification.id;
    notificationList.appendChild(li);

    const newNotification = document.getElementById('NewNotification');
    newNotification.style.visibility = 'visible';
});

notificationConnection.on("ReceiveUserNotifications", function (userNotifications) {
    const notificationList = document.getElementById('notificationList');
    const newNotification = document.getElementById('NewNotification');
    newNotification.style.visibility = 'hidden';

    notificationList.innerHTML = '';

    userNotifications.forEach(notification => {
        if (notification.isRead){
            const li = document.createElement('li');
            li.textContent = notification.content;
            li.id = notification.id;
            notificationList.appendChild(li);
        }
        else {
            const li = document.createElement('li');
            li.textContent = notification.content;
            li.id = notification.id;
            notificationList.appendChild(li);
            newNotification.style.visibility = 'visible';
        }
    });
});

notificationConnection.on("ReceiveNotificationToRead", function () {
    const newNotification = document.getElementById('NewNotification');
    newNotification.style.visibility = 'hidden';
});

startNotificationConnection();