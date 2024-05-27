document.addEventListener('DOMContentLoaded', () => {
    const stars = document.querySelectorAll('#star-rating span');

    stars.forEach(star => {
        star.addEventListener('click', () => {
            // Remove the "selected" class from all stars
            stars.forEach(s => s.classList.remove('selected'));

            // Add the "selected" class to the clicked star and all preceding stars
            star.classList.add('selected');
            let prev = star.previousElementSibling;
            while (prev) {
                prev.classList.add('selected');
                prev = prev.previousElementSibling;
            }
        });
    });
});
