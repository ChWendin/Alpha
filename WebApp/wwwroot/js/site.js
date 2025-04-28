document.addEventListener('DOMContentLoaded', () => {
    // --- Dropdown‐logik: öppna/stäng menyer ---
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]');
    document.addEventListener('click', event => {
        let clickedDropdown = null;

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);
            if (dropdown.contains(event.target)) {
                clickedDropdown = targetElement;
                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show');
                    }
                });
                targetElement.classList.toggle('dropdown-show');
            }
        });

        if (!clickedDropdown && !event.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show')
                .forEach(openDropdown => openDropdown.classList.remove('dropdown-show'));
        }
    });


    // --- Modal‐öppnare (t.ex. Add Project) ---
    const modals = document.querySelectorAll('[data-type="modal"]');
    modals.forEach(btn => {
        btn.addEventListener('click', () => {
            const targetId = btn.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);
            targetElement.classList.add('modal-show');
        });
    });

    // --- Modal‐stängare ---
    const closeButtons = document.querySelectorAll('[data-type="close"]');
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const targetId = button.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);
            targetElement.classList.remove('modal-show');
        });
    });


    // --- Custom select‐dropdown ---
    document.querySelectorAll('.form-select').forEach(select => {
        const trigger = select.querySelector('.form-select-trigger');
        const triggerText = trigger.querySelector('.form-select-text');
        const options = select.querySelectorAll('.form-select-option');
        const hiddenInput = select.querySelector('input[type="hidden"]');
        const placeholder = select.dataset.placeholder || "Choose";

        const setValue = (value = "", text = placeholder) => {
            triggerText.textContent = text;
            hiddenInput.value = value;
            select.classList.toggle('has-placeholder', !value);
        };
        setValue();

        trigger.addEventListener('click', e => {
            e.stopPropagation();
            document.querySelectorAll('.form-select.open')
                .forEach(el => el !== select && el.classList.remove('open'));
            select.classList.toggle('open');
        });

        options.forEach(option => {
            option.addEventListener('click', () => {
                setValue(option.dataset.value, option.textContent);
                select.classList.remove('open');
            });
        });

        document.addEventListener('click', e => {
            if (!select.contains(e.target)) {
                select.classList.remove('open');
            }
        });
    });


    // --- AJAX‐driven Edit Project‐modal ---
    document.addEventListener('click', async e => {
        const editBtn = e.target.closest('.edit-project');
        if (!editBtn) return;

        const projectId = editBtn.getAttribute('data-id');
        const modal = document.querySelector('#edit-project-modal');
        const container = modal.querySelector('#edit-project-form-container');

        try {
            const resp = await fetch(`/Projects/Edit/${projectId}`);
            if (!resp.ok) throw new Error(`Status ${resp.status}`);

            container.innerHTML = await resp.text();
            modal.classList.add('modal-show');
        } catch (err) {
            console.error('Kunde inte ladda Edit-formuläret:', err);
        }
    });
});
