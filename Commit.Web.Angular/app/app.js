var app = angular.module('myApp', ['ngRoute', 'myApp.localization', 'ui.bootstrap', 'ngAnimate', 'myApp.directives', "kendo.directives", "uiGmapgoogle-maps"]);

app.config(['$routeProvider', function ($routeProvider) {

    $routeProvider.
      when('/', {
          templateUrl: '../../app/templates/home.html',
          controller: 'homeController'
      }).
      when('/schools', {
          templateUrl: '../../app/templates/pages/schools_dashboard.html',
          controller: 'schoolsController'
      }).
      when('/districts', {
          templateUrl: '../../app/templates/pages/districts_dashboard.html',
          controller: 'districtsController'
      }).
      when('/type', {
          templateUrl: '../../app/templates/pages/type.html'
      }).
      when('/components', {
          templateUrl: '../../app/templates/pages/components.html'
      }).
      when('/boxes', {
          templateUrl: '../../app/templates/pages/boxes.html'
      }).
      when('/tab', {
          templateUrl: '../../app/templates/pages/tab.html'
      }).
      otherwise({
          redirectTo: '/'
      });

}]);

app.controller("slimParentController", ["$scope", "$rootScope", function ($scope, $rootScope) {
    var $window;
    return $window = $(window), $scope.main = {
        brand: "Commit!",
        name: "Lisa Doe"
    },
        $scope.pageTransitionOpts = [{
            name: "Fade up",
            "class": "animate-fade-up"
        }, {
            name: "Scale up",
            "class": "ainmate-scale-up"
        }, {
            name: "Slide in from right",
            "class": "ainmate-slide-in-right"
        }, {
            name: "Flip Y",
            "class": "animate-flip-y"
        }],

        $scope.admin = {
            layout: "wide",
            menu: "vertical",
            fixedHeader: !0,
            fixedSidebar: !0,
            pageTransition: $scope.pageTransitionOpts[0],
            skin: "11"
        },

        $scope.$watch("admin", function (newVal, oldVal) {
            return "horizontal" === newVal.menu && "vertical" === oldVal.menu ? void $rootScope.$broadcast("nav:reset") : newVal.fixedHeader === !1 && newVal.fixedSidebar === !0 ? (oldVal.fixedHeader === !1 && oldVal.fixedSidebar === !1 && ($scope.admin.fixedHeader = !0, $scope.admin.fixedSidebar = !0), void (oldVal.fixedHeader === !0 && oldVal.fixedSidebar === !0 && ($scope.admin.fixedHeader = !1, $scope.admin.fixedSidebar = !1))) : (newVal.fixedSidebar === !0 && ($scope.admin.fixedHeader = !0), void (newVal.fixedHeader === !1 && ($scope.admin.fixedSidebar = !1)))
        }, !0),

        $scope.color = {
            primary: "#5B90BF",
            success: "#A3BE8C",
            info: "#7FABD2",
            infoAlt: "#B48EAD",
            warning: "#EBCB8B",
            danger: "#BF616A",
            gray: "#DCDCDC"
        }
}]);

app.controller('HeaderCtrl', function ($scope) {

});

app.controller('NavContainerCtrl', function ($scope) {

});

app.controller("NavCtrl", ["$scope", function ($scope) {


}]);

app.controller("LangCtrl", ["$scope", "localize", function ($scope, localize) {

    return $scope.lang = "English", $scope.setLang = function (lang) {
        switch (lang) {
            case "English":
                localize.setLanguage("EN-US");
                break;
            case "Español":
                localize.setLanguage("ES-ES");
                break;
            case "日本語":
                localize.setLanguage("JA-JP");
                break;
            case "中文":
                localize.setLanguage("ZH-TW");
                break;
            case "Deutsch":
                localize.setLanguage("DE-DE");
                break;
            case "français":
                localize.setLanguage("FR-FR");
                break;
            case "Italiano":
                localize.setLanguage("IT-IT");
                break;
            case "Portugal":
                localize.setLanguage("PT-BR");
                break;
            case "Русский язык":
                localize.setLanguage("RU-RU");
                break;
            case "한국어":
                localize.setLanguage("KO-KR")
        }
        return $scope.lang = lang

    }, $scope.getFlag = function () {
        var lang;
        switch (lang = $scope.lang) {
            case "English":
                return "flags-american";
            case "Español":
                return "flags-spain";
            case "日本語":
                return "flags-japan";
            case "中文":
                return "flags-china";
            case "Deutsch":
                return "flags-germany";
            case "français":
                return "flags-france";
            case "Italiano":
                return "flags-italy";
            case "Portugal":
                return "flags-portugal";
            case "Русский язык":
                return "flags-russia";
            case "한국어":
                return "flags-korea"
        }
    }
}]);

