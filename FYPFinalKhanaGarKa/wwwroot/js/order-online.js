$(document).ready(function(){
    "use strict";

    /*------------------------------------------/
    /* STICKY CATEGORY MENU AND CART ON SCROLL
    /*-----------------------------------------*/

    $(".sticky-content-left-side").sticky({topSpacing: 15});
    //$("#order-online-tab-nav").sticky({topSpacing: 10});

    /*------------------------/
     /* SMOOTH SCROLLING
     /*-----------------------*/

    //$('.scroll-section').bind('click', function(event) {
    //    var $anchor = $(this);
    //    $('html, body').stop().animate({
    //        scrollTop: $($anchor.attr('href')).offset().top - 10
    //    }, 1500, 'easeInOutExpo');
    //    event.preventDefault();
    //});


    /*--------------------------------------/
    /* HIDE CATRGORY MENU ON ITEM CLICK
    /*-------------------------------------*/

    $('#mobile-category-menu .navbar-collapse ul li a').click(function() {
        $('#mobile-category-menu .navbar-toggle:visible').click();
    });

    /*----------------------------------------/
    /* TOGGLE CART ON MOBILE VIEW
    /*---------------------------------------*/

    $('#mobileCartToggle').on('click', function(e){
        e.preventDefault();
        $('.ui-order-online-right-side').toggleClass('cart-visible');
        return false;
    });

    /*-------------------------------------------/
    /* HILIGHT CATEGORY MENU ITEM ON SCROLL
    /*------------------------------------------*/

     $('body').scrollspy({
        target: ".order-list",
        offset: 20
    });

     /*-----------------------------------------/
     /* ON SCROLL CHANGE CATEGORY TITLE
     /*----------------------------------------*/
     
     $(window).on('scroll', function(){
         var scrollTop = $(this).scrollTop();
         $('#mobile-category-menu .navbar-brand').html($('#mobile-category-menu li.active a').html());

        /*------ Toggle Show/Hide Dish Category Mobile Menu ------*/
         if( scrollTop > $('.ui-order-online-left-side').offset().top ) {
            $('#mobile-category-menu').addClass('visible');
         } else {
             $('#mobile-category-menu').removeClass('visible');
         }
     })


    /*------------------------------------------/
     /* STICKY WORKING ABOVE 768PX
     /*-----------------------------------------*/
    if( $(window).width() > 992 ){
        $(".sticky-content-right-side").sticky({topSpacing: 10});
        //$('.right-sidebar-inner-content').addClass('sticky-content-right-side');
    }

    $('.orderOnlineTab').on( 'click', function (event) {
        $(this).parent().siblings().removeClass('active');
        $(this).parent().addClass('active');

        if($(this).attr('id') == 'orderonlineOfferTab') {
            $('.orderonline-menu').hide();
            $('.orderonline-offer').show();
        } else {
            $('.orderonline-menu').show();
            $('.orderonline-offer').hide();
        }
        event.preventDefault();
    } );
});