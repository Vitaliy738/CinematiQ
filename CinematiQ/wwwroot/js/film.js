var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hub/film", signalR.HttpTransportType.WebSocket)
    .build();

connection.on("ReceivePostComment", function (userName, date, content, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        addComment(userName, date, content);
    }
});

connection.on("ReceiveSetPlotRating", function (newPlotRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;

    if (pageMovieId === movieId) {
        // Знаходимо елемент з класом circle-rating
        let circleRatingElement = document.getElementById('circle-rating');

        if (circleRatingElement) {
            // Оновлюємо значення атрибута data-rating
            circleRatingElement.setAttribute('data-rating', newPlotRating.toFixed(1));

            // Оновлюємо текст всередині <span>
            let spanElement = circleRatingElement.querySelector('span');
            if (spanElement) {
                spanElement.textContent = newPlotRating.toFixed(1);
            }
        }
    }
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

document.getElementById("bookmark-select").addEventListener("change", function (event) {
    let movieId = document.getElementById('movieId').value;
    let selectedValue = document.getElementById('bookmark-select').value;
    let selectedValueInt = parseInt(selectedValue, 10);

    if (selectedValueInt === -1) {
        connection.send("RemoveBookmark", movieId).catch(function (err){
            return console.error(err.toString());
        })
    }
    else {
        connection.send("SetBookmark", movieId, selectedValueInt).catch(function (err){
            return console.error(err.toString());
        })
    }
});

// plot-rating
document.addEventListener("DOMContentLoaded", function() {
    let stars = document.querySelectorAll("#plot-rating span");
    let movieId = document.getElementById('movieId').value;

    stars.forEach(function(star) {
        star.addEventListener("click", function() {
            let ratingValue = this.getAttribute("data-value");
            let ratingValueInt = parseInt(ratingValue, 10);
            
            console.log(ratingValue);

            connection.send("SetPlotRating", movieId, ratingValueInt).catch(function (err){
                return console.error(err.toString());
            })
        });
    });
});

function fulfilled() {

}

function rejected() {

}

connection.start().then(fulfilled, rejected);