app.controller('dashboardController', function ($scope) {

});

app.controller('homeController', function ($scope) {
    $scope.welcomeMessage = "Welcome to Home";
});

app.controller("jvectormapCtrl", ["$scope",
            function ($scope) {
                console.log('asdfasdf');
                var marker_data;
                return marker_data = [{
                    latLng: [40.71, -74],
                    name: "New York"
                }, {
                    latLng: [39.9, 116.4],
                    name: "Beijing"
                }, {
                    latLng: [31.23, 121.47],
                    name: "Shanghai"
                }, {
                    latLng: [-33.86, 151.2],
                    name: "Sydney"
                }, {
                    latLng: [-37.81, 144.96],
                    name: "Melboune"
                }, {
                    latLng: [37.33, -121.89],
                    name: "San Jose"
                }, {
                    latLng: [1.3, 103.8],
                    name: "Singapore"
                }, {
                    latLng: [47.6, -122.33],
                    name: "Seattle"
                }, {
                    latLng: [41.87, -87.62],
                    name: "Chicago"
                }, {
                    latLng: [37.77, -122.41],
                    name: "San Francisco"
                }, {
                    latLng: [32.71, -117.16],
                    name: "San Diego"
                }, {
                    latLng: [51.5, -.12],
                    name: "London"
                }, {
                    latLng: [48.85, 2.35],
                    name: "Paris"
                }, {
                    latLng: [52.52, 13.4],
                    name: "Berlin"
                }, {
                    latLng: [-26.2, 28.04],
                    name: "Johannesburg"
                }, {
                    latLng: [35.68, 139.69],
                    name: "Tokyo"
                }, {
                    latLng: [13.72, 100.52],
                    name: "Bangkok"
                }, {
                    latLng: [37.56, 126.97],
                    name: "Seoul"
                }, {
                    latLng: [41.87, 12.48],
                    name: "Roma"
                }, {
                    latLng: [45.42, -75.69],
                    name: "Ottawa"
                }, {
                    latLng: [55.75, 37.61],
                    name: "Moscow"
                }, {
                    latLng: [-22.9, -43.19],
                    name: "Rio de Janeiro"
                }], $scope.worldMap = {
                    map: "world_mill_en",
                    markers: marker_data,
                    normalizeFunction: "polynomial",
                    backgroundColor: null,
                    zoomOnScroll: !1,
                    regionStyle: {
                        initial: {
                            fill: "#EEEFF3"
                        },
                        hover: {
                            fill: $scope.color.primary
                        }
                    },
                    markerStyle: {
                        initial: {
                            fill: "#BF616A",
                            stroke: "rgba(191,97,106,.8)",
                            "fill-opacity": 1,
                            "stroke-width": 9,
                            "stroke-opacity": .5
                        },
                        hover: {
                            stroke: "black",
                            "stroke-width": 2
                        }
                    }
                }
            }
]);

