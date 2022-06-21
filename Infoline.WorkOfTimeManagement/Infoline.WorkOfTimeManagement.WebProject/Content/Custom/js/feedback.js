
function feedback(feedback) {

    if (feedback == "" || feedback == null || feedback == "null") return false;

    if (feedback == 'SERVER') {
        feedback = { action: '', status: 'error', timeout: 20, message: 'Sunucu ile bağlantı kurulamıyor. Lütfen tekrar deneyin.', title: 'Sunucu Bağlantı Problemi !' };
    }

    var feedbackObj = feedback;

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": feedbackObj.timeout * 1000,
        "extendedTimeOut": 0,
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
        "tapToDismiss": false
    }

    if (typeof feedbackObj.action != "undefined" && feedbackObj.action != null && feedbackObj.action != "") {
        toastr.options.onHidden = function (a) { location.href = feedbackObj.action; }
    } else {
        //bura grid refresh metodu ile güncelleneccek sayfaya refresh atılmıcak
        toastr.options.onHidden = function (a) {
            //location.reload(true);
        }
    }

    if (feedbackObj.message != "" && feedbackObj.status != "") {
        toastr[feedbackObj.status](feedbackObj.message, feedbackObj.title);
    }

}
function MesajSuccess(mesaj, title) {
    if (typeof title === "undefined" && title === null) {
        title = "İşlem Başarılı";
    }
    feedback(JSON.parse(('{"action":"","title":"' + title + '","message":"' + mesaj + '","status":"success","timeout":8}').replace(/\n/g, '<br />')));
}
function MesajError(mesaj) {
    feedback(JSON.parse(('{"action":"","title":"Sistem Uyarısı","message":"' + mesaj + '","status":"error","timeout":8}').replace(/\n/g, '<br />')));
}
function MesajWarning(mesaj, title) {
    if (typeof title === "undefined") {
        title = "İşlem Eksik Gerçekleşti";
    }
    feedback(JSON.parse(('{"action":"","title":"' + title + '","message":"' + mesaj + '","status":"warning","timeout":8}').replace(/\n/g, '<br />')));
}
function MesajInfo(mesaj, title) {
    if (typeof title === "undefined") {
        title = "İşlem Eksik Gerçekleşti";
    }
    feedback(JSON.parse(('{"action":"","title":"' + title + '","message":"' + mesaj + '","status":"info","timeout":8}').replace(/\n/g, '<br />')));
}


function GetDataFromUrl(url, _data, returnF, beforeF, compF) {

    $.ajax({
        dataType: 'JSON',
        type: 'POST',
        async: true,
        timeout: 600000,
        url: url,
        data: (typeof (_data) != 'undefined' ? _data : null),
        beforeSend: function () {
            if (typeof (beforeF) == 'function') { beforeF(); }
        },
        success: function (res) {
            if (typeof (res) == 'undefined' || res == null || res == '') {
                feedback("SERVER");
                returnF(null);
            } else {

                if (res.hasOwnProperty("FeedBack")) {
                    feedback(res.FeedBack);
                };

                if (res.hasOwnProperty("Result")) {
                    returnF(res.Object);
                };

                if (res.result) {
                    returnF(res.objects);
                } else {
                    feedback(res.objects);
                }
            }
        },
        complete: function () {
            if (typeof (compF) == 'function') { compF(); }
        },
        error: function () {
            returnF(null);
        }
    });
}

