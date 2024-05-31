var connection = new signalR.HubConnectionBuilder()
    .configureLogging(signalR.LogLevel.Information)
    .withUrl("/hub/moviemarks", signalR.HttpTransportType.WebSocket)
    .build();

document.getElementById("bookmark-select").addEventListener("change", function (event) {
    let movieId = document.getElementById('movieId').value;
    let selectedValue = document.getElementById('bookmark-select').value;
    let selectedValueInt = parseInt(selectedValue, 10);

    if (selectedValueInt === -1) {
        connection.send("RemoveBookmark", movieId).catch(function (err){
            return console.error(err.toString());
        })
        console.log("Moviemark has been removed");
    }
    else {
        connection.send("SetBookmark", movieId, selectedValueInt).catch(function (err){
            return console.error(err.toString());
        })
    }
});

function fulfilled() {
    console.log("Connection to Bookmarks hub successful");
}

function rejected() {

}

connection.start().then(fulfilled, rejected);