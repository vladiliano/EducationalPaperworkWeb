var previousClickedBtn = null;
var table;

function getDateFormHtml(text) {
    return `
        <div class="msg_date">
            ${text}
        </div>
    `;
}

function getWaitFormHtml() {
    return `
        <div id="hello-world" class="hello-form" style="font-size: 15px; display: flex;">
            Очікується підтвердження звернення від представника дирекції університету
        </div>
    `;
}

function getClientGreetingsFormHtml() {
    return `
    <div class="hello-form">
            Оберіть чат або надішліть звернення для його створення
        </div>
    `;
}

function getCreateRequestFormHtml() {
    return `
    <div id="myModal" class="modal">
        <div class="modal-content">
            <span id="close-modal" class="close">&times;</span>
            <p class="modal-text">Вкажіть тему звернення</p>
            <input type="text" id="textInput" class="modal-input" minlength="5" maxlength="25">
            <span id="errorText" class="text-danger validation-message"></span>
            <button id="sendButton" class="modal-button">Надіслати</button>
        </div>
    </div>
    `;
}

function getTextMessageHtml(content, time, messageLocation) {
    return `
        <div class="d-flex ${messageLocation} mb-4">
            <div class="msg_container" id="mess">
                ${content}
                <span class="msg_time">${time}</span>
            </div>
        </div>
    `;
}

function getFileMessageHtml(content, time, messageLocation) {
    fileName = content.split('*')[1];
    return `
        <div class="d-flex ${messageLocation} mb-4">
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

function hideMessageForm() {
    $('#sendMessageForm').css('visibility', 'hidden');
    $('#card-header').css('visibility', 'hidden');
}

function showMessageForm() {
    $('#sendMessageForm').css('visibility', 'visible');
    $('#card-header').css('visibility', 'visible');
}