am4core.ready(function () {

    am4core.useTheme(am4themes_moonrisekingdom);
    am4core.useTheme(am4themes_animated);

    // Themes end

    // Create chart instance
    var chart2 = am4core.create("appReportCharts", am4charts.PieChart3D);
    chart2.hiddenState.properties.opacity = 0;

    // Add chart title
    var title = chart2.titles.create();
    title.text = "Application Report";
    title.fontSize = 20;
    title.marginBottom = 30;

    chart2.data = generateChartData();
    chart2.legend = new am4charts.Legend();
    chart2.exporting.menu = new am4core.ExportMenu();
    var series2 = chart2.series.push(new am4charts.PieSeries3D());
    series2.dataFields.value = "Count";
    series2.dataFields.category = "Category";
    series2.dataFields.valueYShow = "Count";
    series2.labels.template.text = "{Count}";


    function generateChartData(){
        return JSON.parse($("#chart").val());
    }

});

am4core.ready(function () {

    am4core.useTheme(am4themes_moonrisekingdom);
    am4core.useTheme(am4themes_animated);

    // Themes end

    // Create chart instance
    var chart2 = am4core.create("appReportByLocation", am4charts.PieChart3D);
    chart2.hiddenState.properties.opacity = 0;

    // Add chart title
    var title = chart2.titles.create();
    title.text = "Application Report by Location";
    title.fontSize = 20;
    title.marginBottom = 30;

    chart2.data = generateChartData();
    chart2.legend = new am4charts.Legend();
    chart2.exporting.menu = new am4core.ExportMenu();
    var series2 = chart2.series.push(new am4charts.PieSeries3D());
    series2.dataFields.value = "Count";
    series2.dataFields.category = "Category";
    series2.dataFields.valueYShow = "Count";
    series2.labels.template.text = "{Count}";


    function generateChartData(){
        return JSON.parse($("#chartByLocation").val());
    }

});

am4core.ready(function () {

    am4core.useTheme(am4themes_moonrisekingdom);
    am4core.useTheme(am4themes_animated);

    // Themes end

    // Create chart instance
    var chart2 = am4core.create("paymentReportChart", am4charts.PieChart3D);
    chart2.hiddenState.properties.opacity = 0;

    // Add chart title
    var title = chart2.titles.create();
    title.text = "Payment report by status";
    title.fontSize = 20;
    title.marginBottom = 30;

    chart2.data = generateChartData();
    chart2.legend = new am4charts.Legend();
    chart2.exporting.menu = new am4core.ExportMenu();
    var series2 = chart2.series.push(new am4charts.PieSeries3D());
    series2.dataFields.value = "Sum";
    series2.dataFields.category = "Category";
    series2.dataFields.valueYShow = "Sum";
    series2.labels.template.text = "{Sum}";


    function generateChartData(){
        return JSON.parse($("#paymentList").val());
    }

});

am4core.ready(function () {

    am4core.useTheme(am4themes_animated);
    am4core.useTheme(am4themes_kelly);

    // Themes end

    // Create chart instance
    var chart2 = am4core.create("paymentReportLocationChart", am4charts.XYChart);
    chart2.hiddenState.properties.opacity = 0;

    // Add chart title
    var title = chart2.titles.create();
    title.text = "Payment report by location";
    title.fontSize = 20;
    title.marginBottom = 30;

    chart2.data = generateChartData();
    // Create axes
    var categoryAxis = chart2.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "Category";
    categoryAxis.title.text = "States(s)";
    categoryAxis.renderer.grid.template.location = 0;
    categoryAxis.renderer.minGridDistance = 20;

    var  valueAxis = chart2.yAxes.push(new am4charts.ValueAxis());
    valueAxis.title.text = "Sum Total";
    valueAxis.calculateTotals = true;
    valueAxis.min = 0;
    // valueAxis2.renderer.opposite = true;
    // valueAxis.strictMinMax = true;

// Create series
    var series = chart2.series.push(new am4charts.ColumnSeries());
    series.dataFields.valueY = "Approved";
    series.dataFields.categoryX = "Category";
    series.name = "Approved";
    series.tooltipText = "{name}: [bold]{valueY}[/]";
// This has no effect
// series.stacked = true;

    var series2 = chart2.series.push(new am4charts.ColumnSeries());
    series2.dataFields.valueY = "Pending";
    series2.dataFields.categoryX = "Category";
    series2.name = "Pending";
    series2.tooltipText = "{name}: [bold]{valueY}[/]";
    series2.stacked = true;
// Do not try to stack on top of previous series

    // var series3 = chart.series.push(new am4charts.ColumnSeries());
    // series3.dataFields.valueY = "sales";
    // series3.dataFields.categoryX = "country";
    // series3.name = "Sales";
    // series3.tooltipText = "{name}: [bold]{valueY}[/]";
    // series3.stacked = true;

````// Add cursor
    chart2.cursor = new am4charts.XYCursor();

``  // Add legend
    chart2.legend = new am4charts.Legend();


    function generateChartData(){
        return JSON.parse($("#paymentByLocation").val());
    }

});