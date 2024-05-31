var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hub/comments", signalR.HttpTransportType.WebSocket)
    .build();

connection.on("ReceivePostComment", function (userName, date, content) {
    addComment(userName, date, content);
});

function addComment(userName, date, content) {
    const commentDiv = document.createElement('div');
    commentDiv.className = 'comment';

    const avatarImg = document.createElement('img');
    avatarImg.src = '/images/icon_profile.png';
    avatarImg.alt = 'Avatar';
    avatarImg.className = 'avatar';

    const commentTextDiv = document.createElement('div');
    commentTextDiv.className = 'comment-text';

    const userNameP = document.createElement('p');
    userNameP.innerHTML = `<strong>${userName}</strong> ${new Date(date).toLocaleDateString()}`;

    const contentP = document.createElement('p');
    contentP.textContent = content;

    commentTextDiv.appendChild(userNameP);
    commentTextDiv.appendChild(contentP);
    commentDiv.appendChild(avatarImg);
    commentDiv.appendChild(commentTextDiv);

    const commentsSection = document.querySelector('.comments');

    if (commentsSection.firstChild) {
        commentsSection.insertBefore(commentDiv, commentsSection.firstChild);
    } else {
        commentsSection.appendChild(commentDiv);
    }
}

document.getElementById("sendComment").addEventListener("click", function (event) {
    let movieId = document.getElementById('movieId').value;
    let commentText = document.getElementById('commentArea').value;

    if (!movieId || !commentText) {
        let alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger';
        alertDiv.role = 'alert';
        alertDiv.innerText = 'A simple danger alert—check it out!';

        document.body.appendChild(alertDiv);

        setTimeout(() => {
            alertDiv.remove();
        }, 3000);

        return;
    }
    else {
        connection.send("PostComment", movieId, commentText).catch(function (err){
            return console.error(err.toString());
        })
    }

    event.preventDefault();
})

function fulfilled() {
    console.log("Connection to Comment hub successful");
}

function rejected() {

}

connection.start().then(fulfilled, rejected);