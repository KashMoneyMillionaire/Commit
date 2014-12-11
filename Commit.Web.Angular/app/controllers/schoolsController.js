angular.module('myApp').controller('schoolsController', function ($scope, allData) {

    //grab all the data

    $scope.hello = "world";

    $scope.schools = [];
    $scope.selectedSchool;

    $scope.selectedSchoolData = [];
    
    $scope.myOptions = {
        chartArea: {
            background: "transparent"
        }
    };

    var styler = [{ "featureType": "all", "stylers": [{ "saturation": 0 }, { "hue": "#e7ecf0" }] }, { "featureType": "road", "stylers": [{ "saturation": -70 }] }, { "featureType": "transit", "stylers": [{ "visibility": "off" }] }, { "featureType": "poi", "stylers": [{ "visibility": "off" }] }, { "featureType": "water", "stylers": [{ "visibility": "simplified" }, { "saturation": -60 }] }];

    $scope.map = {
        center: {
            latitude: 32.833992,
            longitude: -96.791640
        },
        zoom: 16,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        styles: styler
    };

    allData.getData()
       .success(function (data) {
           $scope.schools = data;
           //$scope.schools = _.pluck(data, 'Id');
          // console.log($scope.schools);
           //console.log("data from schools");
           //console.log(data);
           //$scope.selectedSchool = data[0];

           //for (var i = 0; i < data.length; i++) {
           //    $scope.schools.push(data[i]["CAMPNAME"]);
           //}

           $scope.selectedSchool = data[157];
           $scope.selectedSchoolData = [
                { 'CPETBLAP': .5, 'CPETHISP': .5, 'CPETWHIP': .5, 'CPETECOP': .5, 'CPETLEPP': .5 }
           ];

           $scope.selectedSchoolData = [
             {
                 'CPETBLAP': parseFloat($scope.selectedSchool["CPETBLAP"]), 'CPETHISP': parseFloat($scope.selectedSchool["CPETHISP"]), 'CPETWHIP': parseFloat($scope.selectedSchool["CPETWHIP"]),
                 'CPETECOP': parseFloat($scope.selectedSchool["CPETECOP"]),
                 'CPETLEPP': parseFloat($scope.selectedSchool["CPETLEPP"])
             }
           ];

           console.log("schjools");
           console.log(data);


           createGrid();
           //$scope.$apply();

       })
       .error(function (error) {
           $scope.status = 'Unable to load radio data: ' + error.message;
       });





    $scope.campusChange = function () {

        console.log("selected school");
        console.log($scope.selectedSchool);

        //now change the charts based on this selected object
        $scope.hello = $scope.selectedSchool["CAMPNAME"];

        //have to parse int to do this
        //$scope.selectedSchoolData["CPEMALLP"] = parseFloat($scope.selectedSchool["CPEMALLP"]);  
        //$scope.selectedSchoolData["CPETHISP"] = parseFloat($scope.selectedSchool["CPETHISP"]);
        //$scope.selectedSchoolData["CPST01FP"] = parseFloat($scope.selectedSchool["CPST01FP"]);

        $scope.selectedSchoolData = [
         {
             'CPETBLAP': parseFloat($scope.selectedSchool["CPETBLAP"]), 'CPETHISP': parseFloat($scope.selectedSchool["CPETHISP"]), 'CPETWHIP': parseFloat($scope.selectedSchool["CPETWHIP"]),
          'CPETECOP': parseFloat($scope.selectedSchool["CPETECOP"]),
          'CPETLEPP': parseFloat($scope.selectedSchool["CPETLEPP"])
         }
        ];

        console.log("okay");
        console.log(parseFloat($scope.selectedSchool["CPEMALLP"]));

        //$scope.myChart.refresh();
        //$scope.myChart.dataSource.read();
    }

    //$scope.$watchCollection('selectedSchoolData', function (newData) {

    //    // Update data bindings with changes
    //    $scope.chart.dataSource.data = newData;
    //});

    function createGrid() {



        $("#schoolsGrid").kendoGrid({
            dataSource: {
                data: $scope.schools,
                schema: {
                    model: {
                        fields: {
                            CAMPUS: { type: "string" },
                            CAMPNAME: { type: "string" },
                            CPEMALLP: { type: "string" },
                            CPETHISP: { type: "string" }
                        }
                    }
                },
                pageSize: 20
            },
            height: 550,
            scrollable: true,
            sortable: true,
            filterable: true,
            pageable: {
                input: true,
                numeric: false
            },
            columns: [
                { field: "CAMPUS", width: "130px" },
                { field: "CAMPNAME", title: "Unit Price", format: "{0:c}", width: "130px" },
                { field: "CPETWHIP", title: "Units In Stock", width: "130px" },
                { field: "CPETHISP", width: "130px" }
            ]
        });
    }
    
});

angular.module('myApp').factory('allData', function ($http) {

    return {
        getData: function () {
            return $http.get("/api/sql");
        }
    };

});











//$scope.electricity = new kendo.data.DataSource({
//    transport: {
//        read: {
//            url: "../../spain-electricity.json",
//            dataType: "json"
//        }
//    },
//    sort: {
//        field: "year",
//        dir: "asc"
//    }
//});

//angular.module('myApp').factory('allData', ['$http',
//  function ($http) {
//      return {
//          getData: function (object, options) {
//              return $http.get("/api/sql").success(
//                function (results) {
//                    options.success(object);
//                });

//          }
//      }
//  }
//]);





//$scope.FruitAutoCompleteFromFactory = {
//    dataTextField: 'CAMPNAME',
//    dataSource: new kendo.data.DataSource({
//        transport: {
//            read: function () {
//                return allData.getData()

//            }
//        }
//    })
//}


//$scope.FruitAutoCompleteFromFactory = {
//    dataTextField: "'CAMPNAME'",
//    dataSource: new kendo.data.DataSource({
//        transport: {
//            read: function (options) {
//                return allData.getData($scope.schools, options)

//            }
//        }
//    })
//};