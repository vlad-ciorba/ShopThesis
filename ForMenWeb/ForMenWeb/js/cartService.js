webShopApp.service('cartService', ['$cookieStore', '$rootScope', '$http', '$q', function ($cookieStore, $rootScope, $http, $q) {
    return {
        init: function () {

            var getCart = function () {
                var deferred = $q.defer();

                $http({
                    method: 'GET',
                    url: $rootScope.serviceURL + 'carts/' + $cookieStore.get('loggedUser')
                }).then(function successCallback(response) {
                    var cartProdustIDs = response.data;
                    deferred.resolve(cartProdustIDs);
                }, function errorCallback(response) {
                    $cookieStore.remove("cart");
                    alert("Eroor loading cart");
                });

                return deferred.promise;
            }

            getCartProducts = function (cartProdustIDs) {
                var products = [];
                var promises = [];

                angular.forEach(cartProdustIDs, function (product) {
                    var deferred = $q.defer();

                    deferred.promise = $http({
                        method: 'GET',
                        url: $rootScope.serviceURL + 'products/' + product.ProductID
                    });

                    deferred.promise.then(function successCallback(response) {
                        product["Name"] = response.data.Name;
                        product["Price"] = response.data.Price;
                        product.ID = response.data.ID;
                        products.push(product);
                        deferred.resolve();
                    }, function errorCallback(response) {
                        deferred.reject("Product not loaded" + response);
                    });

                    promises.push(deferred.promise);
                });

                return $q.all(promises).then(function () {
                    return products;
                });
            }

            getCart().then(function (cart) {
                return getCartProducts(cart.Products);
            }).then(function (cartProducts) {
                $cookieStore.put('cart', cartProducts);
                // get cart size
                var cart = $cookieStore.get('cart');
                if (cart != undefined)
                    $rootScope.cartSize = cart.length;
            })
        },

        getProducts: function () {
            return $cookieStore.get('cart');
        },

        addProduct: function (product) {
            var cart = $cookieStore.get('cart');
            if (cart == undefined) {
                cart = [];
            }
            var exists = cart.filter(function (p) { return p.ID == product.ID }).length > 0;
            if (!exists) {

                var productCopy = product.constructor();
                for (var attr in product) {
                    if (product.hasOwnProperty(attr)) productCopy[attr] = product[attr];
                }

                productCopy.Quantity = 1;
                cart.push(productCopy);
                $cookieStore.remove("cart");
                $cookieStore.put('cart', cart);
            }
            else {
                for (var i = 0; i < cart.length; i++) {
                    if (cart[i].ID == product.ID) {
                        cart[i].Quantity++;
                        break;
                    }
                }
                $cookieStore.remove("cart");
                $cookieStore.put('cart', cart);
            }
            $rootScope.cartSize = this.getCartSize();
        },

        removeProduct: function (product) {
            if ($cookieStore.get('cart')) {
                var products = $cookieStore.get('cart').filter(function (p) { return p.ID != product.ID });
                $cookieStore.remove('cart');
                $cookieStore.put('cart', products);
            }
            $rootScope.cartSize = this.getCartSize();
        },

        updateProductQuantity: function (product, newQuantity) {
            if (newQuantity < 1) {
                this.removeProduct(product);
                return;
            }

            if ($cookieStore.get('cart')) {
                var products = $cookieStore.get('cart');
                for (var i = 0; i < products.length; i++) {
                    if (products[i].ID == product.ID) {
                        products[i].Quantity = newQuantity
                        break;
                    }
                }
                $cookieStore.remove('cart');
                $cookieStore.put('cart', products);
            }
            $rootScope.cartSize = this.getCartSize();
        },

        getTotalPrice: function () {
            var tPrice = 0;
            var products = $cookieStore.get('cart');
            angular.forEach(products, function (product) {
                tPrice += product.Price * product.Quantity;
            });
            return tPrice;
        },

        getCartSize: function () {
            var cart = $cookieStore.get('cart');
            if (cart != undefined) {
                return cart ? cart.length : 0;
            } else {
                return 0;
            }
        },

        clearCart: function () {
            $cookieStore.remove('cart');
        }
    }
}]);