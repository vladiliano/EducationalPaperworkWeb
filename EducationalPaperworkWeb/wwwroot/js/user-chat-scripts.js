function addMessage(messageContent) {
    var currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit' });

    var newMessageHtml = `
        <div class="d-flex justify-content-end mb-4">
            <div class="msg_container" id="mess">
                ${messageContent}
                <span class="msg_time">${currentTime}</span>
            </div>
        </div>
    `;

    $('.msg_card_body').append(newMessageHtml);
    $('#inputMessage').val('');
}

function updateChat(senderId, messages) {
    var newMessageHtml = '';

    messages.forEach(function (message) {
        var messageTime = new Date(message.timeStamp);
        var hours = messageTime.getHours();
        var minutes = messageTime.getMinutes();
        var formattedTime = (hours < 10 ? '0' : '') + hours + ':' + (minutes < 10 ? '0' : '') + minutes;

        var messageHtml = `
        <div class="d-flex ${(message.senderId == senderId) ? "justify-content-end" : "justify-content-start"} mb-4">
            <div class="msg_container" id="mess">
                ${message.content}
                <span class="msg_time">${formattedTime}</span>
            </div>
        </div>
        `;
        newMessageHtml += messageHtml;
    });

    $('.msg_card_body').html(newMessageHtml);
}