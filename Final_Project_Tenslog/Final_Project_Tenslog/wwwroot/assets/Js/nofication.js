let connection = new signalR.HubConnectionBuilder().withUrl("/nofication").build();
connection.start();
console.log(connection)


connection.on("ReciveNotifyForFollow", function myfunction(message, type, userId) {
    $('.notificationIcon').addClass(message);
    $('.notificationIcon').removeClass('d-none');
    setTimeout(function () {
        $('.notificationIcon').addClass('d-none');
        $('.notificationIcon').removeClass(message);
    }, 5000);
    if (type == 2) {
        fetch('/Nofications/AddNofication?userId=' + userId + '?notificationType=2')
            .then(res => {
                return res.text()
            })
            .catch(err => {
                console.log(err)
            })
    }
})