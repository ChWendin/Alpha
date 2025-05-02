document.addEventListener("DOMContentLoaded", function () {
    // Hitta just “Add Project”-formuläret
    const form = document.querySelector("form.add-project-form");
    if (!form) return;

    // Hitta alla input och textarea som har data-val="true"
    const fields = form.querySelectorAll("input[data-val='true'], textarea[data-val='true']");

    // Valideringsfunktion för ett enskilt fält
    const validateField = field => {
        const errorSpan = document.querySelector(`span[data-valmsg-for='${field.name}']`);
        if (!errorSpan) return true;

        let errorMessage = "";
        const value = field.value.trim();

        // Required-validering
        if (field.hasAttribute("data-val-required") && value === "") {
            errorMessage = field.getAttribute("data-val-required");
        }

        // Regex-validering
        if (!errorMessage && field.hasAttribute("data-val-regex") && value !== "") {
            const pattern = new RegExp(field.getAttribute("data-val-regex-pattern"));
            if (!pattern.test(value)) {
                errorMessage = field.getAttribute("data-val-regex");
            }
        }

        if (errorMessage) {
            field.classList.add("input-validation-error");
            errorSpan.classList.remove("field-validation-valid");
            errorSpan.classList.add("field-validation-error");
            errorSpan.textContent = errorMessage;
            return false;
        } else {
            field.classList.remove("input-validation-error");
            errorSpan.classList.remove("field-validation-error");
            errorSpan.classList.add("field-validation-valid");
            errorSpan.textContent = "";
            return true;
        }
    };

    // Kör live‐validering vid varje input‐händelse
    fields.forEach(field =>
        field.addEventListener("input", () => validateField(field))
    );

    // På submit: validera alla fält, avbryt POST om något fält är fel
    form.addEventListener("submit", function (e) {
        console.log("Add-project-form submit triggat");
        let allValid = true;
        fields.forEach(field => {
            if (!validateField(field)) {
                allValid = false;
            }
        });

        if (!allValid) {
            e.preventDefault();
            // Håll modalen öppen när valideringen blockerar
            const modal = form.closest(".modal");
            if (modal) {
                modal.classList.add("modal-show");
            }
        }
    });
});
