class Charts {
    constructor() {
        this.dealsChart = null;
        this.leadsChart = null;
        this.initCharts();
    }

    initCharts() {
        // Deals Chart
        const dealsCtx = document.getElementById('dealsChart').getContext('2d');
        this.dealsChart = new Chart(dealsCtx, {
            type: 'bar',
            data: {
                labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                datasets: [{
                    label: 'Deals Value',
                    data: [0, 0, 0, 0, 0, 0],
                    backgroundColor: '#3498db'
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Deals by Month'
                    }
                }
            }
        });

        // Leads Chart
        const leadsCtx = document.getElementById('leadsChart').getContext('2d');
        this.leadsChart = new Chart(leadsCtx, {
            type: 'doughnut',
            data: {
                labels: ['New', 'Contacted', 'Qualified', 'Lost'],
                datasets: [{
                    data: [0, 0, 0, 0],
                    backgroundColor: [
                        '#3498db',
                        '#2ecc71',
                        '#f1c40f',
                        '#e74c3c'
                    ]
                }]
            },
            options: {
                responsive: true,
                plugins: {
                    title: {
                        display: true,
                        text: 'Leads by Status'
                    }
                }
            }
        });
    }

    updateDealsChart(data) {
        this.dealsChart.data.labels = data.labels;
        this.dealsChart.data.datasets[0].data = data.values;
        this.dealsChart.update();
    }

    updateLeadsChart(data) {
        this.leadsChart.data.datasets[0].data = [
            data.new || 0,
            data.contacted || 0,
            data.qualified || 0,
            data.lost || 0
        ];
        this.leadsChart.update();
    }
}

const charts = new Charts(); 