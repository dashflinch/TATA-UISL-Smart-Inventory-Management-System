// ===============================
// Toggle Password Visibility
// ===============================

function togglePassword(inputId, eyeId) {

    const passwordInput = document.getElementById(inputId);
    const eyeIcon = document.getElementById(eyeId);

    if (!passwordInput || !eyeIcon) {
        return;
    }

    if (passwordInput.type === "password") {

        passwordInput.type = "text";

        eyeIcon.classList.remove("fa-eye");
        eyeIcon.classList.add("fa-eye-slash");

    }
    else {

        passwordInput.type = "password";

        eyeIcon.classList.remove("fa-eye-slash");
        eyeIcon.classList.add("fa-eye");

    }
}

// ===============================
// Auto Focus First Input
// ===============================

document.addEventListener("DOMContentLoaded", () => {

    const firstInput = document.querySelector("input");

    if (firstInput) {
        firstInput.focus();
    }

});

// ===============================
// Disable Button After Submit
// ===============================

document.addEventListener("DOMContentLoaded", () => {

    const forms = document.querySelectorAll("form");

    forms.forEach(form => {

        form.addEventListener("submit", () => {

            const submitButton = form.querySelector("button[type='submit']");

            if (submitButton) {

                submitButton.disabled = true;

                submitButton.innerHTML =
                    '<i class="fa-solid fa-spinner fa-spin"></i> Please wait...';

            }

        });

    });

});