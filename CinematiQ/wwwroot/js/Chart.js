document.addEventListener('DOMContentLoaded', function () {
    var watchingCountElement = document.getElementById('watchingCount');
    var plannedCountElement = document.getElementById('plannedCount');
    var viewedCountElement = document.getElementById('viewedCount');
    var postponedCountElement = document.getElementById('postponedCount');
    var abandonedCountElement = document.getElementById('abandonedCount');
    var favoriteCountElement = document.getElementById('favoriteCount');

    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Дивлюсь', 'В планах', 'Переглянуто', 'Відкладено', 'Кинуто', 'Улюблені'],
            datasets: [{
                data: [
                    parseInt(watchingCountElement.textContent),
                    parseInt(plannedCountElement.textContent),
                    parseInt(viewedCountElement.textContent),
                    parseInt(postponedCountElement.textContent),
                    parseInt(abandonedCountElement.textContent),
                    parseInt(favoriteCountElement.textContent)
                ],
                backgroundColor: [
                    '#639C67',
                    '#9E67A0',
                    '#5C68A2',
                    '#D0902E',
                    '#B0443F'
                ],
                borderWidth: 1
            }]
        },
        options: {
            plugins: {
                tooltip: {
                    enabled: false 
                },
                legend: {
                    display: false
                }
            }
        }
    });
});
