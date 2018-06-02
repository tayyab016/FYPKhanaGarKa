
// rating stars ajax
$("#rateYo").rateYo({
    starWidth: "20px",
    fullStar: true,
    onSet: function (rating, rateYoInstance) {
        $.ajax({
            url: '/Order/RatingManag/',
            type: "POST",
            data: {Id: $(this).data("c"), CRating: rating },
            success: function (data) {
                if (data == "OK") {
                    window.alert("Thank you for yor rating!");
                }
                else if (data == "Login") {
                    window.alert("Please, Login to rate.");

                } else {
                    window.alert("We are having some problem in rating.");

                }
            }
        });

    }
});

//order recive and order dispatched and order confirm ajax
$('.order-receive-btn').click(function () {
    var justclicked = $(this);
    $.ajax({

        beforeSend: function () {
            justclicked.text('wait...');

        },
        url: '/Order/OrderDisRec',
        type: 'POST',
        data: {
            Id: justclicked.data('o'),
            Role: 'Customer'
        },
        success: function (responsedata) {
            if (responsedata == "OK") {
                justclicked.fadeOut(2000);
            }
        },
        error: function () {
            window.alert("Some Error Occured ");
        }
    });
});

$('.order-deliver-btn').click(function () {
    var justclicked = $(this);
    $.ajax({

        beforeSend: function () {
            justclicked.text('wait...');

        },
        url: '/Order/OrderDisRec',
        type: 'POST',
        data: {
            Id: justclicked.data('o'),
            Role: 'Chef'
        },
        success: function (responsedata) {
            if (responsedata == "OK") {
                justclicked.fadeOut(2000);
            }
        },
        error: function () {
            window.alert("Some Error Occured ");
        }
    });
});

$('.order-confirm-btn').click(function () {
    var justclicked = $(this);
    $.ajax({

        beforeSend: function () {
            justclicked.text('wait...');

        },
        url: '/Order/OrderConfirm',
        type: 'POST',
        data: {
            Id: justclicked.data('o')
        },
        success: function (responsedata) {
            if (responsedata == "OK") {
                justclicked.fadeOut(2000);
            }
        },
        error: function () {
            window.alert("Some Error Occured ");
        }
    });
});

$('.order-cancel-btn').click(function () {
    var justclicked = $(this);
    $.ajax({
        url: '/Order/OrderCancel',
        type: 'POST',
        data: {
            Id: justclicked.data('o')
        },
        success: function (responsedata) {
            if (responsedata == "OK") {
                justclicked.prev('button').remove();
                justclicked.after('Canceled');
                justclicked.remove();

            }
        },
        error: function () {
            window.alert("Some Error Occured ");
        }
    });
});

// thumbs up and thumbs down handling ajax
$('.dish-rating').likeDislike({
    reverseMode: true,
    likeBtnClass: 'dish-like',
    dislikeBtnClass: 'dish-dislike',
    click: function (value, l, d, event) {
        var likes = $(this.element).find('.dish-like-text');
        var dislikes = $(this.element).find('.dish-dislike-text');

        likes.text(parseInt(likes.text()) + l);
        dislikes.text(parseInt(dislikes.text()) + d);

        if (l == 1) {
            $(this.element).find('.dish-like').removeClass('fa fa-thumbs-o-up');
            $(this.element).find('.dish-like').addClass('fa fa-thumbs-up');

        }
        else if (l == -1) {
            $(this.element).find('.dish-like').removeClass('fa fa-thumbs-up');
            $(this.element).find('.dish-like').addClass('fa fa-thumbs-o-up');
        }
        if (d == 1) {
            $(this.element).find('.dish-dislike').removeClass('fa fa-thumbs-o-down');
            $(this.element).find('.dish-dislike').addClass('fa fa-thumbs-down');

        }
        else if (d == -1) {
            $(this.element).find('.dish-dislike').removeClass('fa fa-thumbs-down');
            $(this.element).find('.dish-dislike').addClass('fa fa-thumbs-o-down');
        }
        $.ajax({
            url: '/Order/Voting/',
            type: 'POST',
            data: { MenuId: $(this.element).data("m"), ChefId: $(this.element).data("c"), Likes: parseInt(likes.text()), Dislikes: parseInt(dislikes.text()) },
            success: function (data) {
                if (data == "OK") {
                    window.alert("Thank you for yor review!");
                }
                else if (data == "Login") {
                    window.alert("Please, Login first.");
                }
            }
        });
    }
});
