function appendMessageToChat(data) {
    var todayLabel = addTodayLabel(data);

    var messageHtml = createMessageHtml(data, todayLabel);
    $('.msg_card_body').append(messageHtml);
    $('#inputMessage').val('');

    var element = document.querySelector('.msg_card_body');
    element.scrollTop = element.scrollHeight;
}

function createMessageHtml(data, todayLabel) {
    var currentTime = new Date().toLocaleTimeString('en-US', { hour12: false, hour: '2-digit', minute: '2-digit' });
    var messageHtml = '';

    if (todayLabel) {
        messageHtml += getDateMessageHtml("Сьогодні");
    }

    var messageLocation = data.senderId.toString() === senderId ? "justify-content-end" : "justify-content-start";

    if (data.isFile) {
        return messageHtml += getFileMessage(data.content, currentTime, messageLocation);
    } else {
        return messageHtml += getTextMessage(data.content, currentTime, messageLocation)
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
}

function getTextMessage(content, time, sender) {
    return `
        <div class="d-flex ${sender} mb-4">
            <div class="msg_container" id="mess">
                ${content}
                <span class="msg_time">${time}</span>
            </div>
        </div>
    `;
}

function getFileMessage(content, time, sender) {
    fileName = getFileName(content);
    return `
        <div class="d-flex ${sender} mb-4">
            <div class="msg_container file_container" id="mess">
                <span class="material-symbols-outlined file_icon">upload_file</span>
                    <a id="file-link" class="file_name" href="#" data-content="${content}">
                        ${fileName}
                    </a>
                <span class="msg_time">${time}</span>
            </div>
        </div>
    `;
}