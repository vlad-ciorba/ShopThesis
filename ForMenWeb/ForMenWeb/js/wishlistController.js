webShopApp.controller('wishlistController', ['$scope', '$rootScope', 'wishlistService', 'cartService', '$http', '$route', 'Notification', function ($scope, $rootScope, wishlistService, cartService, $http, $route, Notification) {

    $scope.wlProducts = wishlistService.getProducts();
    $scope.totalPrice = 0;

    angular.forEach($scope.wlProducts, function (product) {
        $scope.totalPrice += product.Price;
    });

    $scope.removeFromWishlist = function (product) {
        $http({
            method: 'PUT',
            url: $rootScope.serviceURL + 'wishlists/' + $rootScope.getLoggedUser(),
            data: { 'Products': [{ 'ProductID': product.ID }] }
        }).then(function successCallback(response) {
            wishlistService.removeProduct(product);
            $route.reload();
            Notification.success('Removed from wishlist');
        }, function errorCallback(response) {
            Notification.error('Error removing wishlist product');
        });
    }

    $scope.moveToCart = function (product) {
        $http({
            method: 'POST',
            url: $rootScope.serviceURL + 'carts',
            data: { 'UserID': $rootScope.getLoggedUser(), 'Products': [{ 'ProductID': product.ID, 'Quantity': 1 }] }
        }).then(function successCallback(response) {
            cartService.addProduct(product);
            wishlistService.removeProduct(product);
            Notification.success('Moved to cart');
            $scope.removeFromWishlist(product);
            $route.reload();
        }, function errorCallback(response) {
            Notification.error('Error moving to cart');
        });
        $route.reload();
    }
}]);