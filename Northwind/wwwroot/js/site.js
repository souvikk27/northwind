document.addEventListener("DOMContentLoaded", () => {
    // Collapse menu toggle
    const toggleOpen = document.getElementById('toggleOpen');
    const toggleClose = document.getElementById('toggleClose');
    const collapseMenu = document.getElementById('collapseMenu');

    const handleClick = () => {
        collapseMenu.style.display = collapseMenu.style.display === 'block' ? 'none' : 'block';
    };

    toggleOpen.addEventListener('click', handleClick);
    toggleClose.addEventListener('click', handleClick);

    // Dropdown toggle logic
    const dropdownToggles = document.querySelectorAll(".dropdown-toggle");

    dropdownToggles.forEach(toggle => {
        toggle.addEventListener("click", event => {
            event.stopPropagation();
            const dropdownMenu = toggle.nextElementSibling;
            dropdownMenu.classList.toggle("hidden");
        });
    });

    // Handle clicks on dropdown items
    document.querySelectorAll('.dropdown-menu a').forEach(item => {
        item.addEventListener('click', event => {
            event.stopPropagation();
            // Your action handling code here
            console.log('Dropdown item clicked:', event.target.textContent);
        });
    });

    // Close dropdowns when clicking outside
    window.addEventListener("click", event => {
        if (!event.target.matches(".dropdown-toggle")) {
            document.querySelectorAll(".dropdown-menu").forEach(menu => {
                if (!menu.contains(event.target)) {
                    menu.classList.add("hidden");
                }
            });
        }
    });

    // User Menu popup implementation
    const profileButton = document.getElementById('profileButton');
    const profilePopup = document.getElementById('profilePopup');

    profileButton.addEventListener('click', () => {
        profilePopup.classList.toggle('hidden');
        profilePopup.classList.toggle('block', !profilePopup.classList.contains('hidden'));
    });

    // Close popup when clicking outside
    window.addEventListener('click', event => {
        if (!profilePopup.contains(event.target) && !profileButton.contains(event.target)) {
            profilePopup.classList.remove('block');
            profilePopup.classList.add('hidden');
        }
    });
});
