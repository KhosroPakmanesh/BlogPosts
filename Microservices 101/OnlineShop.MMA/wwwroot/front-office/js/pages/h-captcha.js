"use strict";

(function () {
    setInterval(hCaptchaChangeDetectorCallback, 1000);
})();
function initializeHCaptcha(sitekey) {   
    $('#hCaptchaWrapper')
        .append(`<div class="h-captcha" data-sitekey="${sitekey}"></div>`);        
}
function hCaptchaChangeDetectorCallback() {
    const hCaptchaValue = $('[name=h-captcha-response]').val();
    if (hCaptchaValue === "" || hCaptchaValue === undefined) {
        $('#submitButton').prop('disabled', true);
    }
    else {
        $('#submitButton').prop('disabled', false);
    }
}
