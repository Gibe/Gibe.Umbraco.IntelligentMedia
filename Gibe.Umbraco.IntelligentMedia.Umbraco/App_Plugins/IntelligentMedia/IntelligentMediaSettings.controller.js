function intelligentMediaSettingsController($scope, $log, $http, notificationsService) {
	$http.get("/Umbraco/Api/IntelligentMediaSettings/settings").then(function (response) {
		$scope.settings = response.data;
	});

	$scope.save = function() {
		$http.post("/Umbraco/Api/IntelligentMediaSettings/settings", $scope.settings).then(function (response) {
			notificationsService.success("Settings saved");
			$scope.settingsForm.$dirty = false;
		});
	}
}

angular.module("umbraco").controller("Umbraco.Dashboard.IntelligentMediaSettingsController", intelligentMediaSettingsController);