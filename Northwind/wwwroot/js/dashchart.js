document.addEventListener('DOMContentLoaded', function () {
    // Bar Chart
    let ctxBar = document.getElementById('barChart').getContext('2d');

    // Function to generate random data within a range
    function generateRandomData(min, max, count) {
        return Array.from({length: count}, () => Math.floor(Math.random() * (max - min + 1)) + min);
    }

    let barChart = new Chart(ctxBar, {
        type: 'bar', data: {
            labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August'], datasets: [{
                label: 'Monthly Visitors',
                data: generateRandomData(1000, 3500, 8),
                backgroundColor: 'rgba(59, 130, 246, 0.6)',
                borderColor: 'rgba(59, 130, 246, 1)',
                borderWidth: 1
            }, {
                label: 'Monthly Sales ($)',
                data: generateRandomData(1500, 3500, 8),
                backgroundColor: 'rgba(16, 185, 129, 0.6)',
                borderColor: 'rgba(16, 185, 129, 1)',
                borderWidth: 1
            }]
        }, options: {
            scales: {
                y: {
                    beginAtZero: true, max: 3500
                }
            }, responsive: true, maintainAspectRatio: false
        }
    });

    //Doughnut Chart
    let ctxDoughnut = document.getElementById('pieChart').getContext('2d');

    // Function to generate random data
    function generateRandomDataForDoughnut(count, total) {
        let data = [];
        let remaining = total;

        for (let i = 0; i < count - 1; i++) {
            let value = Math.floor(Math.random() * remaining);
            data.push(value);
            remaining -= value;
        }
        data.push(remaining);

        return data.sort((a, b) => b - a); // Sort in descending order
    }

    let doughnutChart = new Chart(ctxDoughnut, {
        type: 'doughnut', data: {
            labels: ['Search Engine', 'Paid Ads', 'Social Posts', 'Referral Links', 'Email Campaigns', 'Others'],
            datasets: [{
                data: generateRandomDataForDoughnut(6, 100),
                backgroundColor: ['rgba(59, 130, 246, 0.6)', 'rgba(255, 99, 132, 0.6)', 'rgba(54, 162, 235, 0.6)', 'rgba(255, 206, 86, 0.6)', 'rgba(75, 192, 192, 0.6)', 'rgba(153, 102, 255, 0.6)'],
                borderColor: ['rgba(59, 130, 246, 1)', 'rgba(255, 99, 132, 1)', 'rgba(54, 162, 235, 1)', 'rgba(255, 206, 86, 1)', 'rgba(75, 192, 192, 1)', 'rgba(153, 102, 255, 1)'],
                borderWidth: 1
            }]
        }, options: {
            responsive: true, maintainAspectRatio: false
        }
    });
});