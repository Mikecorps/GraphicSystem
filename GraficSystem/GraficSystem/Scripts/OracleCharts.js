var dataToGraphics = [];
var keys = [];
var module = angular.module('oracleTab', ['checklist-model']);
module.controller('view', function ($scope, $http) {
    $scope.tables = null;
    $scope.table;
    $scope.names = [];
    $scope.columns = [];
    $scope.load = function () {
        $http.get('getTables')
        .then(function succesCallback(response) {
            $scope.tables = response.data;
            angular.forEach($scope.tables, function (value, key) {
                this.push({ id: key, name: value.key });
            }, $scope.names);

        });
    };
    $scope.loadTables = function () {
        dataToGraphics = [];
        $scope.columns = [];
        $scope.rows = [];
        angular.forEach($scope.tables[$scope.table].values, function (value, key) {
            $scope.columns.push({ val: value });
            keys.push(value);
        });
        console.log(keys);
        var parameters = { table: $scope.tables[$scope.table].key };
        var config = { params: parameters };
        $http.get('getData', config)
        .then(function succesCallback(response) {
            angular.forEach(response.data, function (value, key) {
                this.push(value);
            }, $scope.rows);


            angular.forEach($scope.rows, function (value, key) {
                var o = {};
                for (var i = 0; i < value.value.length; i++) {

                    o[$scope.columns[i].val] = value.value[i];
                }
                dataToGraphics.push(o);
            });

        });
    }
    $scope.load();
});

module.controller('graphics', function ($scope) {
    $scope.chart = null;
    $scope.config = {};
    $scope.heads = keys;
    $scope.typeOptions = ["line", "bar", "spline", "step", "area"];
    $scope.config.type = $scope.typeOptions[0];
    $scope.value = {
        heads: []
    };
    $scope.show = function () {
        var config = {};
        config.bindto = '#chart';
        config.data = {};
        config.data.json = dataToGraphics;
        config.data.keys = { "value": $scope.value.heads };
        config.data.type = $scope.config.type;
        config.zoom = { enabled: true };
        $scope.chart = c3.generate(config);
    }
});