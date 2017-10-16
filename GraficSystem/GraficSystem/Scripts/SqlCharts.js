
var vm;
var jsonData = [];
var Tables = angular.module('table', ['checklist-model']);
Tables.controller('view', function ($scope, $http) {
    $scope.DBs = [];
    $scope.db1;
    $scope.tables = [];
    $scope.tab;
    $scope.colums = [];
    $scope.heads = [];
    $scope.completeData = null;
    $scope.dataRows = [];
    $scope.viewdata = function () {
        $http.get('getdata')
        .then(function successCallback(response) {
            $scope.completeData = response.data;
            angular.forEach($scope.completeData, function (value, key) {
                this.push({ id: key, name: value['Key'] });
            }, $scope.DBs);
        }, function errorCallback(response) {

        });

    };
    $scope.getTables = function () {
        $scope.tables = [];
        if ($scope.db1 != null) {
            angular.forEach($scope.completeData[$scope.db1]['Value'], function (value, key) {
                this.push({ id: key, name: value['Key'] });
            }, $scope.tables);
        }

    };
    $scope.fields = function () {
        $scope.colums = [];
        jsonData = [];
        $scope.heads = [];
        $scope.dataRows = [];
        if ($scope.tab != null) {
            angular.forEach($scope.completeData[$scope.db1]['Value'][$scope.tab]['Value'], function (value, key) {
                this.push({ id: key, val: value });
            }, $scope.heads);
            vm = $scope.heads;
            angular.forEach($scope.heads, function (value, key) {
                this.push([value.val]);
            }, $scope.colums);

            var parameters = {
                table: $scope.tables[$scope.tab].name,
                catalog: $scope.DBs[$scope.db1].name
            };
            var config = { params: parameters };
            $http.get('getTables', config)
            .then(function succesCallback(response) {
                angular.forEach(response.data, function (value, key) {
                    this.push(value);
                }, $scope.dataRows);
                angular.forEach($scope.dataRows, function (value, key) {
                    var o = {};
                    for (var i = 0; i < value.Value.length; i++) {
                        o[$scope.heads[i].val] = value.Value[i];
                    }
                    jsonData.push(o);
                });
                colums = $scope.colums;
            }, function errorCallback(response) {
            });
        }
    }
    $scope.viewdata();
});

Tables.controller('charts', function ($scope) {
    //Configuracio
    $scope.chart = null;
    $scope.config = {};
    $scope.heads = ['upload', 'download', 'total'];
    $scope.typeOptions = ["line", "bar", "spline", "step", "area"];
    $scope.config.type1 = $scope.typeOptions[0];
    $scope.value = {
        heads: ['total']
    };
    //funciones
    $scope.show = function () {
        $scope.heads = [];
        angular.forEach(vm, function (value, key) {
            this.push(value.val);
        }, $scope.heads);
        var SCO = $scope.value;
        var config = {};
        config.bindto = '#d3';
        config.data = {};
        config.data.json = jsonData;
        config.data.keys = { "value": $scope.value.heads };
        config.axis = { "y": { "label": { "text": "Numero de items", "position": "outer-middle" } } };
        config.data.type = $scope.config.type1;
        config.zoom = { enabled: true };
        $scope.chart = c3.generate(config);
    }
});