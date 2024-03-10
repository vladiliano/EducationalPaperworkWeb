function initializeGroupInputValidation(inputId) {
    var groupInput = document.getElementById(inputId);

    groupInput.addEventListener('input', function () {
        var inputValue = groupInput.value;

        inputValue = inputValue.trim();

        if (/^\d{0,3}-?[A-Za-zА-Яа-я]?$/.test(inputValue)) {
            if (inputValue.length === 3 && !inputValue.includes('-')) {
                groupInput.value = inputValue + '-';
            }

            if (inputValue.length > 3) {
                groupInput.value = inputValue.substring(0, 4) + inputValue.substring(4).toLowerCase();
            }
        } else {
            groupInput.value = inputValue.substring(0, inputValue.length - 1);
        }

        if (inputValue.length > 5) {
            groupInput.value = inputValue.substring(0, 5);
        }
    });
}