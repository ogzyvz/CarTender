const config = {
    type: 'line',
    data: data,
    options: {
      plugins: {
        title: {
          text: 'Chart.js Combo Time Scale',
          display: true
        }
      },
      scales: {
        x: {
          type: 'time',
          display: true,
          offset: true,
          ticks: {
            source: 'data'
          },
          time: {
            unit: 'day'
          },
        },
      },
    },
  };
  const DATA_COUNT = 7;
const NUMBER_CFG = {count: DATA_COUNT, min: 0, max: 100};

const labels = [];

for (let i = 0; i < DATA_COUNT; ++i) {
  labels.push(Utils.newDate(i));
}

const data = {
  labels: labels,
  datasets: [{
    type: 'bar',
    label: 'Dataset 1',
    backgroundColor: Utils.transparentize(Utils.CHART_COLORS.red, 0.5),
    borderColor: Utils.CHART_COLORS.red,
    data: Utils.numbers(NUMBER_CFG),
  }, {
    type: 'bar',
    label: 'Dataset 2',
    backgroundColor: Utils.transparentize(Utils.CHART_COLORS.blue, 0.5),
    borderColor: Utils.CHART_COLORS.blue,
    data: Utils.numbers(NUMBER_CFG),
  }, {
    type: 'line',
    label: 'Dataset 3',
    backgroundColor: Utils.transparentize(Utils.CHART_COLORS.green, 0.5),
    borderColor: Utils.CHART_COLORS.green,
    fill: false,
    data: Utils.numbers(NUMBER_CFG),
  }]
};
const actions = [
    {
      name: 'Randomize',
      handler(chart) {
        chart.data.datasets.forEach(dataset => {
          dataset.data = Utils.numbers({count: chart.data.labels.length, min: 0, max: 100});
        });
        chart.update();
      }
    },
  ];

  // DEPRECATED
	window.randomScalingFactor = function() {
		return Math.round(Samples.utils.rand(0, 1000));
	};

var barChartData = {
  labels: ['mon','tue','wed','thu','fri'],
  datasets: [
    {label: 'SchipholH',
    backgroundColor: gradient,
    data: [
      48,
      137,
      172,
      0,
      0
    ]}, 
     {label: 'SchipholG',
    backgroundColor: '#E2E2F7',
    data: [
      0,
      0,
      0,
      44,
      39
    ]}, 
    {label: 'AndereH',
    backgroundColor: '#09A0CE',
    data: [
      48*0.29, 
      137*0.29,
      172*0.29,
      0,
      0
    ]},
    {label: 'AndereG',
    backgroundColor: '#DCEBEF',
    data: [
      0,
      0,
      0,
      44*0.29,
      39*0.29
    ]}, 
    
  ]

};
var ctx = document.getElementById('canvas').getContext('2d'),
    gradient = ctx.createLinearGradient(0, 0, 0, 450);

gradient.addColorStop(0, 'rgba(255, 0,0, 0.5)');
gradient.addColorStop(0.5, 'rgba(255, 0,0, 0.25)');
gradient.addColorStop(1, 'rgba(255, 0,0, 0)');




window.myBar = new Chart(ctx, {
  type: 'bar',
  data: barChartData,
  options: {
    animation:{
      easing: 'easeOutBounce',
    },
    tooltips: {
      mode: 'index',
      intersect: false
    },
    responsive: true,
    scales: {
      xAxes: [{
        stacked: true,
        ticks: {
        display:false,
      },
        gridLines: {
          display: false,
          drawBorder: false,
        }
      }],
      yAxes: [{
        stacked: true,
        gridLines: {
          drawBorder: false,
          drawTicks: false,
          color: '#E2E2F7',
          borderDash: [3,3],
          borderDashOffset: 0.5,
          zeroLineColor: '#E2E2F7',
          zeroLineBorderDash: [3,3],
          zeroLineWidth: 3,
          lineWidth: 3,
          cornerRadius: 2,
        }
      },
      ]
    }
  }
});




