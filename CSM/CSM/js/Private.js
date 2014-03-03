var AjaxHandler = "/AjaxHandler.ashx"

$(document).ready(function () {
	
	$(".modal").fancybox({
        fitToView: true,
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeBtn: true,
        padding: 40,
        closeEffect: 'none'
    });

    $(".iframe").fancybox({
        fitToView: true,
        autoSize: true,
        closeClick: false,
        openEffect: 'none',
        closeBtn: true,
        padding: 40,
        iframe: true,
        closeEffect: 'none'
    });

    $(".ctcnameinput").labeledInput({ label: "Nombre de contacto" });
    $(".ctcemailinput").labeledInput({ label: "Email de contacto", isEmail: true });
    $(".ctcsubjinput").labeledInput({ label: "Asunto" });
    $(".ctcmessageinput").labeledInput({ label: "Mensaje" });

	$("#txtpublication").labeledInput({ label: "Suelta tu improperio..." });
	
});

function clearHTMLStrips(obj) {
    obj.value = obj.value.replace(/<(?:.|\n)*?>/gm, '')
}

function setImageLoad(responseLabel) {

    $("#" + responseLabel).html('<img src="images/ajax-loader.gif" alt="loading" />');

}

function SendNewComment(obj) {
    
    $.ajax({
        url: AjaxHandler,
        type: "POST",
        async: true,
        data: {
            fn: "NewComment",
            userid: $("#hdnuserid").val(),
            comment: encodeURI($(obj).prev().val()),
            ruleid: $(obj).next().val(),
            publid: $(obj).next().next().val()
        },
        beforeSend: function (xhr) {
            if ($(obj).prev().val() == "") {
                jsAlert("Por favor, introduce un texto");
                $('.preventDouble').enableSubmit();
                return false;
            }

            $('.preventDouble').preventDoubleSubmit();
        },
        success: function (data) {
            switch (data.substring(0, 2)) {
                default:
                case "0:":
                    jsAlert(data.substring(2, data.length));
                    break;
                case "1:":
                    document.location.href = document.location.href;
                    break;
            }
        },
        complete: function () {
            $('.preventDouble').enableSubmit();
        },
        error: function () {
            jsAlert("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.", { type: error });
        },
        statusCode: {
            404: function () {
                //TODO: Informar error 404
                jsAlert("Se ha solicitado una petición que no puede ser procesada por el servicio. Por favor, inténtelo más tarde.", { type: error });
            },
            500: function () {
                jsAlert("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.", { type: error });
                //TODO: Informar error 500
            }
        }
    });

}

function UpdateLinkStatus(userto, status) {

    $.ajax({
        url: AjaxHandler,
        type: "POST",
        async: true,
        data: {
            fn: "UpdateLinkStatus",
            userid: $("#hdnuserid").val(),
            userto: userto,
            status: status
        },
        beforeSend: function (xhr) {
            if (status == -1) {
                return confirm("¿Está seguro que quiere eliminar el enlace con este usuario?");
            }
            $('.preventDouble').preventDoubleSubmit();
        },
        success: function (data) {
            switch (data.substring(0, 2)) {
                default:
                case "0:":
                    jsAlert(data.substring(2, data.length));
                    break;
                case "1:":
                    __doPostBack("udpListPanel", "");
                    __doPostBack("udpLinkedPanel", "");
                    break;
            }
        },
        complete: function () {
            $('.preventDouble').enableSubmit();
        },
        error: function () {
            jsAlert("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.");
        },
        statusCode: {
            404: function () {
                //TODO: Informar error 404
                jsError("Se ha solicitado una petición que no puede ser procesada por el servicio. Por favor, inténtelo más tarde.");
            },
            500: function () {
                jsError("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.");
                //TODO: Informar error 500
            }
        }
    });

}

function ScheduleFinish(pschedid) {

    $.ajax({
        url: AjaxHandler,
        type: "POST",
        async: true,
        data: {
            fn: "FinishSchedule",
            schedid: pschedid
        },
        beforeSend: function (xhr) {
            $('.preventDouble').preventDoubleSubmit();
            return confirm("¿Está seguro que quiere dar por concluida la tarea/evento?");
            
        },
        success: function (data) {
            switch (data.substring(0, 2)) {
                default:
                case "0:":
                    jsAlert(data.substring(2, data.length));
                    break;
                case "1:":
                    __doPostBack("udpSchedulePanel", "RefreshSchedule");
                    break;
            }
        },
        complete: function () {
            $('.preventDouble').enableSubmit();
        },
        error: function () {
            jsAlert("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.");
        },
        statusCode: {
            404: function () {
                //TODO: Informar error 404
                jsError("Se ha solicitado una petición que no puede ser procesada por el servicio. Por favor, inténtelo más tarde.");
            },
            500: function () {
                jsError("Se ha producido un error al procesar su petición. Por favor, inténtelo más tarde.");
                //TODO: Informar error 500
            }
        }
    });

}


function alertWarning(msg, callback) {
    $().toastmessage('showToast', {text: msg,sticky: true,position: 'middle-center',type: 'warning',close: function() {
            if (callback != "") {
                eval(callback);
            }
        }});
}
function alertError(msg, callback) {
    $().toastmessage('showToast', {text: msg,sticky: true,position: 'middle-center',type: 'error',close: function() {
            if (callback != "") {
                eval(callback);
            }
        }});
}
function alertInfo(msg, callback) {
    $().toastmessage('showToast', {text: msg,sticky: true,position: 'middle-center',type: 'notice',close: function() {
            if (callback != "") {
                eval(callback);
            }
        }});
}