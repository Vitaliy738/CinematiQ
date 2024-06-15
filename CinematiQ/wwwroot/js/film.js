var filmConnection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hub/film")
    .build();

async function startFilmConnection() {
    try {
        await filmConnection.start();
    } catch (err) {
        console.error("Error connecting to FilmHub: ", err);
        setTimeout(startFilmConnection, 5000);
    }
}

filmConnection.on("CommentUserIsNotAuthorize", function () {
    var alertPlaceholder = document.getElementById("alertCommentBlock");
    alertPlaceholder.innerHTML = '';

    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-primary alert-dismissible fade show" role="alert">`,
        `   <div>Для того, щоб залишити коментар, вам потрібно бути <a href="/Identity/Account/Login" class="alert-link">авторизованим</a>.
                Якщо ви ще не маєте свого аккаунту - <a href="/Identity/Account/Register" class="alert-link">створіть його</a>.</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    alertPlaceholder.append(wrapper);
});

filmConnection.on("RatingUserIsNotAuthorize", function () {
    var alertPlaceholder = document.getElementById("alertRatingBlock");
    alertPlaceholder.innerHTML = '';

    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-primary alert-dismissible fade show" role="alert">`,
        `   <div>Для того, щоб залишити оцінку до фільму, вам потрібно бути <a href="/Identity/Account/Login" class="alert-link">авторизованим</a>.
                Якщо ви ще не маєте свого аккаунту - <a href="/Identity/Account/Register" class="alert-link">створіть його</a>.</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    alertPlaceholder.append(wrapper);
});

filmConnection.on("MoviemarkerUserIsNotAuthorize", function () {
    var alertPlaceholder = document.getElementById("alertMoviemarkerBlock");
    alertPlaceholder.innerHTML = '';

    const wrapper = document.createElement('div');
    wrapper.innerHTML = [
        `<div class="alert alert-primary alert-dismissible fade show" role="alert">`,
        `   <div>Для того, щоб додати фільм до своїх списків, вам потрібно бути <a href="/Identity/Account/Login" class="alert-link">авторизованим</a>.
                Якщо ви ще не маєте свого аккаунту - <a href="/Identity/Account/Register" class="alert-link">створіть його</a>.</div>`,
        '   <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
        '</div>'
    ].join('');

    alertPlaceholder.append(wrapper);
});

filmConnection.on("ReceivePostComment", function (userName, date, content, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        addComment(userName, date, content);
    }
});

filmConnection.on("CommentDeleted", function (commentId) {
    const commentElement = document.getElementById(commentId);
    if (commentElement) {
        commentElement.remove();
    }
});

filmConnection.on("ReceiveSetPlotRating", function (newPlotRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        updateRating('plot-circle-rating', newPlotRating);
    }
});

filmConnection.on("ReceiveSetCharacterRating", function (newCharacterRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        updateRating('character-circle-rating', newCharacterRating);
    }
});

filmConnection.on("ReceiveSetPictureRating", function (newPictureRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        updateRating('picture-circle-rating', newPictureRating);
    }
});

filmConnection.on("ReceiveSetPersonalRating", function (newPersonalRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;
    if (pageMovieId === movieId) {
        updateRating('personal-circle-rating', newPersonalRating);
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
    commentsSection.insertBefore(commentDiv, commentsSection.firstChild);
}

function updateRating(elementId, newRating) {
    let circleRatingElement = document.getElementById(elementId);
    if (circleRatingElement) {
        circleRatingElement.setAttribute('data-rating', newRating.toFixed(1));
        let spanElement = circleRatingElement.querySelector('span');
        if (spanElement) {
            spanElement.textContent = newRating.toFixed(1);
        }
    }
}

document.getElementById("sendComment").addEventListener("click", function (event) {
    let movieId = document.getElementById('movieId').value;
    let commentText = document.getElementById('commentArea').value;

    if (movieId && commentText) {
        filmConnection.send("PostComment", movieId, commentText).catch(function (err){
            console.error(err.toString());
        });

        commentText.value = "";
    }
    event.preventDefault();
});

document.getElementById("bookmark-select").addEventListener("change", function (event) {
    let movieId = document.getElementById('movieId').value;
    let selectedValue = document.getElementById('bookmark-select').value;
    let selectedValueInt = parseInt(selectedValue, 10);

    if (selectedValueInt === -1) {
        filmConnection.send("RemoveBookmark", movieId).catch(function (err){
            console.error(err.toString());
        });
    } else {
        filmConnection.send("SetBookmark", movieId, selectedValueInt).catch(function (err){
            console.error(err.toString());
        });
    }
});

// Обробка рейтингових подій після завантаження сторінки
document.addEventListener("DOMContentLoaded", function() {
    let ratingTypes = ["plot", "character", "picture", "personal"];
    ratingTypes.forEach(type => {
        let stars = document.querySelectorAll(`#${type}-rating span`);
        let movieId = document.getElementById('movieId').value;

        stars.forEach(star => {
            star.addEventListener("click", function() {
                let ratingValue = this.getAttribute("data-value");
                let ratingValueInt = parseInt(ratingValue, 10);

                filmConnection.send(`Set${type.charAt(0).toUpperCase() + type.slice(1)}Rating`, movieId, ratingValueInt)
                    .catch(function (err){
                        console.error(err.toString());
                    });
            });
        });
    });

    document.querySelectorAll('.btn-delete-comment').forEach(button => {
        button.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            filmConnection.invoke('DeleteComment', commentId).then(() => {
                document.getElementById(commentId).remove();
            }).catch(err => console.error(err.toString()));
        });
    });
});

startFilmConnection();