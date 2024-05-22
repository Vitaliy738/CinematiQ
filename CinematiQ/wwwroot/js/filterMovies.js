function goToPage(pageNumber) {
    console.log('Переключение на страницу', pageNumber);
}
document.querySelectorAll('.page-number').forEach(function (button) {
    button.addEventListener('click', function () {
        var pageNumber = this.textContent;
        goToPage(pageNumber);
    });
});
document.getElementById('prev-page').addEventListener('click', function () {
    console.log('Предыдущая страница');
    //  на предыдущую страницу
});

document.getElementById('next-page').addEventListener('click', function () {
    console.log('Следующая страница');
    //  на следующую страницу
});
