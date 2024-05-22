var data = {
    labels: ['Дивлюсь', 'В планах', 'Переглянуто', 'Відкладено', 'Кинуто'],
    datasets: [{
        data: [2, 5, 3, 2, 1], 
        backgroundColor: [
            '#639C67',
            '#9E67A0',
            '#5C68A2',
            '#D0902E',
            '#B0443F'
        ]
    }]
};

var options = {
    responsive: true,
    legend: {
        position: 'bottom'
    },
    title: {
        display: true,
        text: 'Статистика просмотров'
    },
    animation: {
        animateScale: true,
        animateRotate: true
    }
};

// круговой диаграма
var ctx = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(ctx, {
    type: 'pie',
    data: data,
    options: options
});