app.controller("flotChartCtrl", ["$scope", function ($scope) {
    var areaChart, barChart, barChartH, lineChart1, sampledata1, sampledata2; return lineChart1 = {}, lineChart1.data1 = [[1, 15], [2, 20], [3, 14], [4, 10], [5, 10], [6, 20], [7, 28], [8, 26], [9, 22]], $scope.line1 = {}, $scope.line1.data = [{ data: lineChart1.data1, label: "Trend" }], $scope.line1.options = {
        series: {
            lines: {
                show: !0, fill: !0, fillColor:
                    { colors: [{ opacity: 0 }, { opacity: .3 }] }
            }, points: { show: !0, lineWidth: 2, fill: !0, fillColor: "#ffffff", symbol: "circle", radius: 5 }
        }, colors: [$scope.color.primary, $scope.color.infoAlt], tooltip: !0, tooltipOpts: { defaultTheme: !1 }, grid: { hoverable: !0, clickable: !0, tickColor: "#f9f9f9", borderWidth: 1, borderColor: "#eeeeee" }, xaxis: { ticks: [[1, "Jan."], [2, "Feb."], [3, "Mar."], [4, "Apr."], [5, "May"], [6, "June"], [7, "July"], [8, "Aug."], [9, "Sept."], [10, "Oct."], [11, "Nov."], [12, "Dec."]] }
    }, areaChart = {}, areaChart.data1 = [[2007, 15], [2008, 20], [2009, 10], [2010, 5], [2011, 5], [2012, 20], [2013, 28]], areaChart.data2 = [[2007, 15], [2008, 16], [2009, 22], [2010, 14], [2011, 12], [2012, 19], [2013, 22]], $scope.area = {}, $scope.area.data = [{ data: areaChart.data1, label: "Value A", lines: { fill: !0 } }, { data: areaChart.data2, label: "Value B", points: { show: !0 }, yaxis: 2 }], $scope.area.options = { series: { lines: { show: !0, fill: !1 }, points: { show: !0, lineWidth: 2, fill: !0, fillColor: "#ffffff", symbol: "circle", radius: 5 }, shadowSize: 0 }, grid: { hoverable: !0, clickable: !0, tickColor: "#f9f9f9", borderWidth: 1, borderColor: "#eeeeee" }, colors: [$scope.color.success, $scope.color.danger], tooltip: !0, tooltipOpts: { defaultTheme: !1 }, xaxis: { mode: "time" }, yaxes: [{}, { position: "right" }] }, sampledata1 = [[1, 65], [2, 59], [3, 90], [4, 81], [5, 56], [6, 55], [7, 68], [8, 45], [9, 66]], sampledata2 = [[1, 28], [2, 48], [3, 30], [4, 60], [5, 100], [6, 50], [7, 10], [8, 25], [9, 50]], $scope.area1 = {}, $scope.area1.data = [{ label: " A", data: sampledata1, bars: { order: 0, fillColor: { colors: [{ opacity: .3 }, { opacity: .3 }] }, show: !0, fill: 1, barWidth: .3, align: "center", horizontal: !1 } }, { data: sampledata2, curvedLines: { apply: !0 }, lines: { show: !0, fill: !0, fillColor: { colors: [{ opacity: .2 }, { opacity: .2 }] } } }, { data: sampledata2, label: "D", points: { show: !0 } }], $scope.area1.options = { series: { curvedLines: { active: !0 }, points: { lineWidth: 2, fill: !0, fillColor: "#ffffff", symbol: "circle", radius: 4 } }, grid: { hoverable: !0, clickable: !0, tickColor: "#f9f9f9", borderWidth: 1, borderColor: "#eeeeee" }, tooltip: !0, tooltipOpts: { defaultTheme: !1 }, colors: [$scope.color.gray, $scope.color.primary, $scope.color.primary] }, barChart = {}, barChart.data1 = [[2008, 20], [2009, 10], [2010, 5], [2011, 5], [2012, 20], [2013, 28]], barChart.data2 = [[2008, 16], [2009, 22], [2010, 14], [2011, 12], [2012, 19], [2013, 22]], barChart.data3 = [[2008, 12], [2009, 30], [2010, 20], [2011, 19], [2012, 13], [2013, 20]], $scope.barChart = {}, $scope.barChart.data = [{ label: "Value A", data: barChart.data1 }, { label: "Value B", data: barChart.data2 }, { label: "Value C", data: barChart.data3 }], $scope.barChart.options = { series: { stack: !0, bars: { show: !0, fill: 1, barWidth: .3, align: "center", horizontal: !1, order: 1 } }, grid: { hoverable: !0, borderWidth: 1, borderColor: "#eeeeee" }, tooltip: !0, tooltipOpts: { defaultTheme: !1 }, colors: [$scope.color.success, $scope.color.info, $scope.color.warning, $scope.color.danger] }, $scope.barChart1 = {}, $scope.barChart1.data = [{ label: "Value A", data: barChart.data1, bars: { order: 0 } }, { label: "Value B", data: barChart.data2, bars: { order: 1 } }, { label: "Value C", data: barChart.data3, bars: { order: 2 } }], $scope.barChart1.options = { series: { stack: !0, bars: { show: !0, fill: 1, barWidth: .2, align: "center", horizontal: !1 } }, grid: { hoverable: !0, borderWidth: 1, borderColor: "#eeeeee" }, tooltip: !0, tooltipOpts: { defaultTheme: !1 }, colors: [$scope.color.success, $scope.color.info, $scope.color.warning, $scope.color.danger] }, $scope.barChart3 = {}, $scope.barChart3.data = [{ label: " A", data: [[40, 1], [59, 2], [90, 3], [81, 4], [56, 5]], bars: { order: 0, fillColor: { colors: [{ opacity: .3 }, { opacity: .3 }] } } }, { label: " B", data: [[28, 1], [48, 2], [40, 3], [19, 4], [45, 5]], bars: { order: 1, fillColor: { colors: [{ opacity: .3 }, { opacity: .3 }] } } }], $scope.barChart3.options = { series: { stack: !0, bars: { show: !0, fill: 1, barWidth: .35, align: "center", horizontal: !0 } }, grid: { show: !0, aboveData: !1, color: "#eaeaea", hoverable: !0, borderWidth: 1, borderColor: "#eaeaea" }, tooltip: !0, tooltipOpts: { defaultTheme: !1 }, colors: [$scope.color.gray, $scope.color.primary, $scope.color.info, $scope.color.danger] }, barChartH = {}, barChartH.data1 = [[85, 10], [50, 20], [55, 30]], barChartH.data2 = [[77, 10], [60, 20], [70, 30]], barChartH.data3 = [[100, 10], [70, 20], [55, 30]], $scope.barChart2 = {}, $scope.barChart2.data = [{ label: "Value A", data: barChartH.data1, bars: { order: 1 } }, { label: "Value B", data: barChartH.data2, bars: { order: 2 } }, { label: "Value C", data: barChartH.data3, bars: { order: 3 } }], $scope.barChart2.options = { series: { stack: !0, bars: { show: !0, fill: 1, barWidth: 1, align: "center", horizontal: !0 } }, grid: { hoverable: !0, borderWidth: 1, borderColor: "#eeeeee" }, tooltip: !0, tooltipOpts: { defaultTheme: !1 }, colors: [$scope.color.success, $scope.color.info, $scope.color.warning, $scope.color.danger] }, $scope.pieChart = {}, $scope.pieChart.data = [{ label: "Download Sales", data: 12 }, { label: "In-Store Sales", data: 30 }, { label: "Mail-Order Sales", data: 20 }, { label: "Online Sales", data: 19 }], $scope.pieChart.options = { series: { pie: { show: !0 } }, legend: { show: !0 }, grid: { hoverable: !0, clickable: !0 }, colors: [$scope.color.primary, $scope.color.success, $scope.color.info, $scope.color.warning, $scope.color.danger], tooltip: !0, tooltipOpts: { content: "%p.0%, %s", defaultTheme: !1 } }, $scope.donutChart = {}, $scope.donutChart.data = [{ label: "Download Sales", data: 12 }, { label: "In-Store Sales", data: 30 }, { label: "Mail-Order Sales", data: 20 }, { label: "Online Sales", data: 19 }], $scope.donutChart.options = { series: { pie: { show: !0, innerRadius: .5 } }, legend: { show: !0 }, grid: { hoverable: !0, clickable: !0 }, colors: [$scope.color.primary, $scope.color.success, $scope.color.info, $scope.color.warning, $scope.color.danger], tooltip: !0, tooltipOpts: { content: "%p.0%, %s", defaultTheme: !1 } }, $scope.donutChart2 = {}, $scope.donutChart2.data = [{ label: "Download Sales", data: 12 }, { label: "In-Store Sales", data: 30 }, { label: "Mail-Order Sales", data: 20 }, { label: "Online Sales", data: 19 }, { label: "Direct Sales", data: 15 }], $scope.donutChart2.options = {
        series: { pie: { show: !0, innerRadius: .45 } }, legend: { show: !1 }, grid: { hoverable: !0, clickable: !0 }, colors: ["#1BB7A0", "#39B5B9", "#52A3BB", "#619CC4", "#6D90C5"],
        tooltip: !0, tooltipOpts: { content: "%p.0%, %s", defaultTheme: !1 }
    }
}])

app.controller('myC', function ($scope) {

});
