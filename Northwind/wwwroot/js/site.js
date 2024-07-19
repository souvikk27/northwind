var toggleOpen = document.getElementById('toggleOpen');
var toggleClose = document.getElementById('toggleClose');
var collapseMenu = document.getElementById('collapseMenu');

function handleClick() {
    if (collapseMenu.style.display === 'block') {
        collapseMenu.style.display = 'none';
    } else {
        collapseMenu.style.display = 'block';
    }
}

toggleOpen.addEventListener('click', handleClick);
toggleClose.addEventListener('click', handleClick);

toggleOpen.addEventListener('click', handleClick);
toggleClose.addEventListener('click', handleClick);

document.addEventListener("DOMContentLoaded", () => {
    const dropdownToggles = document.querySelectorAll(".dropdown-toggle");

    dropdownToggles.forEach((toggle) => {
        toggle.addEventListener("click", (event) => {
            event.stopPropagation();
            const dropdownMenu = toggle.nextElementSibling;
            dropdownMenu.classList.toggle("hidden");
        });
    });

    // Handle clicks on dropdown items
    document.querySelectorAll('.dropdown-menu a').forEach(item => {
        item.addEventListener('click', (event) => {
            event.stopPropagation();
            // Your action handling code here
            console.log('Dropdown item clicked:', event.target.textContent);
        });
    });

    // Close dropdowns when clicking outside
    window.addEventListener("click", (event) => {
        if (!event.target.matches(".dropdown-toggle")) {
            document.querySelectorAll(".dropdown-menu").forEach((menu) => {
                if (!menu.contains(event.target)) {
                    menu.classList.add("hidden");
                }
            });
        }
    });
});