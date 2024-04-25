function sendMessage() {
    var messageContent = $('#inputMessage').val();

    $.ajax({
        url: '/Home/SendMessage',
        method: 'POST',
        data: {
            userId: senderId,
            chatId: selectedChatId,
            mess: messageContent
        }
    });
}

function appendMessageToChat(data) {
    if (data.chatId == selectedChatId) {
        var messageHtml = createMessageHtml(data);
        $('.msg_card_body').append(messageHtml);
        $('#inputMessage').val('');

        var element = document.querySelector('.msg_card_body');
        element.scrollTop = element.scrollHeight;
    }
}

function createMessageHtml(data) {
    var currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit' });
    var todayLabel = addTodayLabel(data);
    var messageHtml = '';

    if (todayLabel) {
        messageHtml += getDateFormHtml("Сьогодні");
    }

    var messageLocation = data.messageData.senderId.toString() === senderId ? "justify-content-end" : "justify-content-start";

    if (data.isFile) {
        return messageHtml += getFileMessageHtml(data.messageData.content, currentTime, messageLocation);
    } else {
        return messageHtml += getTextMessageHtml(data.messageData.content, currentTime, messageLocation)
    }
}

function addTodayLabel(data) {
    if (data.previousMessageExist) {
        var todayDate = new Date().getDate();
        var mess = new Date(data.previousMessageTimeStamp);
        var messDate = mess.getDate();
        return messDate !== todayDate ? true : false;
        return false;
    }
    return true;
}