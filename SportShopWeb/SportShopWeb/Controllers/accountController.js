webShopApp.controller('accountController', ['$scope', '$rootScope', '$http', '$location', '$cookieStore', 'cartService', 'wishlistService', 'Notification', function ($scope, $rootScope, $http, $location, $cookieStore, cartService, wishlistService, Notification) {
    if ($cookieStore.get('productsView') == undefined) {
        $cookieStore.put('productsView', 'card');
    }

    if ($rootScope.isLoggedUser()) {
        $rootScope.accessProductsPage();
    }

    $scope.recoverDisabled = false;

    $scope.showLogin = true;
    $scope.showRegister = false;
    $scope.showRecover = false

    $scope.switchLogin = function () {
        $scope.showLogin = true;
        $scope.showRegister = false;
        $scope.showRecover = false
    }

    $scope.switchRegister = function () {
        $scope.showLogin = false;
        $scope.showRegister = true;
        $scope.showRecover = false
    }

    $scope.switchRecover = function () {
        $scope.showLogin = false;
        $scope.showRegister = false;
        $scope.showRecover = true
    }

    $scope.login = function (username, password) {
        var userInfo = {};
        userInfo.username = username;
        userInfo.password = password;

        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'users/login',
            data: userInfo
        }).then(
            function success(response) {
                var user = response.data;
                $cookieStore.put("loggedUser", user.ID);
                $cookieStore.put("isAdmin", user.IsAdmin);

                cartService.init();
                wishlistService.init();

                $rootScope.accessProductsPage();
            },
            function error(message) {
                $scope.message = "Invalid username or password";
            });
    }

    $scope.addUser = function (user, verifPass) {
        if (user.Password == verifPass) {
            user.IsAdmin = false;
            $http({
                method: 'POST',
                url: $rootScope.serviceURL + 'users',
                data: user
            }).then(function successCallback(response) {
                Notification.success("Account registered");
                $scope.switchLogin();
            }, function errorCallback(response) {
                Notification.error("Fail to register: " + response.data);
            });
        } else {
            Notification.error("Passwords must be the same");
        }
    };

    $scope.recover = function (email) {
        $scope.recoverDisabled = true;

        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'users/recover',
            data: { 'Email': email }
        }).then(function successCallback(response) {
            $scope.email = null;
            $scope.recoverDisabled = false;
            Notification.success("We sent you a recovery email");
            $scope.switchLogin();
        }, function errorCallback(response) {
            $scope.recoverDisabled = false;
            Notification.error("Failed to send recovery email: " + response.data);
        });
    };
}]);