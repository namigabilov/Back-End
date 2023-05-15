connection = new signalR.HubConnectionBuilder().withUrl("/ChatBox").build();
connection.start();
console.log("chat hub connected")
$(document).ready(function () { 
    connection.on("ReceiveMessage", function (message) {
        // Gelen mesajı "forRealTimeChat" divine ekleme
        var chatDiv = $("#forRealTimeChat");
        chatDiv.append('<li>' + message + '</li>');
    });

    // SignalR bağlantısını başlatma
    connection.start()
        .then(function () {
            // Bağlantı başarılı
            console.log("SignalR bağlantısı başarıyla kuruldu.");
        })
        .catch(function (error) {
            // Bağlantı hatası
            console.error("SignalR bağlantısı kurulurken bir hata oluştu: " + error);
        });

        var messageInput = $("#realTimeChatInput");
        var message = messageInput.val();

        // Mesajı sunucuya gönderme
        connection.invoke("SendMessage", message)
            .catch(function (error) {
                console.error("Mesaj gönderilirken bir hata oluştu: " + error);
            });

        messageInput.val(""); 
    });
})