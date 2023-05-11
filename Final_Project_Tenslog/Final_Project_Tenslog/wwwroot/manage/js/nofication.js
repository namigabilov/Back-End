let connection = new signalR.HubConnectionBuilder().withUrl("/nofication").build();
connection.start();

connection.on("ReciveNotify", function (message) {

    Swal.fire({
        position: 'top-end',
        icon: 'success',
        title: message,
        showConfirmButton: false,
        timer: 1500000
    })
    
})