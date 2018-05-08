$(document).ready(function(){
    "use strict";
    $("#filterMenuTop").on('click', function(){
        $('#mobileFilter').slideToggle();
    });
    $(window).load(function() {
        $(".loading-overlay .spinner").fadeOut(300), $(".loading-overlay").fadeOut(300);
        $("body").css({
            overflow: "auto",
            height: "auto",
            position: "relative"
        })
    });
    /*----------------------------
     window on scroll
     ------------------------------ */
    var winScroll = $(window).scrollTop();
    winScroll > 1 ? $("#to-top").css({
        bottom: "10px"
    }) : $("#to-top").css({
        bottom: "-100px"
    }), $(window).on("scroll", function() {
        winScroll = $(window).scrollTop(), winScroll > 1 ? $("#to-top").css({
            opacity: 1,
            bottom: "58px"
        }) : $("#to-top").css({
            opacity: 0,
            bottom: "-100px"
        })
    }), $("#to-top").click(function() {
        return $("html, body").animate({
            scrollTop: "0px"
        }, 800), !1
    }),
        (new WOW).init();




});