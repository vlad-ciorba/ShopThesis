webShopApp.controller('userController', ['$scope', '$rootScope', '$route', '$http', '$location', '$cookieStore', 'Notification', function ($scope, $rootScope, $route, $http, $location, $cookieStore, Notification) {

    $scope.editingUser = $scope.user;

    if (!$rootScope.isAdmin() && $location.path() == "/users") {
        $location.path("/");
    }

    $http({
        method: 'GET',
        url: $rootScope.serviceURL + 'users'
    }).then(function successCallback(response) {
        $scope.users = response.data;
        $scope.admins = $scope.users.filter(function (user) {
            return user.IsAdmin;
        });
        $scope.customers = $scope.users.filter(function (user) {
            return !user.IsAdmin;
        });
    }, function errorCallback(response) {
        Notification.error("Users not loaded" + response);
    });

    $scope.addUser = function (user, verifPass) {
        if (user.Password == verifPass) {
            switch ($scope.userRole) {
                case "user":
                    user.IsAdmin = false;
                    break;
                case "admin":
                    user.IsAdmin = true;
                    break;
                default:
                    user.IsAdmin = false;
            }
            $http({
                method: 'POST',
                url: $rootScope.serviceURL + 'users',
                data: user
            }).then(function successCallback(response) {
                Notification.success("User saved");
                if ($rootScope.isAdmin())
                    $location.path("users");
            }, function errorCallback(response) {
                Notification.error("Fail to save user: " + response.data);
            });
        } else {
            Notification.error("Passwords must be the same");
        }
    };

    $scope.editUser = function (user, verifPass) {
        if ((user.Password || verifPass) && user.Password != verifPass) {
            Notification.error("Passwords must be the same");
            return;
        }

        switch ($scope.userRole) {
            case "user":
                user.IsAdmin = false;
                break;
            case "admin":
                user.IsAdmin = true;
                break;
            default:
                user.IsAdmin = false;
        }
        $http({
            method: 'PUT',
            url: $rootScope.serviceURL + 'users/' + user.ID,
            data: user
        }).then(function successCallback(response) {
            Notification.success("User saved");
            if ($rootScope.getLoggedUser() == user.ID)
                $cookieStore.put("isAdmin", user.IsAdmin);
            if ($rootScope.isAdmin())
                $location.path("users");
        }, function errorCallback(response) {
            Notification.error("Fail to save user: " + response.data);
        });
    };

    $scope.deleteUser = function (user) {
        if (confirm("Are you sure?"))
            $http({
                method: 'DELETE',
                url: $rootScope.serviceURL + 'users/' + user.ID
            }).then(function successCallback(response) {
                Notification.success("User deleted");
                $route.reload();
            }, function errorCallback(response) {
                Notification.error("Fail to delete user: " + response.data);
            });
    };

    $scope.gotoEditUser = function (user) {
        $rootScope.user = user;
        $location.path("saveUser");
    };

    $scope.gotoNewUser = function () {
        $rootScope.user = undefined;
        $location.path("saveUser");
    };
}]);