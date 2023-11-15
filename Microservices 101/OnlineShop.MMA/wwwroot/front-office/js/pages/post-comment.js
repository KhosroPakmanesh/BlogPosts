"use strict";

function submitComment() {
	if (!$("#commentForm").valid()) {
		return;
	}

	const url = $("#commentForm").attr("action");
	const requestVerificationToken = $('[name=__RequestVerificationToken]').val();
	const hCaptchaResponse = $('[name=h-captcha-response]').val();
	const idPost = $('#PostModel_IdPost').val();
	const selectedCommentId = $('#SelectedCommentId').val();
	const fullNameValue = $('#CommentBindingModel_CommenterFullName').val();
	const emailValue = $('#CommentBindingModel_CommenterEmail').val();
	const webAddressValue = $('#CommentBindingModel_CommenterWebAddress').val();
	const bodyValue = $('#CommentBindingModel_CommenterCommentBody').val();

	const formData = new FormData();
	formData.append("__RequestVerificationToken", requestVerificationToken);
	formData.append("h-captcha-response", hCaptchaResponse);    
	formData.append("PostModel.IdPost", idPost);
	formData.append("SelectedCommentId", selectedCommentId);
	formData.append("CommentBindingModel.CommenterFullName", fullNameValue);
	formData.append("CommentBindingModel.CommenterEmail", emailValue);
	formData.append("CommentBindingModel.CommenterWebAddress", webAddressValue);
	formData.append("CommentBindingModel.CommenterCommentBody", bodyValue);

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
			toastr.success('Congratulations. Your comment message was saved successfully. Your comment will be published as soon as it is reviewed.');
		},
		error: function (req, status, error) {
			toastr.options.closeButton = true;
			toastr.options.timeOut = 10000;
			toastr.options.preventDuplicates = true;
			toastr.options.progressBar = true;
			toastr.error('Unfortunately, your comment message was not saved successfully. Please submit your comment again later.');
		}
	});
}