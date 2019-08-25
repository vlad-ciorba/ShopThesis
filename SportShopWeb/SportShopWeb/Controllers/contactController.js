webShopApp.controller('contactController', ['$scope', '$rootScope', '$route', '$http', '$cookieStore', 'Notification', function ($scope, $rootScope, $route, $http, $cookieStore, Notification) {
    $scope.sent = false;

    $scope.sendMail = function (contactDetails) {
        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'contacts',
            data: contactDetails
        }).then(function successCallback(response) {
            $scope.contact = null;
            $scope.sent = true;
            Notification.success("Email sent!");
            $scope.switchLogin();
        }, function errorCallback(response) {
            Notification.error("Failed to send email: " + response.data);
        });
    };
}]);