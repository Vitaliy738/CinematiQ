document.querySelectorAll('.star-rating span').forEach(star => {
    star.addEventListener('mouseover', function () {
        let value = this.getAttribute('data-value');
        let circleRating = this.closest('.rating-item').querySelector('.circle-rating');
        circleRating.setAttribute('data-rating', value);
    });

    star.addEventListener('mouseleave', function () {
        let circleRating = this.closest('.rating-item').querySelector('.circle-rating');
        circleRating.setAttribute('data-rating', circleRating.getAttribute('data-rating-original'));
    });

    star.addEventListener('click', function () {
        let value = this.getAttribute('data-value');
        let circleRating = this.closest('.rating-item').querySelector('.circle-rating');
        circleRating.setAttribute('data-rating', value);
        circleRating.setAttribute('data-rating-original', value);
    });
});

document.querySelectorAll('.circle-rating').forEach(circle => {
    circle.setAttribute('data-rating-original', circle.getAttribute('data-rating'));
});
