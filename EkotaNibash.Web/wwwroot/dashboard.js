// wwwroot/js/dashboard.js
window.renderDashboardCharts = (paymentData, expenseData) => {
    // Payment Chart
    const paymentCtx = document.getElementById('paymentChart').getContext('2d');
    new Chart(paymentCtx, {
        type: 'doughnut',
        data: {
            labels: paymentData.map(p => p.paymentType),
            datasets: [{
                data: paymentData.map(p => p.totalAmount),
                backgroundColor: [
                    '#4e73df', '#1cc88a', '#36b9cc', '#f6c23e', '#e74a3b'
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom',
                }
            }
        }
    });

    // Expense Chart
    const expenseCtx = document.getElementById('expenseChart').getContext('2d');
    new Chart(expenseCtx, {
        type: 'bar',
        data: {
            labels: expenseData.map(e => e.expenseType),
            datasets: [{
                label: 'Expense Amount',
                data: expenseData.map(e => e.totalAmount),
                backgroundColor: '#fd7e14'
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return '৳' + value;
                        }
                    }
                }
            }
        }
    });
};