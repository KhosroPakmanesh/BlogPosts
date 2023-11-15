"use strict";

function submitContact() {
    if (!$("#contactForm").valid()) {
        return;
    }

    const url = $("#contactForm").attr("action");
    const requestVerificationToken = $('[name=__RequestVerificationToken]').val();
    const hCaptchaResponse = $('[name=h-captcha-response]').val();
    const fullNameValue = $('#FullName').val();
    const emailValue = $('#Email').val();
    const webAddressValue = $('#WebAddress').val();
    const bodyValue = $('#Body').val();

    const formData = new FormData();
    formData.append("__RequestVerificationToken", requestVerificationToken);
    formData.append("h-captcha-response", hCaptchaResponse);    
    formData.append("fullName", fullNameValue);
    formData.append("email", emailValue);
    formData.append("webAddress", webAddressValue);
    formData.append("body", bodyValue);

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            toastr.options.closeButton = true;
            toastr.options.timeOut = 10000;
            toastr.options.preventDuplicates = true;
            toastr.options.progressBar = true;
            toastr.success('Congratulations. Your contact message was saved successfully. I will contact you as soon as I can.');
        },
        error: function (req, status, error) {
            toastr.options.closeButton = true;
            toastr.options.timeOut = 10000;
            toastr.options.preventDuplicates = true;
            toastr.options.progressBar = true;
            toastr.error('Unfortunately, your contact message was not saved successfully. Please submit your message again later.');
        }
    });
}
