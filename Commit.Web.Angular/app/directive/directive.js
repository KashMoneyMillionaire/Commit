"use strict";
angular.module('myApp.directives', [])
.directive("flotChart", [
            function () {
                return {
                    restrict: "A",
                    scope: {
                        data: "=",
                        options: "="
                    },
                    link: function (scope, ele) {
                        var data, options, plot;
                        return data = scope.data, options = scope.options, plot = $.plot(ele[0], data, options)
                    }
                }
            }
])

.directive("flotChartRealtime", [
            function () {
                return {
                    restrict: "A",
                    link: function (scope, ele) {
                        var data, getRandomData, plot, totalPoints, update, updateInterval;
                        return data = [], totalPoints = 300, getRandomData = function () {
                            var i, prev, res, y;
                            for (data.length > 0 && (data = data.slice(1)) ; data.length < totalPoints;) prev = data.length > 0 ? data[data.length - 1] : 50, y = prev + 10 * Math.random() - 5, 0 > y ? y = 0 : y > 100 && (y = 100), data.push(y);
                            for (res = [], i = 0; i < data.length;) res.push([i, data[i]]), ++i;
                            return res
                        }, update = function () {
                            plot.setData([getRandomData()]), plot.draw(), setTimeout(update, updateInterval)
                        }, data = [], totalPoints = 300, updateInterval = 200, plot = $.plot(ele[0], [getRandomData()], {
                            series: {
                                lines: {
                                    show: !0,
                                    fill: !0
                                },
                                shadowSize: 0
                            },
                            yaxis: {
                                min: 0,
                                max: 100
                            },
                            xaxis: {
                                show: !1
                            },
                            grid: {
                                hoverable: !0,
                                borderWidth: 1,
                                borderColor: "#eeeeee"
                            },
                            colors: ["#5B90BF"]
                        }), update()
                    }
                }
            }
])
    .directive("sparkline", [
            function () {
                return {
                    restrict: "A",
                    scope: {
                        data: "=",
                        options: "="
                    },
                    link: function (scope, ele) {
                        var data, options, sparkResize, sparklineDraw;
                        return data = scope.data, options = scope.options, sparkResize = void 0, sparklineDraw = function () {
                            return ele.sparkline(data, options)
                        }, $(window).resize(function () {
                            return clearTimeout(sparkResize), sparkResize = setTimeout(sparklineDraw, 200)
                        }), sparklineDraw()
                    }
                }
            }
    ])
.directive("uiJvectormap", [
            function () {
                return {
                    restrict: "A",
                    scope: {
                        options: "="
                    },
                    link: function (scope, ele) {
                        var options;
                        return options = scope.options, ele.vectorMap(options)
                    }
                }
            }
]);