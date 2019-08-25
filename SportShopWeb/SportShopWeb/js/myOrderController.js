webShopApp.controller('myOrderController', ['$scope', '$route', '$rootScope', '$q', '$http', '$cookieStore', 'cartService', 'wishlistService', 'Notification', function ($scope, $route, $rootScope, $q, $http, $cookieStore, cartService, wishlistService, Notification) {
    getOrders();

    function getUser(ID) {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'users/' + ID
        })
            .then(function successCallback(response) {
                $scope.user = response.data;
                deferred.resolve($scope.user);
            }, function errorCallback(response) {
                deferred.reject("User not loaded" + response);
            });

        return deferred.promise;
    }

    function getOrderStatuses() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'orders/statuses'
        })
            .then(function successCallback(response) {
                $scope.orderStatuses = response.data;
                deferred.resolve($scope.orderStatuses);
            }, function errorCallback(response) {
                deferred.reject("Order statuses not loaded" + response);
            });

        return deferred.promise;
    }

    function getProducts() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'products'
        })
            .then(function successCallback(response) {
                $scope.products = response.data;
                deferred.resolve($scope.products);
            }, function errorCallback(response) {
                deferred.reject("Products not loaded" + response);
            });

        return deferred.promise;
    }

    function getOrders() {
        getUser($rootScope.getLoggedUser())
            .then(function () {
                return getOrderStatuses();
            })
            .then(function () {
                return getProducts();
            })
            .then(function () {
                $http({
                    method: 'GET',
                    url: $rootScope.serviceURL + 'orders/' + $rootScope.getLoggedUser()
                }).then(function successCallback(response) {
                    $scope.orders = response.data;

                    angular.forEach($scope.orders, function (order, key) {
                        $scope.orders[key]["status"] = $scope.orderStatuses.filter(function (orderStatus) { return orderStatus.ID == order.StatusID })[0].Status;

                        angular.forEach(order.Products, function (product, key) {
                            var prod = $scope.products.filter(function (p) { return p.ID == product.ProductID })[0];

                            if (prod) {
                                order.Products[key]["name"] = prod.Name;
                            }
                            else {
                                order.Products[key]["name"] = "[deleted from store]";
                            }
                        });
                    });
                }, function errorCallback(response) {
                    console.warn("Orders not loaded" + response);
                });
            });
    };

    $scope.calcTotalPrice = function (products) {
        var price = 0;
        angular.forEach(products, function (product) {
            price += product.price;
        });
        return price;
    };

    $scope.changeOrderStatus = function (order, status) {
        if (confirm("Are you sure?"))
            $http({
                method: 'PUT',
                url: $rootScope.serviceURL + 'orders/' + order.ID,
                data: { 'ID': order.ID, 'StatusID': status }
            }).then(function successCallback(response) {
                Notification.success("Order status changed");
                angular.forEach($scope.orders, function (o, key) {
                    if (order.ID == o.ID) {
                        $scope.orders[key].StatusID = status;
                        $scope.orders[key].status = $scope.orderStatuses.filter(function (s) { return s.ID == status })[0].Status;
                    }
                });
            }, function errorCallback(response) {
                Notification.error("Error in changing order status");
            });
    };
}]);