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
        let circleRatingElement = document.getElementById('plot-circle-rating');

        if (circleRatingElement) {
            circleRatingElement.setAttribute('data-rating', newPlotRating.toFixed(1));

            let spanElement = circleRatingElement.querySelector('span');
            if (spanElement) {
                spanElement.textContent = newPlotRating.toFixed(1);
            }
        }
    }
});
connection.on("ReceiveSetCharacterRating", function (newCharacterRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;

    if (pageMovieId === movieId) {
        let circleRatingElement = document.getElementById('character-circle-rating');

        if (circleRatingElement) {
            circleRatingElement.setAttribute('data-rating', newCharacterRating.toFixed(1));

            let spanElement = circleRatingElement.querySelector('span');
            if (spanElement) {
                spanElement.textContent = newCharacterRating.toFixed(1);
            }
        }
    }
});
connection.on("ReceiveSetPictureRating", function (newPictureRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;

    if (pageMovieId === movieId) {
        let circleRatingElement = document.getElementById('picture-circle-rating');

        if (circleRatingElement) {
            circleRatingElement.setAttribute('data-rating', newPictureRating.toFixed(1));

            let spanElement = circleRatingElement.querySelector('span');
            if (spanElement) {
                spanElement.textContent = newPictureRating.toFixed(1);
            }
        }
    }
});
connection.on("ReceiveSetPersonalRating", function (newPersonalRating, movieId) {
    let pageMovieId = document.getElementById('movieId').value;

    if (pageMovieId === movieId) {
        let circleRatingElement = document.getElementById('personal-circle-rating');

        if (circleRatingElement) {
            circleRatingElement.setAttribute('data-rating', newPersonalRating.toFixed(1));

            let spanElement = circleRatingElement.querySelector('span');
            if (spanElement) {
                spanElement.textContent = newPersonalRating.toFixed(1);
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

    if (movieId && commentText) {
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
            
            connection.send("SetPlotRating", movieId, ratingValueInt).catch(function (err){
                return console.error(err.toString());
            })
        });
    });
});

// character-rating
document.addEventListener("DOMContentLoaded", function() {
    let stars = document.querySelectorAll("#character-rating span");
    let movieId = document.getElementById('movieId').value;

    stars.forEach(function(star) {
        star.addEventListener("click", function() {
            let ratingValue = this.getAttribute("data-value");
            let ratingValueInt = parseInt(ratingValue, 10);
            
            connection.send("SetCharacterRating", movieId, ratingValueInt).catch(function (err){
                return console.error(err.toString());
            })
        });
    });
});

// picture-rating
document.addEventListener("DOMContentLoaded", function() {
    let stars = document.querySelectorAll("#picture-rating span");
    let movieId = document.getElementById('movieId').value;

    stars.forEach(function(star) {
        star.addEventListener("click", function() {
            let ratingValue = this.getAttribute("data-value");
            let ratingValueInt = parseInt(ratingValue, 10);
            
            connection.send("SetPictureRating", movieId, ratingValueInt).catch(function (err){
                return console.error(err.toString());
            })
        });
    });
});

// personal-rating
document.addEventListener("DOMContentLoaded", function() {
    let stars = document.querySelectorAll("#personal-rating span");
    let movieId = document.getElementById('movieId').value;

    stars.forEach(function(star) {
        star.addEventListener("click", function() {
            let ratingValue = this.getAttribute("data-value");
            let ratingValueInt = parseInt(ratingValue, 10);
            
            connection.send("SetPersonalRating", movieId, ratingValueInt).catch(function (err){
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