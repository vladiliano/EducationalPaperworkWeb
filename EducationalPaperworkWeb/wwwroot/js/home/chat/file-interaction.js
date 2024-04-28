function AddFile() {
    var fileInput = document.createElement('input');
    fileInput.type = 'file';
    fileInput.style.display = 'none';
    $(fileInput).click();

    $(fileInput).change(function () {
        var formData = new FormData();
        formData.append('file', $(this)[0].files[0]);
        formData.append('userId', senderId);
        formData.append('chatId', selectedChatId);

        $.ajax({
            url: '/Home/UploadFile',
            type: 'POST',
            processData: false,
            contentType: false,
            data: formData
        });
    });
}

document.addEventListener('DOMContentLoaded', function () {
    document.querySelector('.msg_card_body').addEventListener('click', function (event) {
        if (event.target && event.target.matches('#file-link')) {
            event.preventDefault();
            const contentValue = event.target.getAttribute('data-content');
            var formData = new FormData();
            formData.append('fileName', contentValue);

            $.ajax({
                url: '/Home/DownloadFile',
                type: 'POST',
                processData: false,
                contentType: false,
                data: formData,
                xhrFields: {
                    responseType: 'blob'
                },
                success: function (data, textStatus, xhr) {
                    var url = window.URL.createObjectURL(data);
                    var a = document.createElement('a');
                    a.href = url;
                    const fileName = contentValue.split('*')[1];
                    a.download = fileName;
                    document.body.appendChild(a);
                    a.click();
                    window.URL.revokeObjectURL(url);
                    document.body.removeChild(a);
                }
            });
        }
    });
});