$(document).ready(function () {
    "use strict";

    $('.approve-btn-chef').click(function () {
        var justclicked = $(this);
        $.ajax({

            beforeSend: function () {
                justclicked.text('Please wait...');

            },
            url: '/Admin/ApproveReq',
            type: 'POST',
            data: {
                Id: justclicked.data('cid'),
                Role: 'Chef'
            },
            success: function (responsedata) {
                if (responsedata == "OK") {
                    justclicked.fadeOut(2000);
                }
            },
            error: function () {
                window.alert("cannot approve request");
            }
        });
    });

    $('.disapprove-btn-chef').click(function () {
        var justclicked = $(this);
        $.ajax({

            beforeSend: function () {
                justclicked.text('Please wait...');

            },
            url: '/Admin/Disapprove',
            type: 'POST',
            data: {
                Id: justclicked.data('cid'),
                Role: 'Chef'
            },
            success: function (responsedata) {
                if (responsedata == "OK") {
                    justclicked.fadeOut(2000);
                }
            },
            error: function () {
                window.alert("cannot disapprove request");
            }
        });
    });

    $('.approve-btn-dboy').click(function () {
        var justclicked = $(this);
        $.ajax({

            beforeSend: function () {
                justclicked.text('Please wait...');

            },
            url: '/Admin/ApproveReq',
            type: 'POST',
            data: {
                Id: justclicked.data('did'),
                Role: 'DBoy'
            },
            success: function (responsedata) {
                if (responsedata == "OK") {
                    justclicked.fadeOut(2000);
                }
            },
            error: function () {
                window.alert("cannot approve request");
            }
        });
    });

    $('.disapprove-btn-dboy').click(function () {
        var justclicked = $(this);
        $.ajax({

            beforeSend: function () {
                justclicked.text('Please wait...');

            },
            url: '/Admin/Disapprove',
            type: 'POST',
            data: {
                Id: justclicked.data('did'),
                Role: 'DBoy'
            },
            success: function (responsedata) {
                if (responsedata == "OK") {
                    justclicked.fadeOut(2000);
                }
            },
            error: function () {
                window.alert("cannot disapprove request");
            }
        });
    });

});