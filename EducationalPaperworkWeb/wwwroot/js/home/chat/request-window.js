function setModal() {
    var html = getCreateRequestFormHtml();
    hideMessageForm();
    hideClosedRequestMessage();
    $('.msg_card_body').html(html);
}

function myModalOpen() {
    $('#myModal').css('display', 'block');
    $('#textInput').focus();
}

function processTextInput() {
    var textInputValue = $('#textInput').val();
    if (textInputValue.length < 5 || textInputValue.length > 25) {
        $('#errorText').text('Тема звернення повина мати більше 5 символів.');
        return;
    }
    createChat(textInputValue);
    $('#myModal').css('display', 'none');
}