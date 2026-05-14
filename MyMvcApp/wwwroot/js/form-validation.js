(function () {
  function getMessage(input) {
    if (input.validity.valueMissing) {
      return "This field is required.";
    }
    if (input.validity.typeMismatch && input.type === "email") {
      return "Enter a valid email address.";
    }
    if (input.validity.tooLong) {
      return "This value is too long.";
    }
    if (input.validity.badInput) {
      return "Enter a valid value.";
    }
    return "";
  }

  function showMessage(input) {
    var message = getMessage(input);
    var messageTarget = document.querySelector('[data-valmsg-for="' + input.name + '"]');

    if (messageTarget) {
      messageTarget.textContent = message;
    }

    return message === "";
  }

  function wireForm(form) {
    var inputs = form.querySelectorAll("input, select, textarea");

    inputs.forEach(function (input) {
      input.addEventListener("input", function () {
        if (input.validity.valid) {
          showMessage(input);
        }
      });

      input.addEventListener("blur", function () {
        showMessage(input);
      });
    });

    form.addEventListener("submit", function (event) {
      var firstInvalid = null;

      inputs.forEach(function (input) {
        var isValid = showMessage(input);
        if (!isValid && firstInvalid === null) {
          firstInvalid = input;
        }
      });

      if (!form.checkValidity()) {
        event.preventDefault();
        if (firstInvalid) {
          firstInvalid.focus();
        }
      }
    });
  }

  document.querySelectorAll("form.js-validate").forEach(wireForm);
})();
