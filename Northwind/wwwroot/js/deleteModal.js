// JavaScript to handle modal open and close
document.addEventListener('DOMContentLoaded', function () {
    const modal = document.getElementById('modal');
    let currentRoleId = null;

    // Event delegation for delete buttons
    document.addEventListener('click', function (e) {
        if (e.target.closest('.deleteRoleBtn')) {
            const deleteBtn = e.target.closest('.deleteRoleBtn');
            currentRoleId = deleteBtn.getAttribute('data-role-id');
            modal.classList.remove('hidden');
        }
    });

    // Close modal
    document.getElementById('closeModalBtn').addEventListener('click', function () {
        modal.classList.add('hidden');
    });

    document.getElementById('closeModalBtnFooter').addEventListener('click', function () {
        modal.classList.add('hidden');
    });

    // Confirm delete
    document.getElementById('confirmDeleteBtn').addEventListener('click', function () {
        if (currentRoleId) {
            
                fetch('/Roles/Delete/' + currentRoleId, {
                    method: 'DELETE',
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                            window.location.reload();
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        alert('Error deleting role. Please try again.');
                    })
                    .finally(() => {
                        modal.classList.add('hidden');
                    });
        }
    });
});