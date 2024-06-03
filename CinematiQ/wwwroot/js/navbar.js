document.getElementById("searchButton").addEventListener("click", function() {
    var searchQuery = document.getElementById("searchBar").value;
    if (searchQuery) {
        var baseUrl = window.location.origin;
        var searchUrl = baseUrl + "/Films/Search?query=" + encodeURIComponent(searchQuery);
        window.location.href = searchUrl;
    }
});