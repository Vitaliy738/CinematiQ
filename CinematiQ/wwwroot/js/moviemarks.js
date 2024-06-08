document.addEventListener("DOMContentLoaded", function() {
    const sortTypeSelect = document.getElementById('sortTypeSelect');
    const searchInput = document.getElementById('searchInput');
    const randomMovieButton = document.getElementById('randomMovieButton');
    const movieList = document.getElementById('movieList');
    const movieItems = Array.from(movieList.getElementsByClassName('movie-item'));

    sortTypeSelect.addEventListener('change', sortAndFilterMovies);
    searchInput.addEventListener('input', sortAndFilterMovies);
    randomMovieButton.addEventListener('click', getRandomMovie);

    function sortAndFilterMovies() {
        const sortType = sortTypeSelect.value;
        const searchText = searchInput.value.toLowerCase();

        movieItems.sort((a, b) => {
            switch (sortType) {
                case 'dateDesc':
                    return new Date(b.getAttribute('data-date')) - new Date(a.getAttribute('data-date'));
                case 'dateAsc':
                    return new Date(a.getAttribute('data-date')) - new Date(b.getAttribute('data-date'));
                case 'yearDesc':
                    return parseInt(b.getAttribute('data-year')) - parseInt(a.getAttribute('data-year'));
                case 'yearAsc':
                    return parseInt(a.getAttribute('data-year')) - parseInt(b.getAttribute('data-year'));
                case 'titleAsc':
                    return a.getAttribute('data-title').localeCompare(b.getAttribute('data-title'));
                case 'titleDesc':
                    return b.getAttribute('data-title').localeCompare(a.getAttribute('data-title'));
                default:
                    return 0;
            }
        });

        movieItems.forEach(item => {
            const title = item.getAttribute('data-title').toLowerCase();
            if (title.includes(searchText)) {
                item.style.display = '';
            } else {
                item.style.display = 'none';
            }
        });

        movieList.innerHTML = '';
        movieItems.forEach(item => movieList.appendChild(item));
    }
    function getRandomMovie(){
        const visibleMovies = movieItems.filter(item => item.style.display !== 'none');
        if (visibleMovies.length > 0) {
            const randomIndex = Math.floor(Math.random() * visibleMovies.length);
            const randomMovie = visibleMovies[randomIndex];
            const movieLink = randomMovie.querySelector('.movie-title');
            if (movieLink) {
                window.location.href = movieLink.href;
            }
        }
    }
});