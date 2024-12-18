const cookieName = "cart-items";

function addToCart(id, name, price, picture, slug) {
    let products = $.cookie(cookieName);
    if (products === undefined) {
        products = [];
    } else {
        products = JSON.parse(products);
    }

    let count = $("#productCount").val();
    count = count === "" ? 0 : parseInt(count);
    const currentProduct = products.find(x => x.id === id);


    if (currentProduct !== undefined) {
        products.find(x => x.id === id).count = (currentProduct.count || 0) + parseInt(count);
    } else {
        const product = {
            id,
            name,
            unitPrice: price,
            picture,
            slug,
            count
        }
        products.push(product);
    }

    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function updateCart() {
    let products = $.cookie(cookieName);

    if (!products) {
        // If no products in the cookie, clear the cart UI and return
        $("#cart_items_count").text(0);
        $("#cart_items_wrapper").html('<p>سبد خرید خالی است.</p>'); // Display empty cart message
        return;
    }

    try {
        products = JSON.parse(products);
    } catch (error) {
        console.error("Error parsing products from cookie:", error);
        return;
    }

    // Ensure `count` is valid for all products
    products.forEach(product => {
        if (!product.count || product.count === "") {
            product.count = 0; // Set invalid or empty count to 0
        } else {
            product.count = parseInt(product.count); // Convert valid count to number
            if (isNaN(product.count)) {
                product.count = 0; // Fallback if parsing fails
            }
        }
    });

    // Update cart item count
    $("#cart_items_count").text(products.length);

    // Clear cart items wrapper
    const cartItemsWrapper = $("#cart_items_wrapper");
    cartItemsWrapper.empty();

    // Render each product in the cart
    products.forEach(product => {
        const productHtml = `
            <div class="single-cart-item">
                <a href="javascript:void(0)" class="remove-icon" onclick="removeFromCart('${product.id}')">
                    <i class="ion-android-close"></i>
                </a>
                <div class="image">
                    <a href="single-product.html">
                        <img src="/ProductPictures/${product.picture}"
                             class="img-fluid" alt="${product.name}">
                    </a>
                </div>
                <div class="content">
                    <p class="product-title">
                        <a href="single-product.html">محصول: ${escapeHtml(product.name)}</a>
                    </p>
                    <p class="count">تعداد: ${product.count ?? 0}</p>
                    <p class="count">قیمت واحد: ${product.unitPrice}</p>
                </div>
            </div>`;

        cartItemsWrapper.append(productHtml);
    });
}

// Utility function to escape HTML to prevent XSS
function escapeHtml(text) {
    const map = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;',
        "'": '&#039;'
    };
    return text.replace(/[&<>"']/g, m => map[m]);
}



function removeFromCart(id) {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    const itemToRemove = products.findIndex(x => x.id === id);
    products.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();
}

function changeCartItemCount(id, totalId, count) {
    // Parse the products cookie
    let products = $.cookie(cookieName);

    if (!products) {
        console.error("No products in the cart.");
        return;
    }

    try {
        products = JSON.parse(products);
    } catch (error) {
        console.error("Error parsing products from cookie:", error);
        return;
    }

    // Validate and normalize count
    count = parseInt(count);
    if (isNaN(count) || count <= 0) {
        alert("تعداد کالا باید بیشتر از صفر باشد."); // Notify the user
        return;
    }

    // Find the product in the cart
    const productIndex = products.findIndex(x => x.id == id);
    if (productIndex === -1) {
        console.error(`Product with ID ${id} not found in the cart.`);
        return;
    }

    // Update product count and price
    products[productIndex].count = count;
    const product = products[productIndex];
    const newPrice = parseInt(product.unitPrice) * count;
    $(`#${totalId}`).text(newPrice);

    // Update the cookie and refresh the cart UI
    $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
    updateCart();

    // Prepare AJAX request payload
    const requestData = {
        productId: parseInt(id),
        count: count
    };

    // AJAX settings
    const settings = {
        url: "https://localhost:7210/api/inventory",
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        data: JSON.stringify(requestData)
    };

    // Send AJAX request to the backend
    $.ajax(settings)
        .done(function (data) {
            const warningsDiv = $('#productStockWarnings');

            // Handle stock warnings
            if (!data.isStock) {
                if ($(`#${id}`).length === 0) {
                    warningsDiv.append(`
                        <div class="alert alert-warning" id="${id}">
                            <i class="fa fa-warning"></i> کالای
                            <strong>${data.productName}</strong>
                            در انبار کمتر از تعداد درخواستی موجود است.
                        </div>
                    `);
                }
            } else {
                $(`#${id}`).remove(); // Remove warning if stock is sufficient
            }
        })
        .fail(function () {
            alert("خطایی رخ داده است. لطفا با مدیر سیستم تماس بگیرید.");
        });
}



function checkCartValidation() {
    console.log("call checking")
    let products = $.cookie(cookieName);

    if (!products) {
        alert("سبد خرید شما خالی است!");
        return false;
    }

    try {
        products = JSON.parse(products);
    } catch (error) {
        console.error("Error parsing products from cookie:", error);
        alert("خطایی در بارگذاری سبد خرید رخ داده است. لطفاً دوباره تلاش کنید.");
        return false;
    }

    const invalidProducts = products.filter(product => (!product.count || product.count.trim() === "" || parseInt(product.count) <= 0)
    });

    if (invalidProducts.length > 0) {
        alert("تعداد برخی کالاها در سبد خرید معتبر نیست. لطفاً مقادیر را بررسی کنید.");
        return false;
    }

    return true;
}


