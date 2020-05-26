var sweetAlert = function (title, icon, message) {
    Swal.fire({
        icon: icon,
        title: title,
        text: message,
        showClass: {
            popup: 'animated fadeInDown faster'
        },
        hideClass: {
            popup: 'animated fadeOutUp faster'
        }
    });
};
