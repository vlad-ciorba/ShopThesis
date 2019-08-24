webShopApp.controller('cartController', ['$scope', '$rootScope', '$q', '$route', '$http', '$location', '$cookieStore', 'cartService', 'Notification', function ($scope, $rootScope, $q, $route, $http, $location, $cookieStore, cartService, Notification) {

    $scope.totalPrice = cartService.getTotalPrice();
    $scope.cartProducts = cartService.getProducts();
    $scope.newQuantity = [];

    $scope.removeProductFromCart = function (product) {
        $http({
            method: 'PUT',
            url: $rootScope.serviceURL + 'carts/' + $rootScope.getLoggedUser(),
            data: { 'ID': $rootScope.getLoggedUser(), 'Products': [{ 'ProductID': product.ID, 'Quantity': 0 }] }
        }).then(function successCallback(response) {
            cartService.removeProduct(product);
            $scope.totalPrice = cartService.getTotalPrice();
            $route.reload();
            Notification.success('Removed from cart');
        }, function errorCallback(response) {
            Notification.error('Error removing cart product');
        });
    };

    $scope.updateQuantity = function (product, newQuantity) {
        if (newQuantity != null) {
            if (newQuantity <= 0 && !confirm("Remove product?")) {
                $scope.newQuantity[product.ID] = product.Quantity;
                return;
            }

            $http({
                method: 'PUT',
                url: $rootScope.serviceURL + 'carts/' + $rootScope.getLoggedUser(),
                data: { 'ID': $rootScope.getLoggedUser(), 'Products': [{ 'ProductID': product.ID, 'Quantity': newQuantity }] }
            }).then(function successCallback(response) {
                cartService.updateProductQuantity(product, newQuantity);
                $scope.cartProducts = cartService.getProducts();
                $scope.totalPrice = cartService.getTotalPrice();
                $route.reload();
                Notification.success('Quantity updated');
            }, function errorCallback(response) {
                Notification.error('Error updating quantity');
            });
        }
    }

    function getUser(ID) {
        var deferred = $q.defer();

        $http({
            method: 'GET',
            url: $rootScope.serviceURL + 'users/' + ID
        })
            .then(function successCallback(response) {
                deferred.resolve(response.data);
            }, function errorCallback(response) {
                deferred.reject("User not loaded" + response);
            });

        return deferred.promise;
    }

    $scope.addOrder = function () {

        getUser($rootScope.getLoggedUser()).then(function (user) {

            var address = prompt("Please enter your address:", user.Address);

            if (address) {

                var order = {};
                order.UserID = $rootScope.getLoggedUser();
                order.Address = address;
                order.Products = cartService.getProducts().map(function (product) {
                    return { 'ProductID': product.ID, 'Quantity': product.Quantity }
                });

                $http({
                    method: 'POST',
                    url: $rootScope.serviceURL + 'orders',
                    data: order
                }).then(
                    function success(data) {
                        cartService.clearCart();
                        $rootScope.cartSize = cartService.getCartSize();
                        Notification.success('Order placed');
                        $location.path("myOrders");
                    },
                    function error(response) {
                        Notification.error(response.data);
                    });
            }
        });
    }
}]);