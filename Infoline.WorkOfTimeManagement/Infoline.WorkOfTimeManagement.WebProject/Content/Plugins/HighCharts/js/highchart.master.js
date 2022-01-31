$(document)

    .on('ready', function () {

        Highcharts.setOptions({
            global: {
                useUTC: false
            },
            lang: {
                shortMonths: ['Ocak', 'Şub', 'Mart', 'Nis', 'May', 'Haz', 'Tem', 'Ağu', 'Eyl', 'Ekim', 'Kas', 'Ara'],
                months: ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'],
                weekdays: ['Pazar', 'Pazartesi', 'Salı', 'Çarşamba', 'Perşembe', 'Cuma', 'Cumartesi'],
                downloadJPEG: 'JPEG Olarak Kaydet',
                downloadPNG: 'PDF Olarak Kaydet',
                downloadSVG: 'SVG Olarak Kaydet',
                downloadPDF: 'PNG Olarak Kaydet',
                printChart: 'Yazdır'
            },
            colors: ['#1ab394', '#1c84c6', '#ed5565', '#f8ac59', '#1ab394', '#f8ac59', '#ed5565', '#23c6c8', '#1c84c6', '#3d4d5d', '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
            chart: {
                backgroundColor: null,
                style: {
                    fontFamily: "open sans"
                }
            },
            title: {
                style: {
                    fontSize: '16px',
                    fontWeight: 'bold',
                    textTransform: 'uppercase'
                }
            },
            tooltip: {
                borderWidth: 0,
                backgroundColor: 'rgba(219,219,216,0.8)',
                shadow: false
            },
            legend: {
                itemStyle: {
                    fontWeight: 'bold',
                    fontSize: '13px'
                }
            },
            xAxis: {
                gridLineWidth: 1,
                labels: {
                    style: {
                        fontSize: '12px'
                    }
                }
            },
            yAxis: {
                minorTickInterval: 'auto',
                title: {
                    style: {
                        textTransform: 'uppercase'
                    }
                },
                labels: {
                    style: {
                        fontSize: '12px'
                    }
                }
            },
            plotOptions: {
                candlestick: {
                    lineColor: '#404048'
                }
            },
            credits: {
                enabled: false
            },

            // General
            background2: '#F0F0EA'

        });


    })

;