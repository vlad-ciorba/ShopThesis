webShopApp.controller('orderController', ['$scope', '$route', '$rootScope', '$q', '$http', '$cookieStore', 'cartService', 'wishlistService', 'Notification', function ($scope, $route, $rootScope, $q, $http, $cookieStore, cartService, wishlistService, Notification) {
    if (!$rootScope.isAdmin()) {
        $location.path("/");
    }

    function getUsers() {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'users'
        })
            .then(function successCallback(response) {
                $scope.users = response.data;
                deferred.resolve($scope.users);
            }, function errorCallback(response) {
                deferred.reject("Users not loaded" + response);
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
        getUsers()
            .then(function () {
                return getProducts();
            })
            .then(function () {
                return getOrderStatuses();
            })
            .then(function () {
                $http({
                    method: 'GET',
                    url: $rootScope.serviceURL + 'orders'
                }).then(function successCallback(response) {
                    $scope.orders = response.data;

                    angular.forEach($scope.orders, function (order, key) {
                        $scope.orders[key]["status"] = $scope.orderStatuses.filter(function (orderStatus) { return orderStatus.ID == order.StatusID })[0].Status;
                        order.user = $scope.users.filter(function (u) { return u.ID == order.UserID })[0];

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

    getOrders();

    $scope.changeOrderStatus = function (order, status) {
        $http({
            method: 'PUT',
            url: $rootScope.serviceURL + 'orders/' + order.ID,
            data: { 'ID': order.ID, 'StatusID': status.ID }
        }).then(function successCallback(response) {
            Notification.success("Order status changed");
            angular.forEach($scope.orders, function (o, key) {
                if (order.ID == o.ID) {
                    $scope.orders[key].StatusID = status.StatusID;
                    $scope.orders[key].status = status.Status;
                }
            });
        }, function errorCallback(response) {
            Notification.error("Error changing order status");
        });
    };

    $scope.calcTotalPrice = function (products) {
        var price = 0;
        angular.forEach(products, function (product) {
            price += product.price;
        });
        return price;
    };

    $scope.acceptOrder = function (order) {
        //$http({
        //    method: 'DELETE',
        //    url: $rootScope.serviceURL + '',
        //    data: order.user.email
        //}).then(function successCallback(response) {
        //    $scope.orders = $scope.orders.filter(function (ord) {
        //        return ord.id != order.id;
        //    });
        //}, function errorCallback(response) {
        //    alert("Order can't be accepted" + response);
        //});
    };

    $scope.acceptAllOrders = function () {
        //var orders = $scope.orders;
        //angular.forEach(orders, function (order) {
        //    $http({
        //        method: 'DELETE',
        //        url: $rootScope.serviceURL + '',
        //        data: order.user.email
        //    }).then(function successCallback(response) {
        //        $scope.orders = $scope.orders.filter(function (ord) {
        //            return ord.id != order.id;
        //        });
        //    }, function errorCallback(response) {
        //        alert("Order" + order.id + " can't be accepted" + response);
        //    });
        //});
    };

    $scope.rejectOrder = function (order) {
        //$http({
        //    method: 'DELETE',
        //    url: $rootScope.serviceURL + '',
        //    data: order.user.email
        //}).then(function successCallback(response) {
        //    $scope.orders = $scope.orders.filter(function (ord) {
        //        return ord.id != order.id;
        //    });
        //}, function errorCallback(response) {
        //    alert("Orders can't be deleted" + response);
        //});
    };

    $scope.rejectAllOrders = function () {
        //var orders = $scope.orders;
        //angular.forEach(orders, function (order) {
        //    $http({
        //        method: 'DELETE',
        //        url: $rootScope.serviceURL + '',
        //        data: order.user.email
        //    }).then(function successCallback(response) {
        //        $scope.orders = $scope.orders.filter(function (ord) {
        //            return ord.id != order.id;
        //        });
        //    }, function errorCallback(response) {
        //        alert("Order" + order.id + " can't be deleted" + response);
        //    });
        //});
    };
}]);