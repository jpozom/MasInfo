var showModalLoading = function () {
    var opts = {
        lines: 13 // The number of lines to draw
        , length: 28 // The length of each line
        , width: 14 // The line thickness
        , radius: 42 // The radius of the inner circle
        , scale: 1 // Scales overall size of the spinner
        , corners: 1 // Corner roundness (0..1)
        , color: '#000' // #rgb or #rrggbb or array of colors
        , opacity: 0.25 // Opacity of the lines
        , rotate: 0 // The rotation offset
        , direction: 1 // 1: clockwise, -1: counterclockwise
        , speed: 1 // Rounds per second
        , trail: 60 // Afterglow percentage
        , fps: 20 // Frames per second when using setTimeout() as a fallback for CSS
        , zIndex: 2e9 // The z-index (defaults to 2000000000)
        , className: 'spinner' // The CSS class to assign to the spinner
        , top: '50%' // Top position relative to parent
        , left: '50%' // Left position relative to parent
        , shadow: false // Whether to render a shadow
        , hwaccel: false // Whether to use hardware acceleration
        , position: 'fixed' // Element positioning
    };

    var spinner = new Spinner(opts).spin();
    var target = document.getElementById('divModalLoading');
    $(target).html('');
    $(target).append(spinner.el);
    $(target).show();
};

var hideModalLoading = function () {
    var target = document.getElementById('divModalLoading');
    $(target).html('');
    $(target).hide();
};

var showMessageDialog = function (type, title, message) {
    BootstrapDialog.show({
        type: type,
        title: 'masInfo - ' + title,
        message: message,
        buttons: [{
            label: 'Aceptar',
            cssClass: 'btn-success',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
};

var showMessageDialogRedirect = function (type, title, message, url) {
    BootstrapDialog.show({
        type: type,
        title: 'F22 - ' + title,
        message: message,
        buttons: [{
            label: 'Aceptar',
            cssClass: 'btn-success',
            action: function (dialog) {
                dialog.close();
                document.location.href = url;
            }
        }]
    });
};

var showAlertMessage = function (type, message) {
    var divAlert = document.getElementById("divAlertMessage");
    divAlert.innerHTML = `<div class="alert alert-${type} alert-dismissible fade in">` +
        `<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">×</span></button>` +
        `${message}</div>`;
    divAlert.style.display = 'block';
};

var logoutConfirmation = function (url) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_INFO,
        title: 'masInfo - Cerrar Sesión',
        message: '¿Esta seguro(a) que desea cerrar su sesión?',
        buttons: [{
            label: 'Aceptar',
            cssClass: 'btn-primary',
            action: function (dialog) {
                dialog.close();
                document.location.href = url;
            }
        }, {
            label: 'Cancelar',
            cssClass: 'btn-default',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
};

var confirmationDialog = function (title, message, callback) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_INFO,
        title: `masInfo - ${title}`,
        message: message,
        buttons: [{
            label: 'Aceptar',
            cssClass: 'btn-primary',
            action: function (dialog) {
                dialog.close();
                callback();
            }
        }, {
            label: 'Cancelar',
            cssClass: 'btn-default',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
};

var doPaginador = function (valor, callback) {
    var pagActual = parseInt($('#PaginaActual').val());
    var totalPags = parseInt($('#CantidadPaginas').val());
    var doCallback = false;
    if (valor === 'previous') {
        if (pagActual > 1) {
            pagActual--;
            doCallback = true;
        }
    }
    else if (valor === 'next') {
        if (pagActual < totalPags) {
            pagActual++;
            doCallback = true;
        }
    }
    else if (valor === 'page') {
        pagActual = 1;
        doCallback = true;
    }

    if (doCallback) {
        $('#PaginaActual').val(pagActual);
        callback();
    }
};

var updatePaginador = function (datosPag) {
    $('#DetallePaginaActual').text($('#PaginaActual').val() + '/' + datosPag.CantidadPaginas);
    $('#CantidadPaginas').val(datosPag.CantidadPaginas);
    $('#CantidadRegistros').text(datosPag.CantidadRegistros);
};

var bindPagerControlToEvents = function (callback) {
    // Asignamos el evento click al botón "Anterior" del paginador
    $('#btnPagerPrevious').attr("href", `javascript:doPaginador('previous', ${callback})`);
    // Asignamos el evento click al botón "Siguiente" del paginador
    $('#btnPagerNext').attr("href", `javascript:doPaginador('next', ${callback})`);
    // Asignamos el evento onchange al dropdownlist "Tamaño Página" del paginador
    $('#TamanoPagina').on("change", function () { doPaginador('page', callback) });
};

var showPrivacidad = function (url) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_INFO,
        title: 'MasInfo Términos y Condiciones',
        message: function (dialog) {
            var $message = $('<div></div>');
            var pageToLoad = dialog.getData('pageToLoad');
            $message.load(pageToLoad);

            return $message;
        },
        data: {
            'pageToLoad': url
        },
        size: BootstrapDialog.SIZE_WIDE,
        buttons: [{
            label: 'Aceptar',
            cssClass: 'btn-default',
            action: function (dialog) {
                dialog.close();
            }
        }]
    });
};
