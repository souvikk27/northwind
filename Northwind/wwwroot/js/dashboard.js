document.addEventListener('DOMContentLoaded', function () {
    // Set up color theme
    const colors = {
        primary: '#4F46E5',
        secondary: '#10B981',
        tertiary: '#F59E0B',
        quaternary: '#EC4899',
        info: '#3B82F6',
        success: '#22C55E',
        warning: '#F97316',
        danger: '#EF4444',
        light: '#F3F4F6',
        dark: '#1F2937',
        // Gradients and transparencies
        primaryTransparent: 'rgba(79, 70, 229, 0.2)',
        secondaryTransparent: 'rgba(16, 185, 129, 0.2)',
        tertiaryTransparent: 'rgba(245, 158, 11, 0.2)',
        quaternaryTransparent: 'rgba(236, 72, 153, 0.2)',
    };

    // Utility function to create gradient backgrounds for charts
    function createGradient(ctx, startColor, endColor) {
        const gradient = ctx.createLinearGradient(0, 0, 0, 400);
        gradient.addColorStop(0, startColor);
        gradient.addColorStop(1, endColor);
        return gradient;
    }

    // Function to generate realistic sales data with seasonal trends
    function generateSalesData() {
        // Base values with seasonal patterns (higher in Q4, lower in Q1)
        const baseValues = [
            5200, 4800, 5500, 6200, 7100, 7800, 
            8300, 8100, 7600, 8500, 9800, 11200
        ];
        
        // Add some randomness but keep the trend
        return baseValues.map(value => {
            const randomFactor = 0.9 + Math.random() * 0.2; // Random between 0.9 and 1.1
            return Math.round(value * randomFactor);
        });
    }

    // Function to generate realistic customer growth data
    function generateCustomerData() {
        // Starting with 1000 customers and growing
        let customers = 1000;
        const growth = [];
        
        for (let i = 0; i < 12; i++) {
            // Growth rate varies between 2-5% per month
            const growthRate = 0.02 + Math.random() * 0.03;
            const newCustomers = Math.round(customers * growthRate);
            customers += newCustomers;
            growth.push(customers);
        }
        
        return growth;
    }

    // Function to generate realistic order data
    function generateOrderData() {
        // Base values with seasonal patterns
        const baseValues = [
            320, 310, 340, 380, 420, 450, 
            470, 460, 430, 480, 520, 580
        ];
        
        // Add some randomness but keep the trend
        return baseValues.map(value => {
            const randomFactor = 0.95 + Math.random() * 0.1; // Random between 0.95 and 1.05
            return Math.round(value * randomFactor);
        });
    }

    // Monthly Sales & Revenue Chart
    const salesChartCtx = document.getElementById('salesChart').getContext('2d');
    const salesData = generateSalesData();
    const customerData = generateCustomerData();
    
    new Chart(salesChartCtx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [
                {
                    label: 'Revenue ($K)',
                    data: salesData,
                    borderColor: colors.primary,
                    backgroundColor: createGradient(salesChartCtx, colors.primaryTransparent, 'rgba(255, 255, 255, 0)'),
                    tension: 0.4,
                    fill: true,
                    pointBackgroundColor: colors.primary,
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4,
                    pointHoverRadius: 6,
                },
                {
                    label: 'Customers',
                    data: customerData,
                    borderColor: colors.secondary,
                    backgroundColor: 'transparent',
                    tension: 0.4,
                    borderDash: [5, 5],
                    pointBackgroundColor: colors.secondary,
                    pointBorderColor: '#fff',
                    pointBorderWidth: 2,
                    pointRadius: 4,
                    pointHoverRadius: 6,
                    yAxisID: 'y1',
                }
            ]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            interaction: {
                mode: 'index',
                intersect: false,
            },
            plugins: {
                legend: {
                    position: 'top',
                    labels: {
                        usePointStyle: true,
                        boxWidth: 6
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(255, 255, 255, 0.9)',
                    titleColor: '#1F2937',
                    bodyColor: '#4B5563',
                    borderColor: '#E5E7EB',
                    borderWidth: 1,
                    padding: 12,
                    boxPadding: 6,
                    usePointStyle: true,
                    callbacks: {
                        label: function(context) {
                            let label = context.dataset.label || '';
                            if (label) {
                                label += ': ';
                            }
                            if (context.datasetIndex === 0) {
                                label += '$' + context.parsed.y.toLocaleString();
                            } else {
                                label += context.parsed.y.toLocaleString() + ' customers';
                            }
                            return label;
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        borderDash: [2, 4],
                        color: '#E5E7EB'
                    },
                    ticks: {
                        callback: function(value) {
                            return '$' + value / 1000 + 'K';
                        }
                    }
                },
                y1: {
                    beginAtZero: true,
                    position: 'right',
                    grid: {
                        display: false,
                    },
                    ticks: {
                        callback: function(value) {
                            return value.toLocaleString();
                        }
                    }
                }
            }
        }
    });

    // Orders by Category Chart
    const categoryChartCtx = document.getElementById('categoryChart').getContext('2d');
    
    new Chart(categoryChartCtx, {
        type: 'doughnut',
        data: {
            labels: ['Beverages', 'Confections', 'Dairy Products', 'Grains/Cereals', 'Meat/Poultry', 'Produce', 'Seafood', 'Condiments'],
            datasets: [{
                data: [18, 14, 16, 12, 10, 9, 11, 10],
                backgroundColor: [
                    colors.primary,
                    colors.secondary,
                    colors.tertiary,
                    colors.quaternary,
                    colors.info,
                    colors.success,
                    colors.warning,
                    colors.danger
                ],
                borderWidth: 0,
                hoverOffset: 6
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '70%',
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        usePointStyle: true,
                        padding: 20,
                        font: {
                            size: 12
                        }
                    }
                },
                tooltip: {
                    backgroundColor: 'rgba(255, 255, 255, 0.9)',
                    titleColor: '#1F2937',
                    bodyColor: '#4B5563',
                    borderColor: '#E5E7EB',
                    borderWidth: 1,
                    padding: 12,
                    boxPadding: 6,
                    usePointStyle: true,
                    callbacks: {
                        label: function(context) {
                            const value = context.raw;
                            const total = context.dataset.data.reduce((acc, data) => acc + data, 0);
                            const percentage = Math.round((value / total) * 100);
                            return `${context.label}: ${percentage}% (${value} orders)`;
                        }
                    }
                }
            }
        }
    });

    // Orders Trend Chart
    const ordersTrendCtx = document.getElementById('ordersTrendChart').getContext('2d');
    const ordersData = generateOrderData();
    
    new Chart(ordersTrendCtx, {
        type: 'bar',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Orders',
                data: ordersData,
                backgroundColor: createGradient(ordersTrendCtx, colors.tertiaryTransparent, colors.tertiary),
                borderColor: colors.tertiary,
                borderWidth: 1,
                borderRadius: 4,
                barThickness: 12,
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    backgroundColor: 'rgba(255, 255, 255, 0.9)',
                    titleColor: '#1F2937',
                    bodyColor: '#4B5563',
                    borderColor: '#E5E7EB',
                    borderWidth: 1,
                    padding: 12,
                    boxPadding: 6,
                    callbacks: {
                        label: function(context) {
                            return `Orders: ${context.parsed.y}`;
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: {
                        display: false
                    }
                },
                y: {
                    beginAtZero: true,
                    grid: {
                        borderDash: [2, 4],
                        color: '#E5E7EB'
                    }
                }
            }
        }
    });

    // Top Countries Map
    const map = new Datamap({
        element: document.getElementById('worldMap'),
        responsive: true,
        fills: {
            defaultFill: '#E5E7EB',
            primary: colors.primary,
            secondary: colors.secondary,
            tertiary: colors.tertiary,
            quaternary: colors.quaternary,
            info: colors.info
        },
        data: {
            USA: { fillKey: 'primary', orders: 428, revenue: '$38,290' },
            DEU: { fillKey: 'secondary', orders: 326, revenue: '$29,830' },
            GBR: { fillKey: 'tertiary', orders: 274, revenue: '$25,430' },
            FRA: { fillKey: 'quaternary', orders: 216, revenue: '$19,820' },
            ESP: { fillKey: 'info', orders: 187, revenue: '$16,750' },
        },
        geographyConfig: {
            borderWidth: 0.5,
            borderColor: '#FFFFFF',
            highlightFillColor: function(geo) {
                return geo.fillKey || '#4F46E5';
            },
            highlightBorderColor: '#FFFFFF',
            highlightBorderWidth: 1,
            highlightBorderOpacity: 1,
            popupTemplate: function(geo, data) {
                if (!data) return ['<div class="p-2 bg-white shadow-lg rounded-lg border border-gray-200">', 
                                  '<strong>' + geo.properties.name + '</strong>',
                                  '<p class="text-sm text-gray-600">No data available</p>',
                                  '</div>'].join('');
                
                return ['<div class="p-2 bg-white shadow-lg rounded-lg border border-gray-200">', 
                        '<strong>' + geo.properties.name + '</strong>',
                        '<p class="text-sm text-gray-600">Orders: ' + data.orders + '</p>',
                        '<p class="text-sm text-gray-600">Revenue: ' + data.revenue + '</p>',
                        '</div>'].join('');
            }
        }
    });

    // Handle window resize for responsive map
    window.addEventListener('resize', function() {
        map.resize();
    });
});
